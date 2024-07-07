using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBlogProject.Common;
using RecipeBlogProject.Models;

namespace RecipeBlogProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly ModelContext _context;

        public LoginController(ModelContext context)
        {
            _context = context;
        }

        public IActionResult Login_Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var personInformation = await _context.Persons.
             Where(X => X.Email == email && X.Password == password)
             .FirstOrDefaultAsync();

            if (personInformation != null) {

               if(personInformation.ImagePath != null)
                {
                    HttpContext.Session.SetString("ImagePath", personInformation.ImagePath);
                }
                HttpContext.Session.SetInt32("PersonId", (int) personInformation.id);
                HttpContext.Session.SetInt32("RoleId", personInformation.RoleId??0);
                HttpContext.Session.SetString("Name", personInformation.Firstname + personInformation.Lastname);
               
                TempData.Keep();
                switch (personInformation.RoleId) {

                    case (int) Roles.Administrator:
                        var adminId = (await _context.Admins.FirstOrDefaultAsync(p => p.PersonId == personInformation.id)).id;
                        HttpContext.Session.SetInt32("AdminId", adminId);
                        return RedirectToAction("Index", "Admin");

                    case (int) Roles.Chef:
                        var chefId = (await _context.Chefs.FirstOrDefaultAsync(p => p.PersonId == personInformation.id)).id;
                        HttpContext.Session.SetInt32("ChefId", chefId);
                        return RedirectToAction("Index", "Chef");

                    case (int) Roles.RegisteredUser:
                        var userId = (await _context.Systemusers.FirstOrDefaultAsync(p => p.PersonId == personInformation.id)).id;
                        HttpContext.Session.SetInt32("UserId", userId);
                        return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Username or Password are incorrect.");
            }

            return View();
        }

        public IActionResult Logout() 
        { 
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}
