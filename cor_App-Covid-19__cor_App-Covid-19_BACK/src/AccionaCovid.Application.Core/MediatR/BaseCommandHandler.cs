using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AccionaCovid.Crosscutting;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccionaCovid.Application.Core
{
    /// <summary>
    /// Basse class for MediatR command handlers
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class BaseCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Stopwatch
        /// </summary>
        readonly Stopwatch stopWatch;
        
        /// <summary>
        /// Constructor de la clase abstracta
        /// </summary>
        protected BaseCommandHandler()
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
        }

        /// <summary>
        /// Idioma obtenido de la cabecera de la solicitud
        /// </summary>
        public string Idioma { get; set; }

        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public int IdUser { get; set; }

        /// <summary>
        /// Nombre de los roles de usuario
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Instancia del logger
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Escribe en el log el tiempo de respuests de una operacion
        /// </summary>
        protected void SendTimeOperationToLogger(string operationName)
        {
            string nameClassDerived = this.GetType().Name;
            //string nameClassDerived = MethodBase.GetCurrentMethod().DeclaringType.Name;
            stopWatch.SendTimeOperation($"{nameClassDerived} -> {operationName}", Logger);
        }

        /// <summary>
        /// IRequestHandler implementation
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
