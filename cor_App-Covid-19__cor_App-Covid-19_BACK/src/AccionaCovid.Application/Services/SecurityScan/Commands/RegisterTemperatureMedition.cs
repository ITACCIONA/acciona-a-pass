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
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.SecurityScan
{
    public class RegisterTemperatureMeditionSecurity
    {
        public class RegisterTemperatureMeditionSecurityRequest : IRequest<bool>
        {
            public int IdEmployee { get; set; }
            public string IdDevice { get; set; }
            public bool IsTemperatureOverThreshold { get; set; }
            public DateTimeOffset? MeditionDateTime { get; set; }
        }

        public class RegisterTemperatureMeditionSecurityValidator : AbstractValidator<RegisterTemperatureMeditionSecurityRequest>
        {
            public RegisterTemperatureMeditionSecurityValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
                RuleFor(r => r.IdDevice).Must(id => id != null).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_NULL_OR_EMPTY, ValidatorFields.IdDispositivo);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class RegisterTemperatureMeditionSecurityCommandHandler : BasePassportChangeHandler<RegisterTemperatureMeditionSecurityRequest, bool>
        {
            private readonly IRepository<SeguimientoMedico> repositorySeguimientos;
            private readonly IRepository<Empleado> repositoryEmpleado;
            private readonly IRepository<ParametroMedico> repositoryParametroMedico;
            private readonly ICreatePassportService createPassportService;
            private readonly IRepository<EstadoPasaporte> repositoryEstados;

            private int idEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public RegisterTemperatureMeditionSecurityCommandHandler(IRepository<SeguimientoMedico> repositorySeguimientos, IRepository<Empleado> repositoryEmpleado,
                IRepository<ParametroMedico> repositoryParametroMedico, ICreatePassportService createPassportService, IRepository<EstadoPasaporte> repositoryEstados,
                IRepository<FichaMedica> repositoryFichaMedica) : base(repositoryEmpleado,repositoryFichaMedica)
            {
                this.repositorySeguimientos = repositorySeguimientos;
                this.repositoryEmpleado = repositoryEmpleado;
                this.repositoryParametroMedico = repositoryParametroMedico;
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
            public async override Task<bool> Handle(RegisterTemperatureMeditionSecurityRequest request, CancellationToken cancellationToken)
            {
                idEmpleado = request.IdEmployee;
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                ParametroMedico paramTempe = await repositoryParametroMedico.GetAll().FirstOrDefaultAsync(c => c.Nombre == ParametroMedico.ParameterTypes.TemperaturaAlta.ToString()).ConfigureAwait(false);

                SeguimientoMedico seguimiento = new SeguimientoMedico()
                {
                    IdFichaMedica = empleado.IdFichaMedica.Value,
                    Comentarios = "Security Scan Temperature Medition",
                    Activo = true,
                    FechaSeguimiento = request.MeditionDateTime.HasValue ? request.MeditionDateTime.Value : DateTimeOffset.Now
                };

                seguimiento.ValoracionParametroMedico = new List<ValoracionParametroMedico>()
                {
                    new ValoracionParametroMedico()
                    {
                        Valor = request.IsTemperatureOverThreshold,
                        IdParametroMedicoNavigation = paramTempe,
                    }
                };

                List<SeguimientoMedico> oldSeguimentos = await repositorySeguimientos.GetBy(c => c.IdFichaMedica == empleado.IdFichaMedica.Value
                        && c.ValoracionParametroMedico.Select(d => d.IdParametroMedico).Any(e => e == paramTempe.Id))
                    .ToListAsync().ConfigureAwait(false);

                foreach (var item in oldSeguimentos)
                {
                    item.Activo = false;
                }

                repositorySeguimientos.UpdateRange(oldSeguimentos);


                repositorySeguimientos.Add(seguimiento);
                await this.repositorySeguimientos.SaveChangesAsync().ConfigureAwait(false);

                // creamos un nuevo pasaporte en base a la nueva info proporcionada
                var estados = await repositoryEstados.GetAll()
                    .Include(c => c.IdTipoEstadoNavigation)
                    .Include(c => c.IdColorEstadoNavigation)
                    .ToDictionaryAsync(e => e.Nombre);

                //-------------------------------------------------------------------------------------------

                Pasaporte currentPassport = await GetLastPassportAsync(empleado.Id).ConfigureAwait(false);
                List<ResultadoTestPcr> pcrList = await GetAllResultadoPcrAsync(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                ValoracionParametroMedico lastAnalitIgG = await GetLastAnalisticIgG(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                ValoracionParametroMedico lastAnalitIgM = await GetLastAnalisticIgM(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                ResultadoTestMedico lastTestRapido = await GetLastTestRapidoAsync(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                ValoracionParametroMedico lastFiebre = seguimiento.ValoracionParametroMedico.FirstOrDefault();
                var ultimosResul = await GetLastResultadoEncuestaAsync(empleado.IdFichaMedica.Value).ConfigureAwait(false);

                createPassportService.CreateWithStatedCalculated(empleado, request.MeditionDateTime ?? DateTimeOffset.UtcNow, estados,
                    currentPassport?.IdEstadoPasaporteNavigation, pcrList, lastAnalitIgG, lastAnalitIgM, lastTestRapido, lastFiebre, ultimosResul.Fiebre,
                    ultimosResul.Otros, ultimosResul.Contacto);

                //-------------------------------------------------------------------------------------------

                repositoryEmpleado.Update(empleado);
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
                        .Include(c => c.Pasaporte)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

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
