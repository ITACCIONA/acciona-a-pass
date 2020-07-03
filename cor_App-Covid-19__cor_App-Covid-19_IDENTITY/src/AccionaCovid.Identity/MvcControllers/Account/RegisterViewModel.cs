// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace AccionaCovid.Identity.MvcControllers.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Views.Account.Register), ErrorMessageResourceName = "PASSWORDS_MISMATCH")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string DNI { get; set; }

        public string Telefono { get; set; }

        public string ReturnUrl { get; set; }
    }
}