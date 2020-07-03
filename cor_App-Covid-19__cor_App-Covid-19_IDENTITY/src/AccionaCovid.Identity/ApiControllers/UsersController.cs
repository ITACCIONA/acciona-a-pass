using AccionaCovid.Identity.Data;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccionaCovid.Identity.ApiControllers
{
    [ApiController, Route("api/Users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStringLocalizer<UsersController> localizer;
        private readonly IdentityDbContext context;
        IdentityServer4.Validation.IIntrospectionRequestValidator val;

        public UsersController(UserManager<ApplicationUser> userManager, IStringLocalizer<UsersController> localizer,
            IdentityDbContext context, IdentityServer4.Validation.IIntrospectionRequestValidator val)
        {
            this.userManager = userManager;
            this.localizer = localizer;
            this.context = context;
            this.val = val;
        }

        /// <summary>
        /// Metodo que resetea la password de un usuario local externo (de contrata)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("reset")]
        [Authorize(AuthenticationSchemes = "AzureADBearer")]
        public async Task<IActionResult> GetLoginMedicalServices([FromBody]string userId)
        {
            var upnClaim = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Upn) ?? (User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email) ?? User.Claims.SingleOrDefault(c => c.Type == "preferred_username"));

            if (upnClaim == null)
                return Unauthorized();

            int idEmpleadoResponsable = await context.Empleado
                    .Include(c => c.EmpleadoRole)
                        .ThenInclude(c => c.IdRoleNavigation)
                    .Where(c => c.Upn.Trim().ToUpper() == upnClaim.Value.Trim().ToUpper())
                    .Select(e => e.Id)
                    .SingleOrDefaultAsync().ConfigureAwait(false);

            if (idEmpleadoResponsable == 0)
                return Unauthorized();

            var user = await userManager.Users
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return Unauthorized();

            bool dbPassCheck = await context.Empleado
                .AnyAsync(x => x.Id == user.IdEmpleado &&
                            x.IdFichaLaboralNavigation.IsExternal &&
                            x.IdFichaLaboralNavigation.IdResponsableDirecto == idEmpleadoResponsable).ConfigureAwait(false);

            if (!dbPassCheck)
                return Unauthorized();


            string password = GeneratePassword(userManager.Options.Password);
            string hashedPassword = userManager.PasswordHasher.HashPassword(user, password);
            user.PasswordHash = hashedPassword;
            user.PasswordExpiration = new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero);

            var result = await userManager.UpdateAsync(user).ConfigureAwait(false);


            if (!result.Succeeded)
            {
                throw new Exception("Error updating user");
            }
            return Ok(new { otp = password });
        }


        private string GeneratePassword(PasswordOptions validator)
        {
            if (validator == null)
                return null;

            bool requireNonLetterOrDigit = validator.RequireNonAlphanumeric;
            bool requireDigit = validator.RequireDigit;
            bool requireLowercase = validator.RequireLowercase;
            bool requireUppercase = validator.RequireUppercase;

            string randomPassword = string.Empty;

            int passwordLength = validator.RequiredLength + 4;

            Random random = new Random();
            while (randomPassword.Length != passwordLength)
            {
                int randomNumber = random.Next(48, 122);  // >= 48 && < 122 
                if (randomNumber == 95 || randomNumber == 96) continue;  // != 95, 96 _'

                char c = Convert.ToChar(randomNumber);

                if (requireDigit)
                    if (char.IsDigit(c))
                        requireDigit = false;

                if (requireLowercase)
                    if (char.IsLower(c))
                        requireLowercase = false;

                if (requireUppercase)
                    if (char.IsUpper(c))
                        requireUppercase = false;

                if (requireNonLetterOrDigit)
                    if (!char.IsLetterOrDigit(c))
                        requireNonLetterOrDigit = false;

                randomPassword += c;
            }

            if (requireDigit)
                randomPassword += Convert.ToChar(random.Next(48, 58));  // 0-9

            if (requireLowercase)
                randomPassword += Convert.ToChar(random.Next(97, 123));  // a-z

            if (requireUppercase)
                randomPassword += Convert.ToChar(random.Next(65, 91));  // A-Z

            if (requireNonLetterOrDigit)
                randomPassword += Convert.ToChar(random.Next(33, 48));  // symbols !"#$%&'()*+,-./

            return randomPassword;
        }
    }
}
