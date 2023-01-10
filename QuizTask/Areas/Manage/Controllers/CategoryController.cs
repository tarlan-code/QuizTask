using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizTask.DAL;
using QuizTask.Models;
using System.Drawing;

namespace QuizTask.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    public class CategoryController : Controller
    {

        readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Categories);
        }

        public IActionResult Delete(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest();
            Category cat = _context.Categories.Find(Id);
            if (cat == null) return NotFound();
            _context.Categories.Remove(cat);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Category cat)
        {

            if (!ModelState.IsValid) return View();
            cat.IsActive = true;
            _context.Add(cat);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            
        }


        public IActionResult Update(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest();

            Category cat = _context.Categories.Find(Id);
            if (cat is null) return NotFound();


            return View(cat);
        }

      
        [HttpPost]
        public IActionResult Update(int? Id, Category cat)
        {
            if (Id is null || Id <= 0 || Id != cat.Id) return BadRequest();
            if (!ModelState.IsValid) return View();
            Category exist = _context.Categories.Find(Id);
            if (exist is null) return NotFound();

            exist.Name = cat.Name;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangeStatus(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Category cat = _context.Categories.Find(id);
            if (cat == null) return NotFound();

            if (cat.IsActive) cat.IsActive = false;
            else cat.IsActive = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }




    }
}
