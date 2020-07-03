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
    public class GetSymptomInquiryResult
    {
        public class GetSymptomInquiryResultRequest : IRequest<GetSymptomInquiryResultResponse>
        {
            public int IdEmployee { get; set; }
        }

        /// <summary>
        /// Validador del request
        /// </summary>
        public class GetSymptomInquiryResultRequestValidator : AbstractValidator<GetSymptomInquiryResultRequest>
        {
            /// <summary>
            /// Validaciones a realizar
            /// </summary>
            public GetSymptomInquiryResultRequestValidator()
            {
                RuleFor(v => v.IdEmployee).Must(p => p > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Respuesta del procesado del evento
        /// </summary>
        public class GetSymptomInquiryResultResponse
        {
            public List<Symptoms> SymptomsByDay { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public GetSymptomInquiryResultResponse(List<Symptoms> symptoms)
            {
                this.SymptomsByDay = symptoms;

                this.SymptomsByDay = this.SymptomsByDay.OrderByDescending(c => c.Date).ToList();
            }
        }

        public class Symptoms
        {
            public DateTime Date { get; set; }
            public List<Symptom> Values { get; set; }
        }

        public class Symptom
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Value { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetSymptomInquiryResultCommandHandler : BaseCommandHandler<GetSymptomInquiryResultRequest, GetSymptomInquiryResultResponse>
        {
            private readonly IRepository<Empleado> repository;
            private readonly IRepository<ResultadoEncuestaSintomas> repositoryEncuesta;
            private readonly IRepository<TipoSintomas> repositorySintomas;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetSymptomInquiryResultCommandHandler(IRepository<TipoSintomas> repositorySintomas, IRepository<Empleado> repository, IRepository<ResultadoEncuestaSintomas> repositoryEncuesta)
            {
                this.repositorySintomas = repositorySintomas;
                this.repository = repository;
                this.repositoryEncuesta = repositoryEncuesta;
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<GetSymptomInquiryResultResponse> Handle(GetSymptomInquiryResultRequest request, CancellationToken cancellationToken)
            {
                bool empleadoExists = await repository
                    .Any(e => e.Id == request.IdEmployee)
                    .ConfigureAwait(false);

                if (!empleadoExists)
                {
                    throw new MultiMessageValidationException(new ErrorMessage()
                    {
                        Code = "NOT_FOUND",
                        Message = string.Format(ValidatorsMessages.NOT_FOUND, ValidatorFields.Empleado)
                    });
                }

                var fichaMedica = await repository
                    .GetBy(e => e.Id == request.IdEmployee)
                    .Select(e => e.IdFichaMedicaNavigation)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                string[] nombres = Enum.GetNames(typeof(TipoSintomas.ParameterTypes));
                Dictionary<string, TipoSintomas> tipoSintomas = await repositorySintomas.GetBy(ts => nombres.Contains(ts.Nombre)).ToDictionaryAsync(ts => ts.Nombre);

                var allSymptoms = await repositoryEncuesta
                    .GetBy(s => s.IdFichaMedica == fichaMedica.Id)
                    .Select(s => new { s.IdTipoSintoma, s.IdTipoSintomaNavigation.Nombre, s.LastActionDate, s.Valor })
                    .ToListAsync()
                    .ConfigureAwait(false);

                var groupedSymptomsByDay = allSymptoms.GroupBy(s => new DateTime(s.LastActionDate.Year, s.LastActionDate.Month, s.LastActionDate.Day))
                    .Select(group => new Symptoms
                    {
                        Date = group.Key,
                        Values = group.Select(s => new Symptom() { Id = s.IdTipoSintoma, Name = s.Nombre, Value = s.Valor }).ToList(),
                    })
                    .ToList();

                return new GetSymptomInquiryResultResponse(groupedSymptomsByDay);
            }
        }
    }
}
