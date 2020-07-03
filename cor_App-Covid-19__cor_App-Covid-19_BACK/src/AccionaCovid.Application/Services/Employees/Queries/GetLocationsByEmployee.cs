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
    /// </summary>
    public class GetLocationsByEmployee
    {
        /// <summary>
        /// </summary>
        public class GetLocationsByEmployeeRequest : IRequest<GetLocationsByEmployeeResponse>
        {
            public int IdEmployee { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class GetLocationsByEmployeeRequestValidator : AbstractValidator<GetLocationsByEmployeeRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetLocationsByEmployeeRequestValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetLocationsByEmployeeResponse
        {
            public List<EmployeeLocation> Localizations { get; set; }
        }

        public class EmployeeLocation
        {
            public int IdLocalization { get; set; }
            public DateTimeOffset Date { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public EmployeeLocation(LocalizacionEmpleados localizacion)
            {
                this.IdLocalization = localizacion.Id;
                this.Date = localizacion.Fecha;
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetLocationsByEmployeeCommandHandler : IRequestHandler<GetLocationsByEmployeeRequest, GetLocationsByEmployeeResponse>
        {
            private readonly IRepository<Empleado> repositoryEmpleados;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetLocationsByEmployeeCommandHandler(IRepository<Empleado> repositoryEmpleados)
            {
                this.repositoryEmpleados = repositoryEmpleados;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<GetLocationsByEmployeeResponse> Handle(GetLocationsByEmployeeRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                return new GetLocationsByEmployeeResponse { Localizations = empleado.LocalizacionEmpleados.Select(l => new EmployeeLocation(l)).ToList() };
            }

            /// <summary>
            /// Metodo que valida si existe el empleado y su ficha
            /// </summary>
            /// <param name="idEmpleado"></param>
            /// <returns></returns>
            private async Task<Empleado> ValidacionEmpleado(int idEmpleado)
            {
                Empleado empleado = await repositoryEmpleados
                    .GetBy(p => p.Id == idEmpleado)
                        .Include(e => e.LocalizacionEmpleados)
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

                return empleado;
            }
        }
    }
}
