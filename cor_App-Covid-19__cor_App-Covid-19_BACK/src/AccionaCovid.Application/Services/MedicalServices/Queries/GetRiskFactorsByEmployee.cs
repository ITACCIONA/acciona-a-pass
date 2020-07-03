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

namespace AccionaCovid.Application.Services.MedicalServices
{
    /// <summary>
    /// Servicio que obtiene la informacion de los factores de riesgo de un empleado
    /// </summary>
    public class GetRiskFactorsByEmployee
    {
        /// <summary>
        /// </summary>
        public class GetRiskFactorsByEmployeeRequest : IRequest<GetRiskFactorsByEmployeeResponse>
        {
            public GetRiskFactorsByEmployeeRequest(int idEmpleado)
            {
                IdEmpleado = idEmpleado;
            }

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public int IdEmpleado { get; set; }
        }

        /// <summary>
        /// Validador del request
        /// </summary>
        public class GetRiskFactorsByEmployeeRequestValidator : AbstractValidator<GetRiskFactorsByEmployeeRequest>
        {
            /// <summary>
            /// Validaciones a realizar
            /// </summary>
            public GetRiskFactorsByEmployeeRequestValidator()
            {
                RuleFor(v => v.IdEmpleado).Must(p => p > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetRiskFactorsByEmployeeResponse
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="testRapidos"></param>
            /// <param name="testPcr"></param>
            public GetRiskFactorsByEmployeeResponse(List<ValoracionFactorRiesgo> valoraciones)
            {
                ValoracionFactorRiesgos = new List<ValoracionFactor>();

                if (valoraciones != null && valoraciones.Any())
                {
                    var tipos = valoraciones.Select(c => c.IdFactorRiesgoNavigation).Distinct();

                    foreach (var tipo in tipos)
                    {
                        var valor = tipo.ValoracionFactorRiesgo.OrderByDescending(c => c.FechaFactor).FirstOrDefault();

                        ValoracionFactor newFactor = new ValoracionFactor()
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
            /// Listado de test rapidos
            /// </summary>
            public List<ValoracionFactor> ValoracionFactorRiesgos { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class ValoracionFactor
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
        public class GetRiskFactorsByEmployeeCommandHandler : BaseCommandHandler<GetRiskFactorsByEmployeeRequest, GetRiskFactorsByEmployeeResponse>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<ValoracionFactorRiesgo> repositoryValoracionFactorRiesgo;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetRiskFactorsByEmployeeCommandHandler(IRepository<ValoracionFactorRiesgo> repositoryValoracionFactorRiesgo, IRepository<Empleado> repositoryEmpleado)
            {
                this.repositoryValoracionFactorRiesgo = repositoryValoracionFactorRiesgo ?? throw new ArgumentNullException(nameof(repositoryValoracionFactorRiesgo));
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<GetRiskFactorsByEmployeeResponse> Handle(GetRiskFactorsByEmployeeRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmpleado).ConfigureAwait(false);

                var listaTestRap = await repositoryValoracionFactorRiesgo.GetAll().Include(c => c.IdFactorRiesgoNavigation)
                                        .Where(c => c.IdFichaMedica == empleado.IdFichaMedica.Value).ToListAsync().ConfigureAwait(false);

                return new GetRiskFactorsByEmployeeResponse(listaTestRap);
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
