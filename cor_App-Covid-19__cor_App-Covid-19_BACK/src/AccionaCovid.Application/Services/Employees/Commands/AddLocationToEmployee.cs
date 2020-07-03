using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Employees
{
    /// <summary>
    /// </summary>
    public class AddLocationToEmployee
    {
        /// <summary>
        /// </summary>
        public class AddLocationToEmployeeRequest : IRequest<bool>
        {
            public int IdEmployee { get; set; }
            public int IdLocalization { get; set; }
            public DateTimeOffset Date { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class AddLocationToEmployeeRequestValidator : AbstractValidator<AddLocationToEmployeeRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public AddLocationToEmployeeRequestValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
                RuleFor(r => r.IdLocalization).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.Location);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class AddLocationToEmployeeCommandHandler : BaseCommandHandler<AddLocationToEmployeeRequest, bool>
        {
            private readonly IRepository<Empleado> repositoryEmpleados;
            private readonly IRepository<Localizacion> repositoryLocalizaciones;
            private readonly IRepository<LocalizacionEmpleados> repositoryLocalizacionesEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public AddLocationToEmployeeCommandHandler(IRepository<Empleado> repositoryEmpleados, IRepository<Localizacion> repositoryLocalizaciones, IRepository<LocalizacionEmpleados> repositoryLocalizacionesEmpleado)
            {
                this.repositoryEmpleados = repositoryEmpleados;
                this.repositoryLocalizaciones = repositoryLocalizaciones;
                this.repositoryLocalizacionesEmpleado = repositoryLocalizacionesEmpleado;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(AddLocationToEmployeeRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                bool existsLocalizacion = await repositoryLocalizaciones
                    .GetBy(l => l.Id == request.IdLocalization)
                    .AnyAsync()
                    .ConfigureAwait(false);

                if (!existsLocalizacion)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Location)
                    });
                }

                this.repositoryLocalizacionesEmpleado.Add(new LocalizacionEmpleados()
                {
                    IdEmpleado = request.IdEmployee,
                    IdLocalizacion = request.IdLocalization,
                    Fecha = request.Date
                });
                await this.repositoryLocalizacionesEmpleado.SaveChangesAsync().ConfigureAwait(false);

                return true;
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
