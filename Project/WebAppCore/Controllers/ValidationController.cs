﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAppCore.Controllers
{
    public class ValidationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost(Name = "IsEmailAvailable")]
        public IActionResult IsEmailAvailable(String email)
        {
            return new JsonResult(false);
        }
    }
}