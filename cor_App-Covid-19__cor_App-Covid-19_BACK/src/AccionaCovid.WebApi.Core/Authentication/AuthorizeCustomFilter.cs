using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccionaCovid.WebApi.Core.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class AuthorizeRole : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Roles"></param>
        public AuthorizeRole(params string[] Roles)
        {
            this.Roles = Roles.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Role"></param>
        public AuthorizeRole(string Role)
        {
            this.Roles = new List<string>() { Role };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuthorizeCustomFilter : IAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> AllAccessRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AuthorizeCustomFilter() : base()
        {
            AllAccessRoles = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AllAccessRoles"></param>
        public AuthorizeCustomFilter(List<string> AllAccessRoles) : base()
        {
            this.AllAccessRoles = AllAccessRoles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AllAccessRol"></param>
        public AuthorizeCustomFilter(string AllAccessRol) : base()
        {
            this.AllAccessRoles = new List<string>() { AllAccessRol };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var controllerInfo = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerInfo.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any())
                return;

            bool allowAccess = this.AllAccessRoles.Any(role => context.HttpContext.User.IsInRole(role));
            foreach (AuthorizeRole attr in controllerInfo.MethodInfo.GetCustomAttributes(typeof(AuthorizeRole), false))
            {
                if (attr.Roles.Any(role => context.HttpContext.User.IsInRole(role)))
                    allowAccess = true;
            }

            if (!allowAccess)
                context.Result = new UnauthorizedResult();
        }
    }
}
