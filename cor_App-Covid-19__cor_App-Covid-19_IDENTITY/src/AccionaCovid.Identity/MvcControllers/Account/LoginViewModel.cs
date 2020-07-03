// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace AccionaCovid.Identity.MvcControllers.Account
{
    public class LoginViewModel
    {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public bool? IsLocalLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}