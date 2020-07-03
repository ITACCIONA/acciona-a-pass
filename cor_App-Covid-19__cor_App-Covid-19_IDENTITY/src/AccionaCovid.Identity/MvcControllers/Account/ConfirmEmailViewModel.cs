// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AccionaCovid.Identity.MvcControllers.Account
{
    public class ConfirmEmailViewModel
    {
        [TempData]
        public string StatusMessage { get; set; }
    }
}