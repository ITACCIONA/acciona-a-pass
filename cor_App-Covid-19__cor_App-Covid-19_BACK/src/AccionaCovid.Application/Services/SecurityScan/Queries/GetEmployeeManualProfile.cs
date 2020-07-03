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

namespace AccionaCovid.Application.Services.SecurityScan
{
    /// <summary>
    /// Query para obtener el perfil de un empleado de forma manual.
    /// </summary>
    public class GetEmployeeManualProfile
    {
        /// <summary>
        /// Request
        /// </summary>
        public class GetEmployeeManualProfileRequest : IRequest<GetEmployeeManualProfileResponse>
        {
            /// <summary>
            /// DNI del empleado
            /// </summary>
            public string DniEmpleado { get; set; }

            /// <summary>
            /// telefono del empleado
            /// </summary>
            public string TelefonoEmpleado { get; set; }
        }

        /// <summary>
        /// Validador
        /// </summary>
        public class GetEmployeeManualProfileValidator : AbstractValidator<GetEmployeeManualProfileRequest>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetEmployeeManualProfileValidator()
            {
                RuleFor(r => r.DniEmpleado).NotNull().NotEmpty().WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_NULL_OR_EMPTY, ValidatorFields.DNI);
                RuleFor(r => r.DniEmpleado).IsValidLatinString().WithFormatMessage(ValidatorsMessages.VALUE_WRONG_FORMAT, ValidatorFields.DNI);
                RuleFor(r => r.TelefonoEmpleado).Length(5, 25).WithFormatMessage(ValidatorsMessages.VALUE_WRONG_FORMAT, ValidatorFields.Telefono);
            }
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetEmployeeManualProfileResponse
        {

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

        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetEmployeeManualProfileCommandHandler : BaseCommandHandler<GetEmployeeManualProfileRequest, GetEmployeeManualProfileResponse>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetEmployeeManualProfileCommandHandler(IRepository<Empleado> repositoryEmpleado)
            {
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async override Task<GetEmployeeManualProfileResponse> Handle(GetEmployeeManualProfileRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.DniEmpleado, request.TelefonoEmpleado).ConfigureAwait(false);

                GetEmployeeManualProfileResponse response = new GetEmployeeManualProfileResponse()
                {
                    IdEmpleado = empleado.Id,
                    NombreEmpleado = empleado.Nombre,
                    InicialesEmpleado = empleado.Iniciales,
                    EdadEmpleado = empleado.CalcularEdad(),
                    NumEmpleado = empleado.NumEmpleado ?? empleado.Id,
                    ApellidosEmpleado = empleado.Apellido,
                    DNI = empleado.Nif,
                    MailEmpleado = empleado.IdFichaLaboralNavigation?.MailProf,
                    TelefonoEmpleado = empleado.IdFichaLaboralNavigation?.TelefonoCorp
                };

                return response;
            }

            /// <summary>
            /// Metodo que valida si existe el empleado y su ficha
            /// </summary>
            /// <param name="dniEmpleado"></param>
            /// <param name="telefonoEmpleado"></param>
            /// <returns></returns>
            private async Task<Empleado> ValidacionEmpleado(string dniEmpleado, string telefonoEmpleado)
            {
                Empleado empleado = await repositoryEmpleado
                                    .GetAll()
                                        .Include(p => p.IdFichaMedicaNavigation)
                                        .Include(c => c.IdFichaLaboralNavigation)
                                    .SingleOrDefaultAsync(p => p.Nif == dniEmpleado)
                                    .ConfigureAwait(false);

                if (empleado == null && !string.IsNullOrWhiteSpace(telefonoEmpleado))
                {
                    var empleados = await repositoryEmpleado
                        .GetAll()
                            .Include(p => p.IdFichaMedicaNavigation)
                            .Include(c => c.IdFichaLaboralNavigation)
                    .Where(p => p.Telefono.Contains(telefonoEmpleado))
                    .ToListAsync()
                    .ConfigureAwait(false);

                    if (empleados.Count != 1)
                    {
                        throw new MultiMessageValidationException(new ErrorMessage()
                        {
                            Code = "NOT_FOUND",
                            Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Usuario)
                        });
                    }
                    empleado = empleados.SingleOrDefault();
                }

                if (empleado == null)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Usuario)
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
