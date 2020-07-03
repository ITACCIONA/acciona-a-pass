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
    /// Servicio que obtiene la informacion de test rapidos y PCL
    /// </summary>
    public class GetMedicalTest
    {
        /// <summary>
        /// </summary>
        public class GetMedicalTestRequest : IRequest<GetMedicalTestResponse>
        {
            public GetMedicalTestRequest(int idEmpleado)
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
        public class GetMedicalTestRequestValidator : AbstractValidator<GetMedicalTestRequest>
        {
            /// <summary>
            /// Validaciones a realizar
            /// </summary>
            public GetMedicalTestRequestValidator()
            {
                RuleFor(v => v.IdEmpleado).Must(p => p > 0).WithFormatMessage(ValidatorsMessages.VALUE_CANNOT_BE_ZERO_OR_NEGATIVE, ValidatorFields.IdEmpleado);
            }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetMedicalTestResponse
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="testRapidos"></param>
            /// <param name="testPcr"></param>
            public GetMedicalTestResponse(List<ResultadoTestMedico> testRapidos, List<ResultadoTestPcr> testPcr)
            {
                TestRapidos = new List<GetMedicalTestRapido>();
                TestPCR = new List<GetMedicalTestPCR>();

                testRapidos = testRapidos.OrderByDescending(c => c.FechaTest).ToList();
                testPcr = testPcr.OrderByDescending(c => c.FechaTest).ToList();

                foreach (var item in testRapidos)
                {
                    GetMedicalTestRapido newTest = new GetMedicalTestRapido()
                    {
                        Id = item.Id,
                        FechaTest = item.FechaTest,
                        Control = item.Control,
                        IGG = item.Igg,
                        IGM = item.Igm
                    };

                    TestRapidos.Add(newTest);
                }

                foreach (var item in testPcr)
                {
                    GetMedicalTestPCR newTest = new GetMedicalTestPCR()
                    {
                        Id = item.Id,
                        FechaTest = item.FechaTest,
                        Positivo = item.Positivo
                    };

                    TestPCR.Add(newTest);
                }
            }

            /// <summary>
            /// Listado de test rapidos
            /// </summary>
            public List<GetMedicalTestRapido> TestRapidos { get; set; }

            /// <summary>
            /// Listado de test PCR
            /// </summary>
            public List<GetMedicalTestPCR> TestPCR { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetMedicalTestRapido
        {
            public int Id { get; set; }

            /// <summary>
            /// Fecha del test
            /// </summary>
            public DateTimeOffset FechaTest { get; set; }

            /// <summary>
            /// Control
            /// </summary>
            public bool Control { get; set; }

            /// <summary>
            /// IGG
            /// </summary>
            public bool IGG { get; set; }

            /// <summary>
            /// IGM
            /// </summary>
            public bool IGM { get; set; }
        }

        /// <summary>
        /// Informacion de una lista paginada
        /// </summary>
        public class GetMedicalTestPCR
        {
            public int Id { get; set; }

            /// <summary>
            /// Fecha del test
            /// </summary>
            public DateTimeOffset FechaTest { get; set; }

            /// <summary>
            /// Positivo
            /// </summary>
            public bool Positivo { get; set; }
        }

        /// <summary>
        /// Manejador del procesado de un evento
        /// </summary>
        public class GetMedicalTestCommandHandler : BaseCommandHandler<GetMedicalTestRequest, GetMedicalTestResponse>
        {
            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<ResultadoTestMedico> repositoryResultadoTestMedico;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<ResultadoTestPcr> repositoryResultadoTestPcr;

            /// <summary>
            /// Repositorio a usar.
            /// </summary>
            private readonly IRepository<Empleado> repositoryEmpleado;

            /// <summary>
            /// Constructor
            /// </summary>
            public GetMedicalTestCommandHandler(IRepository<ResultadoTestMedico> repositoryResultadoTestMedico, IRepository<ResultadoTestPcr> repositoryResultadoTestPcr, IRepository<Empleado> repositoryEmpleado)
            {
                this.repositoryResultadoTestPcr = repositoryResultadoTestPcr ?? throw new ArgumentNullException(nameof(repositoryResultadoTestPcr));
                this.repositoryResultadoTestMedico = repositoryResultadoTestMedico ?? throw new ArgumentNullException(nameof(repositoryResultadoTestMedico));
                this.repositoryEmpleado = repositoryEmpleado ?? throw new ArgumentNullException(nameof(repositoryEmpleado));
            }

            /// <summary>
            /// Handle
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task<GetMedicalTestResponse> Handle(GetMedicalTestRequest request, CancellationToken cancellationToken)
            {
                Empleado empleado = await ValidacionEmpleado(request.IdEmpleado).ConfigureAwait(false);

                var listaTestRap = await repositoryResultadoTestMedico.GetAll()
                                        .Where(c => c.IdFichaMedica == empleado.IdFichaMedica.Value).ToListAsync().ConfigureAwait(false);

                var listaTestPcr = await repositoryResultadoTestPcr.GetAll()
                                        .Where(c => c.IdFichaMedica == empleado.IdFichaMedica.Value).ToListAsync().ConfigureAwait(false);

                return new GetMedicalTestResponse(listaTestRap, listaTestPcr);
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
