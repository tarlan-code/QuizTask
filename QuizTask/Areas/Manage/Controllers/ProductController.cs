using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuizTask.DAL;
using QuizTask.Models;
using QuizTask.Utilies.Extensions;
using QuizTask.ViewModels;

namespace QuizTask.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    public class ProductController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Products.Include(p=>p.ProductCategories).ThenInclude(pc=>pc.Category).Include(p=>p.ProductImages));
        }

        public IActionResult Delete(int? Id)
        {
            if (Id is null || Id < 0) return BadRequest();

            Product product = _context.Products.Find(Id);
            if (product is null) return NotFound();

            foreach (var item in _context.ProductImages.Where(pi => pi.ProductId == product.Id))
            {
                item.ImgUrl.DeleteFile(_env.WebRootPath, Path.Combine("assets", "img", "product"));
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductVM prod)
        {

            var coverImg= prod?.CoverImage;
            var otherImgs = prod?.OtherImages;


            string result = coverImg?.CheckValidate("image/", 300);

            if (result?.Length>0)
            {
                ModelState.AddModelError("CoverImage", result);
            }
            
            if(otherImgs is not null)
            {
                foreach (IFormFile file in otherImgs)
                {
                    result = file?.CheckValidate("image/", 300);
                    if(result?.Length > 0)
                    {
                        ModelState.AddModelError("OtherImages", "Something wrong");
                    }
                }
            }

            if(prod.CategoryIds is not null)
            {
                foreach (int CatId in prod.CategoryIds)
                {
                    if(!_context.Categories.Any(c=> c.Id == CatId))
                    {
                        ModelState.AddModelError("CategryIds", "Categories are not entered correctly");
                        break;
                    }
                }
            }



            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                return View();
            }


            var categories = _context.Categories.Where(c => prod.CategoryIds.Contains(c.Id));

            Product newProduct = new Product()
            {
                Name = prod.Name,
                SellPrice = prod.SellPrice,
                CostPrice = prod.CostPrice,
                Discount = prod.Discount,
                Desc = prod.Desc,
                IsDeleted = false,
                Date = DateTime.Now,
                SKU = Guid.NewGuid().ToString(),

            };

            List<ProductImage> images = new List<ProductImage>();
            images.Add(new ProductImage { ImgUrl = coverImg?.SaveFile(Path.Combine(_env.WebRootPath, "assets", "img", "product")), IsCover = true, Product = newProduct });

            if(otherImgs is not null)
            {
                foreach (IFormFile item in otherImgs)
                {
                    images.Add(new ProductImage { ImgUrl = item?.SaveFile(Path.Combine(_env.WebRootPath, "assets", "img", "product")), IsCover = false, Product = newProduct });
                }
            }

            newProduct.ProductImages = images;
            _context.Products.Add(newProduct);
            foreach (var item in categories)
            {
                _context.ProductCategories.Add(new ProductCategory
                {
                    Product = newProduct,
                    CategoryId = item.Id
                });
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Update(int? Id) 
        {
            if (Id is null || Id <= 0) return BadRequest();

            Product product = _context.Products.Find(Id);
            if (product is null) return NotFound();
            ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));

            UpdateProductVM pro = new UpdateProductVM
            {
                Name = product.Name,
                SellPrice = product.SellPrice,
                CostPrice = product.CostPrice,
                Discount = product.Discount,
                Desc = product.Desc
               
            };


            return View(pro);
        }


        [HttpPost]
        public IActionResult Update(int? Id,UpdateProductVM prod)
        {
            if (Id is null || Id <= 0) return BadRequest();


            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                return View();
            }


            Product exist = _context.Products.Find(Id);
            if (exist is null) return NotFound();


            var categories = _context.Categories.Where(c => prod.CategoryIds.Contains(c.Id));

            exist.Name = prod.Name;
            exist.CostPrice = prod.CostPrice;
            exist.SellPrice = prod.SellPrice;
            exist.Desc = prod.Desc;
            

            foreach (var item in _context.ProductCategories.Where(pc => pc.ProductId == exist.Id))
            {
                _context.ProductCategories.Remove(item);
            }
           
            foreach (var item in categories)
            {
                _context.ProductCategories.Add(new ProductCategory
                {
                    Product = exist,
                    CategoryId = item.Id
                });
            }
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }




        public IActionResult ChangeStatus(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Product pro = _context.Products.Find(id);
            if (pro == null) return NotFound();

            if (pro.IsDeleted) pro.IsDeleted = false;
            else pro.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
