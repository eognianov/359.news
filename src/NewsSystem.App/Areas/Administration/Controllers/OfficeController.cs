using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsSystem.App.Areas.Administration.Controllers
{
   
    public class OfficeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}