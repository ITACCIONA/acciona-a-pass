using AccionaCovid.Application.Services.Master;
using AccionaCovid.WebApi.Core;
using MediatR;
using System.Threading.Tasks;

namespace AccionaCovid.WebApi.HostedServices
{
    /// <summary>
    /// Tarea en segundo plano para actualizar estado de visitas caducadas.
    /// </summary>
    public class IntegrationFileStorageCronWorker : ICronWorker
    {
        /// <summary>
        /// mediador
        /// </summary>
        protected IMediator Mediator { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator">mediator</param>
        public IntegrationFileStorageCronWorker(IMediator mediator)
        {
            Mediator = mediator;
        }

        /// <summary>
        /// Trabajo a realizar
        /// </summary>
        /// <returns></returns>
        public Task DoWork()
        {
            Mediator.Send(new IntegrationFileStorage.IntegrationFileStorageRequest());
            return Task.CompletedTask;
        }
    }
}
