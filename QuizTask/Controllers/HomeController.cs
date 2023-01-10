using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizTask.DAL;

namespace QuizTask.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Products.Include(p=>p.ProductCategories).ThenInclude(pc=>pc.Category).Include(p => p.ProductImages));
        }

    }
}