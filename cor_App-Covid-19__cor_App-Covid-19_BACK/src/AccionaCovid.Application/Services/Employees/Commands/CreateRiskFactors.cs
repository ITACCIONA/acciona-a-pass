using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using AccionaCovid.Domain.Services;
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
    /// Servicio que registra un nuvo listado de factores de riesgo
    /// </summary>
    public class CreateRiskFactors
    {
        /// <summary>
        /// Request
        /// </summary>
        public class CreateRiskFactorsRequest : IRequest<bool>
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmployee { get; set; }

            /// <summary>
            /// Fecha de inserccion
            /// </summary>
            public DateTimeOffset FechaFactor { get; set; }

            /// <summary>
            /// Lista de factores de riesgos a insertar
            /// </summary>
            public List<RiskFactorValue> RiskFactorValues { get; set; }
        }

        /// <summary>
        /// Request
        /// </summary>
        public class RiskFactorValue
        {
            /// <summary>
            /// Identificador del factor
            /// </summary>
            public int IdRiskFactor { get; set; }

            /// <summary>
            /// Valor del factor
            /// </summary>
            public bool? Value { get; set; }
        }

        /// <summary>
        /// Clase de validacion
        /// </summary>
        public class CreateRiskFactorsRequestValidator : AbstractValidator<CreateRiskFactorsRequest>
        {
            public CreateRiskFactorsRequestValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
                RuleFor(v => v.RiskFactorValues).NotNull().WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_NULL_OR_EMPTY, ValidatorFields.FactoresRiesgo);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class CreateRiskFactorsCommandHandler : BaseCommandHandler<CreateRiskFactorsRequest, bool>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<ValoracionFactorRiesgo> repository;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;
            private readonly ICreatePassportService createPassportService;
            private readonly IRepository<EstadoPasaporte> repositoryEstados;
            private readonly IRepository<FactorRiesgo> repositoryFactores;

            /// <summary>
            /// Constructor
            /// </summary>
            public CreateRiskFactorsCommandHandler(IRepository<ValoracionFactorRiesgo> repository, IRepository<Empleado> repositoryEmpleado, ICreatePassportService createPassportService, IRepository<EstadoPasaporte> repositoryEstados, IRepository<FactorRiesgo> repositoryFactores)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
                this.createPassportService = createPassportService;
                this.repositoryEstados = repositoryEstados;
                this.repositoryFactores = repositoryFactores;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(CreateRiskFactorsRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                DateTime fechaActual = DateTime.UtcNow;

                var riskFactors = await repositoryFactores.GetAll().ToDictionaryAsync(e => e.Id);

                if (request.RiskFactorValues.Any(rf => !riskFactors.ContainsKey(rf.IdRiskFactor)))
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.FactoresRiesgo)
                    });
                }

                var valoraciones = request.RiskFactorValues.Select(rf => new ValoracionFactorRiesgo()
                {
                    Valor = rf.Value,
                    IdFactorRiesgoNavigation = riskFactors[rf.IdRiskFactor],
                    IdFichaMedica = empleado.IdFichaMedica.Value,
                    FechaFactor = request.FechaFactor
                }).ToList();

                repository.AddRange(valoraciones);

                var estados = await repositoryEstados.GetAll().ToListAsync().ConfigureAwait(false);
                createPassportService.CreateInitState(empleado, request.FechaFactor, estados);

                await repository.SaveChangesAsync().ConfigureAwait(false);
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
