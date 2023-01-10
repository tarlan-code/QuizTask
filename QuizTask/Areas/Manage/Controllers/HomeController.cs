using Microsoft.AspNetCore.Mvc;

namespace QuizTask.Areas.Manage.Controllers
{
    public class HomeController : Controller
    {
        [Area(nameof(Manage))]
        public IActionResult Index()
        {
            return View();
        }
    }
}
