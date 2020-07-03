using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.SecurityScan
{
    /// <summary>
    /// Query para obtener el pasaporte de un empleado.
    /// </summary>
    public class GetActivePassport
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetActivePassportRequest : IRequest<GetActivePassportResponse>
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmployee { get; set; }
        }

        /// <summary>
        /// Validaor
        /// </summary>
        public class GetActivePassportValidator : AbstractValidator<GetActivePassport.GetActivePassportRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetActivePassportValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetActivePassportResponse
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmpleado { get; set; }

            /// <summary>
            /// Identificador del pasaporte
            /// </summary>
            public int IdPassport { get; set; }

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public string NombreEmpleado { get; set; }

            /// <summary>
            /// Edad del empleado
            /// </summary>
            public int EdadEmpleado { get; set; }

            /// <summary>
            /// Numero del empleado
            /// </summary>
            public long NumEmpleado { get; set; }

            /// <summary>
            /// Departamento del empleado
            /// </summary>
            public string Departamento { get; set; }

            /// <summary>
            /// Localizacion del empleado
            /// </summary>
            public string Localizacion { get; set; }

            /// <summary>
            /// Division del empleado
            /// </summary>
            public string Division { get; set; }

            /// <summary>
            /// Estado del pasaporte
            /// </summary>
            public string EstadoPasaporte { get; set; }

            /// <summary>
            /// Color del pasaporte
            /// </summary>
            public string ColorPasaporte { get; set; }

            /// <summary>
            /// Indica si hay que sacar mensaje
            /// </summary>
            public bool HasMessage { get; set; }

            /// <summary>
            /// Fecha creacion del pasaporte
            /// </summary>
            public DateTimeOffset FechaCreacion { get; set; }

            /// <summary>
            /// Fecha de expiracion
            /// </summary>
            public DateTimeOffset? FechaExpiracion { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetActivePassportCommandHandler : BaseCommandHandler<GetActivePassportRequest, GetActivePassportResponse>
        {
            /// <summary>
            /// Repositorio
            /// </summary>
            private readonly IRepository<Pasaporte> repositoryPasaporte;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetActivePassportCommandHandler(IRepository<Pasaporte> repositoryPasaporte, IRepository<Empleado> repositoryEmpleado)
            {
                this.repositoryPasaporte = repositoryPasaporte ?? throw new ArgumentNullException(nameof(repositoryPasaporte));
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async override Task<GetActivePassportResponse> Handle(GetActivePassportRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                var passport = await repositoryPasaporte.GetAll()
                    .Include(c => c.IdEmpleadoNavigation)
                        .ThenInclude(c => c.IdFichaLaboralNavigation)
                            .ThenInclude(c => c.IdDepartamentoNavigation)
                    .Include(c => c.IdEmpleadoNavigation)
                        .ThenInclude(c => c.IdFichaLaboralNavigation)
                            .ThenInclude(c => c.IdDivisionNavigation)
                    .Include(c => c.IdEmpleadoNavigation)
                        .ThenInclude(c => c.IdFichaLaboralNavigation)
                            .ThenInclude(c => c.IdLocalizacionNavigation)
                    .Include(c => c.IdEstadoPasaporteNavigation)
                        .ThenInclude(c => c.IdColorEstadoNavigation)
                    .Include(c => c.IdEstadoPasaporteNavigation)
                        .ThenInclude(c => c.EstadoPasaporteIdioma)
                    .FirstOrDefaultAsync(c => c.IdEmpleado == request.IdEmployee && c.Activo.Value).ConfigureAwait(false);

                if (passport == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Passport)
                    });
                }

                GetActivePassportResponse response = new GetActivePassportResponse()
                {
                    IdPassport = passport.Id,
                    IdEmpleado = passport.IdEmpleado,
                    NombreEmpleado = passport.IdEmpleadoNavigation.NombreCompleto,
                    EdadEmpleado = passport.IdEmpleadoNavigation.CalcularEdad(),
                    NumEmpleado = passport.IdEmpleadoNavigation.NumEmpleado ?? passport.IdEmpleado,
                    Departamento = passport.IdEmpleadoNavigation.IdFichaLaboralNavigation?.IdDepartamentoNavigation?.Nombre,
                    Division = passport.IdEmpleadoNavigation.IdFichaLaboralNavigation?.IdDivisionNavigation?.Nombre,
                    Localizacion = passport.IdEmpleadoNavigation.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Nombre,
                    FechaCreacion = passport.FechaCreacion,
                    FechaExpiracion = passport.FechaExpiracion,
                    ColorPasaporte = passport.IdEstadoPasaporteNavigation?.IdColorEstadoNavigation?.Nombre,
                    EstadoPasaporte = passport.IdEstadoPasaporteNavigation?.EstadoPasaporteIdioma.FirstOrDefault(c => c.Idioma == Idioma)?.Nombre ?? passport.IdEstadoPasaporteNavigation?.Nombre,
                    HasMessage = passport.IdEstadoPasaporteNavigation.Comment.GetValueOrDefault()
                };

                return response;
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
        }
    }
}
