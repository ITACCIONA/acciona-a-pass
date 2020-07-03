// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AccionaCovid.Domain.Model;
using AccionaCovid.Identity.Data;
using AccionaCovid.Identity.Infrastructure;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AccionaCovid.Identity.MvcControllers.Account
{
    /// <summary>
    /// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
    /// The login service encapsulates the interactions with the user data store.
    /// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
    /// </summary>
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IStringLocalizer<AccountController> localizer;
        private readonly Lazy<IdentityDbContext> lazyContext;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            IEmailSender emailSender,
            IStringLocalizer<AccountController> localizer,
            Lazy<IdentityDbContext> lazyContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _events = events;
            _emailSender = emailSender;
            this.localizer = localizer;
            this.lazyContext = lazyContext;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            return View(vm);
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string button)
        {
            if (button == "employee")
            {
                return RedirectToAction("Challenge", "External", new { model.ReturnUrl });
            }
            else if (button == "external")
            {
                // build a model so we know what to show on the login page
                var vm = await BuildLoginViewModelAsync(model.ReturnUrl, true);
                return RedirectToAction(nameof(LocalLogin), vm);
            }
            else
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public IActionResult LocalLogin(LoginViewModel model) => View(nameof(Login), model);

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LocalLogin(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (await _clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                var resultCheck = user != null ? await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true) : new Microsoft.AspNetCore.Identity.SignInResult();
                if (resultCheck.Succeeded)
                {
                    if (user.PasswordExpiration != null && user.PasswordExpiration.Value < DateTime.Now)
                    {
                        bool isExternal = await lazyContext.Value.Empleado
                            .AnyAsync(e => e.Id == user.IdEmpleado &&
                                           e.IdFichaLaboralNavigation.IsExternal).ConfigureAwait(false);
                        if (!isExternal)
                        {
                            await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "Blocked user"));
                            ModelState.AddModelError(string.Empty, localizer["BLOCKED_USER"]);
                            return View(nameof(Login), await BuildLoginViewModelAsync(model));
                        }

                        return View("ChangePassword", new ChangePasswordViewModel { Username = model.Username, ReturnUrl = model.ReturnUrl });
                    }

                    await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);

                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            // if the client is PKCE then we assume it's native, so this change in how to
                            // return the response is for better UX for the end user.
                            return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }

                if (resultCheck.IsLockedOut)
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "Blocked user"));
                    ModelState.AddModelError(string.Empty, localizer["BLOCKED_USER"]);
                }
                else
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "Invalid Credentials"));
                    ModelState.AddModelError(string.Empty, localizer["INVALID_CREDENTIALS"]);
                }
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(nameof(Login), vm);
        }


        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Handle postback from forgot password
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (model.ResetByDNI && string.IsNullOrWhiteSpace(model.ResetEmail))
            {
                ModelState.AddModelError("Error", localizer["COULD_NOT_SEND_EMAIL"]);
            }

            if (ModelState.IsValid)
            {
                string userKeyData = model.ResetByDNI ? model.DNI : model.Email;

                var user = await _userManager.FindByNameAsync(userKeyData);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                //Comprobar que si pide reseteo por DNI no sea empleado de contrata
                bool externalEmployee = model.ResetByDNI && await lazyContext.Value.Empleado
                    .Include(e => e.IdFichaLaboralNavigation)
                    .AnyAsync(e => e.Id == user.IdEmpleado.GetValueOrDefault() &&
                                   e.IdFichaLaboralNavigation.IsExternal);

                if (externalEmployee)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // Comprobar que si pide reseteo por DNI y tengo mail personal de workday no se
                // se pida reseteo desde otro mail distinto del reportado como personal en workday
                bool differentPersonalMail = model.ResetByDNI && await lazyContext.Value.Empleado
                    .AnyAsync(e => e.Id == user.IdEmpleado.GetValueOrDefault() &&
                                   e.Mail != null && e.Mail.Trim() != "" &&
                                   e.Mail != model.ResetEmail);
                if (differentPersonalMail)
                {
                    // ACCIONA pide que en esta situación concreta sí se informe al usuario del problema
                    // aunque contraviene las buenas prácticas en materia de seguridad.
                    ModelState.AddModelError("Error", localizer["PERSONAL_MAIL_MISMATCH"]);
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(
                    "ResetPassword",
                    "Account",
                    values: null,
                    protocol: Request.Scheme);

                var uriBuilder = new UriBuilder(callbackUrl);
                var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                query["code"] = code;
                uriBuilder.Query = query.ToString();

                string email = model.ResetByDNI ? model.ResetEmail : model.Email;

                bool sent = await _emailSender.SendEmailAsync(
                     email,
                     localizer["RESET_PASSWORD"],
                     $"{localizer["RESET_PASS_URL"]} <a href='{HtmlEncoder.Default.Encode(uriBuilder.ToString())}'>{localizer["RESET_PASS_CLICKING"]}</a>.");

                if (sent)
                {
                    return View("ForgotPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError("Error", localizer["COULD_NOT_SEND_EMAIL"]);
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string code)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                return View(new ResetPasswordViewModel { Code = code });
            }
        }

        /// <summary>
        /// Handle postback from reset password
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return View("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Handle postback from change external password
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return View("ResetPasswordConfirmation");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
            if (result.Succeeded)
            {
                user.PasswordExpiration = null;
                await _userManager.UpdateAsync(user);
                await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: true);

                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

                // check if we are in the context of an authorization request
                var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
                if (context != null)
                {
                    if (await _clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect(model.ReturnUrl);
                }

                // request for a local page
                if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect("~/");
                }
                else
                {
                    // user might have clicked on a malicious link - should be logged
                    throw new Exception("invalid return URL");
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [HttpGet]
        public IActionResult RegisterExternal(string returnUrl)
        {
            return View(new RegisterExternalViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        /// <summary>
        /// Handle postback from register
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterExternal(RegisterExternalViewModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                Empleado empleado = null;

                IdentityDbContext context = lazyContext.Value;
                List<Empleado> empleados = await context.Empleado
                    .Where(e => e.Nif == model.DNI)
                    .Take(2)
                    .ToListAsync().ConfigureAwait(false);

                // Si no encuentra exclusivamente a uno
                if (empleados.Count != 1)
                {
                    ModelState.AddModelError("Error", localizer["WORKDAY_NOT_FOUND"]);
                    return View(new RegisterExternalViewModel { ReturnUrl = model.ReturnUrl });
                }

                empleado = empleados.First();

                // Si ya está registrado
                if (await _userManager.Users.AnyAsync(u => u.IdEmpleado == empleado.Id))
                {
                    ModelState.AddModelError("Error", localizer["WORKDAY_NOT_FOUND"]);
                    return View(new RegisterExternalViewModel { ReturnUrl = model.ReturnUrl });
                }

                ApplicationUser user = new ApplicationUser { UserName = model.DNI, IdEmpleado = empleado.Id };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Si por configuración está activada la confirmación de email, se hace por código.
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        user.EmailConfirmed = true;
                        await _userManager.UpdateAsync(user).ConfigureAwait(false);
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(model.ReturnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // something went wrong, show form with error
            return View(new RegisterExternalViewModel { ReturnUrl = model.ReturnUrl });
        }

        [HttpGet]
        public IActionResult RegisterConfirmation() => View();

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View(new ConfirmEmailViewModel
                {
                    StatusMessage = localizer["ERROR_CONFIRMING_EMAIL"]
                });
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(new ConfirmEmailViewModel
            {
                StatusMessage = result.Succeeded ? localizer["CONFIRMED_EMAIL"]
                    : localizer["ERROR_CONFIRMING_EMAIL"]
            });
        }


        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, bool? isLocalLogin = null)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (context?.IdP != null)
            {
                // this is meant to short circuit the UI and only trigger the one external IdP
                return new LoginViewModel
                {
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };
            }

            return new LoginViewModel
            {
                IsLocalLogin = isLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.IsLocalLogin = model.IsLocalLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}
