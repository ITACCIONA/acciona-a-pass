using AccionaCovid.Application.Core;
using AccionaCovid.Application.Resources;
using AccionaCovid.Crosscutting;
using AccionaCovid.Domain.Core;
using AccionaCovid.Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Employees
{
    /// <summary>
    /// </summary>
    public class UpdateEmployeeProfile
    {
        /// <summary>
        /// </summary>
        public class UpdateEmployeeProfileRequest : IRequest<bool>
        {
            /// <summary>
            /// Identificador del empleado
            /// </summary>
            public int IdEmployee { get; set; }

            /// <summary>
            /// Telefono del empleado
            /// </summary>
            public string TelefonoEmpleado { get; set; }

            /// <summary>
            /// IdLocalizacion del empleado
            /// </summary>
            public int? IdLocalizacion { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class UpdateEmployeeProfileValidator : AbstractValidator<UpdateEmployeeProfileRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public UpdateEmployeeProfileValidator()
            {
                RuleFor(r => r.IdEmployee).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
                RuleFor(r => r.TelefonoEmpleado).IsValidLatinString().WithFormatMessage(ValidatorsMessages.VALUE_WRONG_FORMAT, ValidatorFields.Telefono);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class UpdateEmployeeProfileCommandHandler : BaseCommandHandler<UpdateEmployeeProfileRequest, bool>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public UpdateEmployeeProfileCommandHandler(IRepository<Empleado> repositoryEmpleado)
            {
                this.repositoryEmpleado = repositoryEmpleado;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(UpdateEmployeeProfileRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmployee).ConfigureAwait(false);

                empleado.IdFichaLaboralNavigation.TelefonoCorp = request.TelefonoEmpleado; 
                empleado.IdFichaLaboralNavigation.IdLocalizacion = request.IdLocalizacion;

                this.repositoryEmpleado.Update(empleado);
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
                        .Include(e => e.IdFichaLaboralNavigation)
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
