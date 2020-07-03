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

namespace AccionaCovid.Application.Services.Employees
{
    /// <summary>
    /// Query para obtener el pasaporte de un empleado.
    /// </summary>
    public class GetPassportByIdEmployee
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetPassportByIdEmployeeRequest : IRequest<GetPassportByIdEmployeeResponse>
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmployee { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class GetPassportByIdEmployeeValidator : AbstractValidator<GetPassportByIdEmployeeRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetPassportByIdEmployeeValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetPassportByIdEmployeeResponse
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
            /// Iniciales del empleado
            /// </summary>
            public string InicialesEmpleado { get; set; }

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
            /// Nombre Localizacion del empleado
            /// </summary>
            public string NameLocalizacion { get; set; }

            /// <summary>
            /// Pais de la localizacion del empleado
            /// </summary>
            public string Pais { get; set; }

            /// <summary>
            /// Direccion de la localizacion del empleado
            /// </summary>
            public string Direccion1 { get; set; }

            /// <summary>
            /// Ciudad de la localizacion del empleado
            /// </summary>
            public string Ciudad { get; set; }

            /// <summary>
            /// Codigo POstal de la localizacion del empleado
            /// </summary>
            public string CodigoPostal { get; set; }

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
            /// Identificador interno del Estado para los mensajes
            /// </summary>
            public int EstadoId { get; set; }

            /// <summary>
            /// Indica el numero de test realizados al empleado
            /// </summary>
            public int NumTest { get; set; }

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
        public class GetPassportByIdEmployeeCommandHandler : BaseCommandHandler<GetPassportByIdEmployeeRequest, GetPassportByIdEmployeeResponse>
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
            /// Repositorio
            /// </summary>
            private readonly IRepository<FichaMedica> repositoryFichaMedica;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetPassportByIdEmployeeCommandHandler(IRepository<Pasaporte> repositoryPasaporte, IRepository<Empleado> repositoryEmpleado, IRepository<FichaMedica> repositoryFichaMedica)
            {
                this.repositoryPasaporte = repositoryPasaporte ?? throw new ArgumentNullException(nameof(repositoryPasaporte));
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
                this.repositoryFichaMedica = repositoryFichaMedica ?? throw new ArgumentNullException(nameof(repositoryFichaMedica));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async override Task<GetPassportByIdEmployeeResponse> Handle(GetPassportByIdEmployeeRequest request, CancellationToken cancellationToken)
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

                FichaMedica fchMedica = await repositoryFichaMedica.GetAll()
                    .Include(c => c.ResultadoTestMedico)
                    .Include(c => c.ResultadoTestPcr)
                    .FirstOrDefaultAsync(c => c.Id == empleado.IdFichaMedica).ConfigureAwait(false);

                GetPassportByIdEmployeeResponse response = new GetPassportByIdEmployeeResponse()
                {
                    IdPassport = passport.Id,
                    IdEmpleado = passport.IdEmpleado,
                    NombreEmpleado = passport.IdEmpleadoNavigation.NombreCompleto,
                    InicialesEmpleado = passport.IdEmpleadoNavigation.Iniciales,
                    EdadEmpleado = passport.IdEmpleadoNavigation.CalcularEdad(),
                    NumEmpleado = empleado.NumEmpleado ?? empleado.Id,
                    Departamento = passport.IdEmpleadoNavigation.IdFichaLaboralNavigation?.IdDepartamentoNavigation?.Nombre,
                    Division = passport.IdEmpleadoNavigation.IdFichaLaboralNavigation?.IdDivisionNavigation?.Nombre,
                    NameLocalizacion = passport.IdEmpleadoNavigation.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Nombre,
                    Pais = passport.IdEmpleadoNavigation.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Pais,
                    Ciudad = passport.IdEmpleadoNavigation.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Ciudad,
                    CodigoPostal = passport.IdEmpleadoNavigation.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.CodigoPostal,
                    Direccion1 = passport.IdEmpleadoNavigation.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Direccion1,
                    FechaCreacion = passport.FechaCreacion,
                    FechaExpiracion = passport.FechaExpiracion,
                    ColorPasaporte = passport.IdEstadoPasaporteNavigation?.IdColorEstadoNavigation?.Nombre,
                    EstadoPasaporte = passport.IdEstadoPasaporteNavigation?.EstadoPasaporteIdioma.FirstOrDefault(c => c.Idioma == Idioma)?.Nombre ?? passport.IdEstadoPasaporteNavigation?.Nombre,
                    NumTest = CalculoNumeroTest(fchMedica),
                    HasMessage = passport.IdEstadoPasaporteNavigation.Comment.GetValueOrDefault(),
                    EstadoId = passport.IdEstadoPasaporteNavigation.EstadoId
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

            /// <summary>
            /// Metodo que calcula el numero de test que tiene un empelado
            /// </summary>
            /// <param name="fchMedica"></param>
            /// <returns></returns>
            private int CalculoNumeroTest(FichaMedica fchMedica)
            {
                int numTest = 0;

                if(fchMedica != null)
                {
                    if(fchMedica.ResultadoTestMedico != null && fchMedica.ResultadoTestMedico.Any())
                    {
                        numTest += fchMedica.ResultadoTestMedico.Count;
                    }

                    if (fchMedica.ResultadoTestPcr != null && fchMedica.ResultadoTestPcr.Any())
                    {
                        numTest += fchMedica.ResultadoTestPcr.Count;
                    }
                }

                return numTest;
            }
        }
    }
}
