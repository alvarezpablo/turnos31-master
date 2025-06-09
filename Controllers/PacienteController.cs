using Microsoft.AspNetCore.Mvc;

namespace Turnos31.Controllers
{
    public class PacienteController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}