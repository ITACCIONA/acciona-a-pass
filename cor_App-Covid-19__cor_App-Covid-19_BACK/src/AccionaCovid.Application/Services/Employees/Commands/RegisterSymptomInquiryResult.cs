using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using AccionaCovid.Domain.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Employees
{
    public class RegisterSymptomInquiryResult
    {
        public class RegisterSymptomInquiryResultRequest : IRequest<bool>
        {
            public int IdEmployee { get; set; }
            public DateTimeOffset CurrentDeviceDateTime { get; set; }
            public List<RegisterSymptom> Values { get; set; }
        }

        public class RegisterSymptom
        {
            public int Id { get; set; }
            public bool Value { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class RegisterSymptomInquiryResultValidator : AbstractValidator<RegisterSymptomInquiryResultRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public RegisterSymptomInquiryResultValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
                RuleFor(r => r.Values).Must(v => v?.All(x => x.Id > 0) == true).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.TipoSintoma);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class RegisterSymptomInquiryResultCommandHandler : BasePassportChangeHandler<RegisterSymptomInquiryResultRequest, bool>
        {
            private readonly IRepository<Empleado> repositoryEmpleado;
            private readonly IRepository<ResultadoEncuestaSintomas> repositoryEncuesta;
            private readonly IRepository<TipoSintomas> repositorySintomas;
            private readonly IRepository<EstadoPasaporte> repositoryEstados;
            private readonly ICreatePassportService createPassportService;

            private int idEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public RegisterSymptomInquiryResultCommandHandler(IRepository<Empleado> repositoryEmpleado, IRepository<ResultadoEncuestaSintomas> repositoryEncuesta,
                IRepository<TipoSintomas> repositorySintomas, ICreatePassportService createPassportService, IRepository<EstadoPasaporte> repositoryEstados,
                IRepository<FichaMedica> repositoryFichaMedica) : base(repositoryEmpleado, repositoryFichaMedica)
            {
                this.repositoryEmpleado = repositoryEmpleado;
                this.repositoryEncuesta = repositoryEncuesta;
                this.repositorySintomas = repositorySintomas;
                this.createPassportService = createPassportService;
                this.repositoryEstados = repositoryEstados;

                createPassportService.CalculateNewStateEvent += CreatePassportService_CalculateNewStateEvent;
                createPassportService.AddNewPassportEvent += CreatePassportService_AddNewPassportEvent;
                createPassportService.AddOldPassportEvent += CreatePassportService_AddOldPassportEvent;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(RegisterSymptomInquiryResultRequest request, CancellationToken cancellationToken)
            {
                idEmpleado = request.IdEmployee;

                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                if (empleado == null || empleado.IdFichaMedica == null) // Si no hay ficha médica no está bien registrado.
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Empleado)
                    });
                }

                var symptoms = await repositorySintomas.GetAll().ToDictionaryAsync(e => e.Id).ConfigureAwait(false);

                if (request.Values.Any(rf => !symptoms.ContainsKey(rf.Id)))
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.TipoSintoma)
                    });
                }

                var estados = await repositoryEstados.GetAll()
                    .Include(c => c.IdTipoEstadoNavigation)
                    .Include(c => c.IdColorEstadoNavigation)
                    .ToDictionaryAsync(e => e.Nombre).ConfigureAwait(false);
                Guid grupoRespuestas = Guid.NewGuid();
                List<ResultadoEncuestaSintomas> resultados = request.Values.Select(s => new ResultadoEncuestaSintomas()
                {
                    IdFichaMedica = empleado.IdFichaMedica.Value,
                    IdTipoSintomaNavigation = symptoms[s.Id],
                    Valor = s.Value,
                    GrupoRespuestas = grupoRespuestas 
                }).ToList();
                this.repositoryEncuesta.AddRange(resultados);

                await repositoryEncuesta.SaveChangesAsync().ConfigureAwait(false);

                //-------------------------------------------------------------------------------------------

                Pasaporte currentPassport = await GetLastPassportAsync(empleado.Id).ConfigureAwait(false);
                List<ResultadoTestPcr> pcrList = await GetAllResultadoPcrAsync(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                ValoracionParametroMedico lastAnalitIgG = await GetLastAnalisticIgG(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                ValoracionParametroMedico lastAnalitIgM = await GetLastAnalisticIgM(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                ResultadoTestMedico lastTestRapido = await GetLastTestRapidoAsync(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                //ValoracionParametroMedico lastFiebre = await GetLastFiebreAsync(empleado.IdFichaMedica.Value).ConfigureAwait(false);

                ResultadoEncuestaSintomas fiebre = resultados.FirstOrDefault(c => c.IdTipoSintoma == symptoms.FirstOrDefault(d => d.Value.Nombre == "Fiebre").Value.Id);
                ResultadoEncuestaSintomas otros = resultados.FirstOrDefault(c => c.IdTipoSintoma == symptoms.FirstOrDefault(d => d.Value.Nombre == "OtrosSintomas").Value.Id);
                ResultadoEncuestaSintomas contacto = resultados.FirstOrDefault(c => c.IdTipoSintoma == symptoms.FirstOrDefault(d => d.Value.Nombre == "Contacto").Value.Id);

                createPassportService.CreateWithStatedCalculated(empleado, request.CurrentDeviceDateTime, estados,
                    currentPassport?.IdEstadoPasaporteNavigation, pcrList, lastAnalitIgG, lastAnalitIgM, lastTestRapido, null, fiebre,
                    otros, contacto);

                //-------------------------------------------------------------------------------------------

                await repositoryEmpleado.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }

            /// <summary>
            /// Metodo que valida si existe el empleado y su ficha
            /// </summary>
            /// <param name="idEmpleado"></param>
            /// <returns></returns>
            private async Task<Empleado> ValidacionEmpleado(int idEmpleado)
            {
                Empleado empleado = await repositoryEmpleado
                    .GetBy(p => p.Id == idEmpleado)
                        .Include(p => p.IdFichaMedicaNavigation)
                        .Include(e => e.Pasaporte)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                SendTimeOperationToLogger("ValidacionEmpleado");

                if (empleado == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Empleado)
                    });
                }

                if (empleado.IdFichaMedica == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.FichaMedica)
                    });
                }

                return empleado;
            }

            private void CreatePassportService_CalculateNewStateEvent(object sender, EventArgs e)
            {
                CalculateNewStateEventArgs arg = (CalculateNewStateEventArgs)e;

                Logger.LogInformation($"STATE PASSPORT -> CALCULATED -> IdEmpleado [{idEmpleado}] PCRReconvertido [{arg.Matrix.PCRReconvertido}] PCRUltimo[{arg.Matrix.PCRUltimo}] TestInmuneIgG [{arg.Matrix.TestInmuneIgG}] TestInmuneIgM [{arg.Matrix.TestInmuneIgM}] Estado [{arg.Matrix.NewState?.Nombre}]");
            }

            private void CreatePassportService_AddOldPassportEvent(object sender, EventArgs e)
            {
                Logger.LogInformation($"CREATE NEW STATE PASSPORT -> OLD STATE");
            }

            private void CreatePassportService_AddNewPassportEvent(object sender, EventArgs e)
            {
                Logger.LogInformation($"CREATE NEW STATE PASSPORT -> NEW STATE");
            }
        }
    }
}
