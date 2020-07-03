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
    public class RegisterGenerationManual
    {
        public class RegisterGenerationManualRequest : IRequest<bool>
        {
            public int IdEmployee { get; set; }
            public bool IsGreenPaper { get; set; }
            public DateTimeOffset? RegistrationDateTime { get; set; }
        }

        public class RegisterGenerationManualValidator : AbstractValidator<RegisterGenerationManualRequest>
        {
            public RegisterGenerationManualValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class RegisterGenerationManualCommandHandler : BasePassportChangeHandler<RegisterGenerationManualRequest, bool>
        {
            private readonly IRepository<Empleado> repositoryEmpleado;
            private readonly IRepository<TipoSintomas> repositoryTipoSintoma;
            private readonly ICreatePassportService createPassportService;
            private readonly IRepository<EstadoPasaporte> repositoryEstados;

            private int idEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public RegisterGenerationManualCommandHandler(IRepository<Empleado> repositoryEmpleado, IRepository<TipoSintomas> repositoryTipoSintoma,
                IRepository<ParametroMedico> repositoryParametroMedico, ICreatePassportService createPassportService, IRepository<EstadoPasaporte> repositoryEstados,
                IRepository<FichaMedica> repositoryFichaMedica) : base(repositoryEmpleado, repositoryFichaMedica)
            {
                this.repositoryEmpleado = repositoryEmpleado;
                this.repositoryTipoSintoma = repositoryTipoSintoma;
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
            public async override Task<bool> Handle(RegisterGenerationManualRequest request, CancellationToken cancellationToken)
            {
                idEmpleado = request.IdEmployee;
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                var oldState = empleado.Pasaporte
                    .SingleOrDefault(p => p.Activo == true)?.IdEstadoPasaporteNavigation;

                var newState = await repositoryEstados
                    .GetAll()
                        .Include(e => e.IdTipoEstadoNavigation)
                        .Include(e => e.IdColorEstadoNavigation)
                    .FirstOrDefaultAsync(s => s.EstadoId == (request.IsGreenPaper ? (int)EstadoPasaporte.PapertStatesId.NoSintomaticoPaper : (int)EstadoPasaporte.PapertStatesId.SintomaticoPaper)).ConfigureAwait(false);

                // Inicio de transacción
                //repositoryEstados.UnitOfWork.BeginTransaction();

                createPassportService.CreateFromChoosenState(empleado, request.RegistrationDateTime.HasValue ? request.RegistrationDateTime.Value : DateTimeOffset.Now, newState, true);

                //Si el pasaporte papel es verde calculamos el pasaporte
                if (request.IsGreenPaper)
                {
                    Guid guid = Guid.NewGuid();
                    List<ResultadoEncuestaSintomas> newResultadoEncuestaSintomas = (await repositoryTipoSintoma
                        .GetAll()
                        .ToListAsync().ConfigureAwait(false))
                        .Select(ts => new ResultadoEncuestaSintomas
                        {
                            IdTipoSintomaNavigation = ts,
                            Valor = false,
                            GrupoRespuestas = guid,
                        }).ToList();

                    foreach (ResultadoEncuestaSintomas res in newResultadoEncuestaSintomas)
                    {
                        empleado.IdFichaMedicaNavigation.ResultadoEncuestaSintomas.Add(res);
                    }

                    //repositoryEmpleado.Update(empleado);
                    //await repositoryEmpleado.SaveChangesAsync().ConfigureAwait(false);

                    var estados = await repositoryEstados.GetAll()
                    .Include(c => c.IdTipoEstadoNavigation)
                    .Include(c => c.IdColorEstadoNavigation)
                    .ToDictionaryAsync(e => e.Nombre);

                    Pasaporte currentPassport = await GetLastPassportAsync(empleado.Id).ConfigureAwait(false);
                    List<ResultadoTestPcr> pcrList = await GetAllResultadoPcrAsync(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                    ValoracionParametroMedico lastAnalitIgG = await GetLastAnalisticIgG(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                    ValoracionParametroMedico lastAnalitIgM = await GetLastAnalisticIgM(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                    ResultadoTestMedico lastTestRapido = await GetLastTestRapidoAsync(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                    ValoracionParametroMedico lastFiebre = await GetLastFiebreAsync(empleado.IdFichaMedica.Value).ConfigureAwait(false);
                    var ultimosResul = GetLastResultadoEncuesta(newResultadoEncuestaSintomas);


                    createPassportService.CreateWithStatedCalculated(empleado, request.RegistrationDateTime ?? DateTimeOffset.UtcNow, estados,
                        currentPassport?.IdEstadoPasaporteNavigation, pcrList, lastAnalitIgG, lastAnalitIgM, lastTestRapido, lastFiebre, ultimosResul.Fiebre,
                        ultimosResul.Otros, ultimosResul.Contacto);
                }

                repositoryEmpleado.Update(empleado);
                await repositoryEmpleado.SaveChangesAsync().ConfigureAwait(false);

                // commit de la transacción
                //repositoryEmpleado.UnitOfWork.Commit();

                return true;
            }

            private void CreatePassport(RegisterGenerationManualRequest request, Empleado empleado, EstadoPasaporte oldState, EstadoPasaporte newState)
            {
                if ((oldState?.IdColorEstadoNavigation?.Prioridad ?? 9999) < newState.IdColorEstadoNavigation.Prioridad)
                {
                    // Mejora el color ---> viejo estado
                    createPassportService.CreateFromChoosenState(empleado, request.RegistrationDateTime.HasValue ? request.RegistrationDateTime.Value : DateTimeOffset.Now, oldState, true);
                    return;
                }

                if ((oldState?.IdTipoEstadoNavigation?.Prioridad ?? 9999) < newState.IdTipoEstadoNavigation.Prioridad)
                {
                    // Mejora el tipo de estado ---> viejo estado
                    createPassportService.CreateFromChoosenState(empleado, request.RegistrationDateTime.HasValue ? request.RegistrationDateTime.Value : DateTimeOffset.Now, oldState, true);
                    return;
                }

                createPassportService.CreateFromChoosenState(empleado, request.RegistrationDateTime.HasValue ? request.RegistrationDateTime.Value : DateTimeOffset.Now, newState, true);
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
                            .ThenInclude(c => c.IdEstadoPasaporteNavigation)
                                .ThenInclude(c => c.IdTipoEstadoNavigation)
                        .Include(c => c.Pasaporte)
                            .ThenInclude(c => c.IdEstadoPasaporteNavigation)
                                .ThenInclude(c => c.IdColorEstadoNavigation)
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
