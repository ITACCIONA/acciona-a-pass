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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.MedicalServices
{
    public class CreateExternalEmployee
    {
        public class CreateExternalEmployeeRequest : IRequest<bool>
        {
            /// <summary>
            /// DNI del empleado
            /// </summary>
            public string DNI { get; set; }

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public string Nombre { get; set; }

            /// <summary>
            /// Apellidos del empleado
            /// </summary>
            public string Apellidos { get; set; }

            /// <summary>
            /// Identificador de la división
            /// </summary>
            public int? IdDivision { get; set; }

            /// <summary>
            /// Identificador de la localizacion
            /// </summary>
            public int IdLocalizacion { get; set; }

            /// <summary>
            /// Identificador del responsable
            /// </summary>
            public int IdResponsable { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class CreateExternalEmployeeValidator : AbstractValidator<CreateExternalEmployeeRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public CreateExternalEmployeeValidator()
            {
                RuleFor(r => r.DNI).NotNull().NotEmpty().WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_NULL_OR_EMPTY, ValidatorFields.DNI);
                RuleFor(r => r.Nombre).NotNull().NotEmpty().WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_NULL_OR_EMPTY, ValidatorFields.Nombre);
                RuleFor(r => r.IdDivision).Must(id => !id.HasValue || id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.Division);
                RuleFor(r => r.IdLocalizacion).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.Location);
                RuleFor(r => r.IdResponsable).Must(id => id > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdResponsable);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class CreateExternalEmployeeCommandHandler : BaseCommandHandler<CreateExternalEmployeeRequest, bool>
        {
            private readonly IRepository<Empleado> repository;
            private readonly IRepository<Division> divisionRepository;
            private readonly IRepository<Localizacion> localizacionRepository;

            /// <summary>
            /// Constructor
            /// </summary>
            public CreateExternalEmployeeCommandHandler(IRepository<Empleado> repository, IRepository<Division> divisionRepository, IRepository<Localizacion> localizacionRepository)
            {
                this.repository = repository;
                this.divisionRepository = divisionRepository;
                this.localizacionRepository = localizacionRepository;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<bool> Handle(CreateExternalEmployeeRequest request, CancellationToken cancellationToken)
            {
                await ValidateRequestAsync(request);

                DateTime now = DateTime.UtcNow;

                Empleado newEmpleado = new Empleado();
                newEmpleado.Nif = request.DNI;
                newEmpleado.Nombre = request.Nombre;
                newEmpleado.Apellido = request.Apellidos;
                newEmpleado.ManualCreator = this.IdUser;
                newEmpleado.UltimaModif = now;
                newEmpleado.LastAction = "CREATE";
                newEmpleado.LastActionDate = now;

                newEmpleado.IdFichaLaboralNavigation = new FichaLaboral();
                newEmpleado.IdFichaLaboralNavigation.IsExternal = true;
                if (request.IdDivision.HasValue) newEmpleado.IdFichaLaboralNavigation.IdDivision = request.IdDivision.Value;
                newEmpleado.IdFichaLaboralNavigation.IdLocalizacion = request.IdLocalizacion;
                newEmpleado.IdFichaLaboralNavigation.IdResponsableDirecto = request.IdResponsable;

                newEmpleado.IdFichaLaboralNavigation.LastAction = "CREATE";
                newEmpleado.IdFichaLaboralNavigation.LastActionDate = now;

                newEmpleado.IdFichaMedicaNavigation = new FichaMedica();
                newEmpleado.IdFichaMedicaNavigation.FechaAlta = now;
                newEmpleado.IdFichaMedicaNavigation.LastAction = "CREATE";

                EmpleadoRole er = new EmpleadoRole();
                er.IdEmpleadoNavigation = newEmpleado;
                er.IdRole = 3;
                newEmpleado.EmpleadoRole.Add(er);

                repository.Add(newEmpleado);
                await repository.SaveChangesAsync();

                return true;
            }

            private async Task ValidateRequestAsync(CreateExternalEmployeeRequest request)
            {
                if (await repository.GetAll().AnyAsync(e=> e.Nif == request.DNI).ConfigureAwait(false))
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "EMPLOYEE_CONFLICT",
                        Message = string.Format(ValidatorsMessages.EMPLOYEE_CONFLICT)
                    });
                }

                if (request.IdDivision.HasValue &&
                    !(await divisionRepository.GetAll().AnyAsync(d => d.Id == request.IdDivision).ConfigureAwait(false)))
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Division)
                    });
                }

                if (!(await localizacionRepository.GetAll().AnyAsync(d => d.Id == request.IdLocalizacion).ConfigureAwait(false)))
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Location)
                    });
                }

                if (!(await repository.GetAll().AnyAsync(d => d.Id == request.IdResponsable).ConfigureAwait(false)))
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.IdResponsable)
                    });
                }
            }
        }
    }
}
