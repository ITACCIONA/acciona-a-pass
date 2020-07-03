using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// Controlador base para el uso de mediator
    /// </summary>
    public class MediatorBaseController : ControllerBase
    {
        /// <summary>
        /// </summary>
        protected IMediator Mediator { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public MediatorBaseController(IMediator mediator)
        {
            this.Mediator = mediator;
        }
    }
}
