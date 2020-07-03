// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace AccionaCovid.Identity.MvcControllers.Account
{
    public class LoginInputModel : LoginViewModel
    {
        [Required]
        public override string Username { get; set; }
        [Required]
        public override string Password { get; set; }
        public bool RememberLogin { get; set; }
    }
}