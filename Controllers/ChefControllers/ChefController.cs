using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBlogProject.Models;
using RecipeProject.Controllers;

namespace RecipeBlogProject.Controllers.ChefControllers
{


    public class ChefController : BaseController
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ChefController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("ChefID");
            var chef = await _context.Systemusers.Where(X => X.id == id).SingleOrDefaultAsync();

            return View(chef);
        }

        public IActionResult RecipeCategories()
        {
            ViewBag.categories = _context.Categories.ToList();

            return View();
        }

        public async Task<IActionResult> MyRecipes() {

            var recipes = await _context.Recipes.ToListAsync();

            return View(recipes);
        }

        public IActionResult GetChefProfilePage() 
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login_Index", "Login");
        }
        public async Task<IActionResult> DashboardProfile()
        {
            var personId = HttpContext.Session.GetInt32("PersonId");
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile ProfileImage)
        {
            var personId = HttpContext.Session.GetInt32("PersonId");
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "profile-images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(fileStream);
                }

                // Assuming you have a method to get the current user
                var person = await _context.Persons.FindAsync(personId);
                person.ImagePath = uniqueFileName;
                // Save changes to the database
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("ImagePath", person.ImagePath);
            }

            return RedirectToAction(nameof(DashboardProfile));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Person model)
        {
            if (ModelState.IsValid)
            {

                var person = await _context.Persons.FindAsync(model.id);
                person.Firstname = model.Firstname;
                person.Lastname = model.Lastname;
                person.Gender = model.Gender;
                person.Phone = model.Phone;
                person.Email = model.Email;
                person.Password = model.Password;
                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(DashboardProfile));
            }

            return View(model);
        }
    }
}
