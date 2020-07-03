using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Employees
{
    /// <summary>
    /// Query para obtener el pasaporte de un empleado.
    /// </summary>
    public class GetEmployeeProfile
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetEmployeeProfileRequest : IRequest<GetEmployeeProfileResponse>
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmployee { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class GetEmployeeProfileValidator : AbstractValidator<GetEmployeeProfileRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetEmployeeProfileValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetEmployeeProfileResponse
        {
            public GetEmployeeProfileResponse(List<ValoracionFactorRiesgo> valoraciones)
            {
                ValoracionFactorRiesgos = new List<ValoracionFactorEmployeeProfile>();

                if (valoraciones != null && valoraciones.Any())
                {
                    var tipos = valoraciones.Select(c => c.IdFactorRiesgoNavigation).Distinct();

                    foreach (var tipo in tipos)
                    {
                        var valor = tipo.ValoracionFactorRiesgo.OrderByDescending(c => c.FechaFactor).FirstOrDefault();

                        ValoracionFactorEmployeeProfile newFactor = new ValoracionFactorEmployeeProfile()
                        {
                            FechaFactor = valor.FechaFactor,
                            IdRiskFactor = tipo.Id,
                            Name = tipo.Nombre,
                            Value = valor.Valor
                        };

                        ValoracionFactorRiesgos.Add(newFactor);
                    }
                }
            }

            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmpleado { get; set; }

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public string NombreEmpleado { get; set; }

            /// <summary>
            /// Apellidos del empleado
            /// </summary>
            public string ApellidosEmpleado { get; set; }

            /// <summary>
            /// Iniciales del empleado
            /// </summary>
            public string InicialesEmpleado { get; set; }

            /// <summary>
            /// DNI del empleado
            /// </summary>
            public string DNI { get; set; }

            /// <summary>
            /// Edad del empleado
            /// </summary>
            public int EdadEmpleado { get; set; }

            /// <summary>
            /// Numero del empleado
            /// </summary>
            public long NumEmpleado { get; set; }

            /// <summary>
            /// Telefono del empleado
            /// </summary>
            public string TelefonoEmpleado { get; set; }

            /// <summary>
            /// Mail del empleado
            /// </summary>
            public string MailEmpleado { get; set; }

            /// <summary>
            /// Listado de test rapidos
            /// </summary>
            public List<ValoracionFactorEmployeeProfile> ValoracionFactorRiesgos { get; set; }

            /// <summary>
            /// IdLocalizacion del empleado
            /// </summary>
            public int? IdLocalizacion { get; set; }

            /// <summary>
            /// Localización del empleado
            /// </summary>
            public string Localizacion { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class ValoracionFactorEmployeeProfile
        {
            /// <summary>
            /// Fecha del test
            /// </summary>
            public DateTimeOffset FechaFactor { get; set; }

            /// <summary>
            /// Identificador del factor de riesgo
            /// </summary>
            public int IdRiskFactor { get; set; }

            /// <summary>
            /// Nombre del factor de riesgo
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Valor del factor
            /// </summary>
            public bool? Value { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetEmployeeProfileCommandHandler : BaseCommandHandler<GetEmployeeProfileRequest, GetEmployeeProfileResponse>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<ValoracionFactorRiesgo> repositoryValoracionFactorRiesgo;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetEmployeeProfileCommandHandler(IRepository<ValoracionFactorRiesgo> repositoryValoracionFactorRiesgo, IRepository<Empleado> repositoryEmpleado)
            {
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
                this.repositoryValoracionFactorRiesgo = repositoryValoracionFactorRiesgo ?? throw new ArgumentNullException(nameof(repositoryValoracionFactorRiesgo));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async override Task<GetEmployeeProfileResponse> Handle(GetEmployeeProfileRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                var listaTestRap = await repositoryValoracionFactorRiesgo.GetAll().Include(c => c.IdFactorRiesgoNavigation)
                                        .Where(c => c.IdFichaMedica == empleado.IdFichaMedica.Value).ToListAsync().ConfigureAwait(false);

                GetEmployeeProfileResponse response = new GetEmployeeProfileResponse(listaTestRap)
                {
                    IdEmpleado = empleado.Id,
                    NombreEmpleado = empleado.Nombre,
                    InicialesEmpleado = empleado.Iniciales,
                    EdadEmpleado = empleado.CalcularEdad(),
                    NumEmpleado = empleado.NumEmpleado ?? empleado.Id,
                    ApellidosEmpleado = empleado.Apellido,
                    DNI = empleado.Nif,
                    MailEmpleado = empleado.IdFichaLaboralNavigation?.MailProf,
                    TelefonoEmpleado = empleado.IdFichaLaboralNavigation?.TelefonoCorp,
                    IdLocalizacion = empleado.IdFichaLaboralNavigation?.IdLocalizacion,
                    Localizacion = empleado.IdFichaLaboralNavigation?.IdLocalizacionNavigation?.Nombre
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
                        .Include(c => c.IdFichaLaboralNavigation)
                        .Include(c => c.IdFichaLaboralNavigation.IdLocalizacionNavigation)
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
