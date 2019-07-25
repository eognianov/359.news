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