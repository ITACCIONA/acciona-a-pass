using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace AccionaCovid.Application.Core
{
    /// <summary>
    /// Decorador del método handle del command handler
    /// </summary>
    /// <typeparam name="TRequest">Tipo de los datos de la petición</typeparam>
    /// <typeparam name="TResponse">Tipo de la información de retorno</typeparam>
    public class ValidatorPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Conjunto de validadores que deben ser superados para admitir los datos de entrada
        /// </summary>
        private readonly IValidator<TRequest>[] validators;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="validators">conjunto de validadores específicos que comprobarán la calidad de la petición</param>
        /// <param name="inner">Instancia del command handler utilizado para la request</param>
        public ValidatorPipeline(IValidator<TRequest>[] validators)
        {
            this.validators = validators;
        }

        /// <summary>
        /// Decorador del método principal del request handler
        /// </summary>
        /// <param name="request">Instancia de la petición</param>
        /// <param name="next">Manejador del decorador</param>
        /// <returns>Tarea de retorno delegada desde el handler no decorador</returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            List<ValidationFailure> failures = validators.Select(v => v.Validate(request))
                                                           .SelectMany(result => result.Errors)
                                                           .Where(error => error != null)
                                                           .ToList();

            if (failures.Count > 0)
            {
                List<ErrorMessage> messages = new List<ErrorMessage>();
                messages.AddRange(failures.Select(f => new ErrorMessage() { Code = f.ErrorCode, Message = f.ErrorMessage }));
                throw new MultiMessageValidationException(messages);
            }

            return await next().ConfigureAwait(false);
        }
    }
}
