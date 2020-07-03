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

namespace AccionaCovid.Application.Services.Master
{
    /// <summary>
    /// Servicio que obtiene la informacion maestro de empleados
    /// </summary>
    public class GetEmployees
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetEmployeesRequest : IRequest<List<GetMasterEmployeesResponse>>
        {
            public string Filtro { get; set; }
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetMasterEmployeesResponse
        {
            /// <summary>
            /// Identificador
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Nombre del empleado
            /// </summary>
            public string Nombre { get; set; }

            /// <summary>
            /// Apellidos del empleado
            /// </summary>
            public string Apellidos { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class GetEmployeesValidator : AbstractValidator<GetEmployeesRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetEmployeesValidator()
            {
                RuleFor(r => r.Filtro).NotNull().NotEmpty().WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_NULL_OR_EMPTY, ValidatorFields.Filtro);
                RuleFor(r => r.Filtro).Must(s => s.Length>=3).WithFormatMessage(ValidatorsMessages.VALUE_LESS_3_CHARACTERS, ValidatorFields.Filtro);
            }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetEmployeesCommandHandler : BaseCommandHandler<GetEmployeesRequest, List<GetMasterEmployeesResponse>>
        {
            private readonly IRepository<Empleado> repository;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetEmployeesCommandHandler(IRepository<Empleado> repository)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<List<GetMasterEmployeesResponse>> Handle(GetEmployeesRequest request, CancellationToken cancellationToken)
            {
                var empleados = await repository.GetAll()
                    .Where(e => (e.Nombre + " " + e.Apellido).Contains(request.Filtro))
                    .OrderBy(e => e.Nombre).ThenBy(e => e.Apellido)
                    .Select(e => new { e.Id, e.Nombre, e.Apellido})
                    .ToListAsync().ConfigureAwait(false);

                var result = empleados.Select(x => new GetMasterEmployeesResponse
                {
                     Id= x.Id,
                     Nombre = x.Nombre,
                     Apellidos = x.Apellido
                });

                return result.ToList();
            }
        }
    }
}
