using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RecipeBlogProject.Models;

namespace RecipeProject.Controllers.ProfileController
{
    public class DashboardController : BaseController
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DashboardController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
