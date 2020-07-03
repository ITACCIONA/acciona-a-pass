using AccionaCovid.Domain.Model;
using AccionaCovid.Identity.Data;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccionaCovid.Identity.Infrastructure
{
    public class ProfileService : IProfileService
    {
        private UserManager<ApplicationUser> userManager;
        private IdentityDbContext dbContext;

        const string _medicalServicesRole = "ServicioMedico";

        const string _medicalServicesClaim = "MedicalServices";

        const string _prlRole = "PRL";

        const string _prlClaim = "PRL";

        private readonly ILogger<ProfileService> logger;

        /// <summary>
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        public ProfileService(UserManager<ApplicationUser> userManager, IdentityDbContext context, ILogger<ProfileService> logger)
        {
            this.userManager = userManager;
            this.dbContext = context;
            this.logger = logger;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            /* Añadimos roles configurados directamente en ASP .Net Identity */
            var user = await userManager.GetUserAsync(context.Subject).ConfigureAwait(false);
            var roles = await userManager.GetRolesAsync(user).ConfigureAwait(false);

            foreach (string rol in roles)
            {
                context.IssuedClaims.Add(new Claim("role", rol));
            }

            /* If its an admin, it doesn't have an employee */
            if (roles.Contains("Administrator"))
            {
                context.IssuedClaims.Add(new Claim("IdUser", user.IdEmpleado > 0 ? user.IdEmpleado.ToString() : "-1"));
                context.IssuedClaims.Add(new Claim("UserFullName", user.UserName));
                logger?.LogInformation($"IDENTITY SERVER SUCCESFULL AUTENTICATION -> {user.IdEmpleado} as Admin");
                return;
            }

            // obtenemos el IdEmpleado
            var upnClaim = context.Subject.FindFirst(c => c.Type == ClaimTypes.Upn)
                ?? context.Subject.FindFirst(c => c.Type == ClaimTypes.Email)
                    ?? context.Subject.FindFirst(c => c.Type == "preferred_username");

            Expression<Func<Empleado, bool>> filter = e => false;

            if (user.IdEmpleado > 0)
                filter = e => e.Id == user.IdEmpleado;
            else if (upnClaim != null)
                filter = e => e.Upn.Trim().ToUpper() == upnClaim.Value.Trim().ToUpper();


            Empleado emp = await dbContext.Empleado.FirstOrDefaultAsync(filter).ConfigureAwait(false);

            if (emp != null)
            {
                context.IssuedClaims.Add(new Claim("IdUser", emp.Id.ToString()));
                context.IssuedClaims.Add(new Claim("UserFullName", $"{emp.Nombre} {emp.Apellido}".Trim()));
                logger?.LogInformation($"IDENTITY SERVER SUCCESFULL AUTENTICATION -> UPN CLAIM {upnClaim} AND IdEmpleado {user.IdEmpleado}");
            }
            else
            {
                context.IssuedClaims.Add(new Claim("IdUser", "-1"));
                logger?.LogError($"IDENTITY SERVER FAIL AUTENTICATION -> UPN CLAIM {upnClaim} not found in System. Claims: {string.Join(" | ", context.Subject.Claims.Select(c => $"{c.Type}:{c.Value}").ToArray())}");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
        }
    }
}
