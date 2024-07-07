using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using RecipeBlogProject.Models;

namespace RecipeProject.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var name = HttpContext.Session.GetString("Name");
            var email = HttpContext.Session.GetString("Email");
            var imagePath = HttpContext.Session.GetString("ImagePath");
            var UserId = HttpContext.Session.GetInt32("UserId");
            var PersonId = HttpContext.Session.GetInt32("PersonId");
            var ChefId = HttpContext.Session.GetInt32("ChefId");
            if (!string.IsNullOrEmpty(name))
            {
                TempData["Name"] = name;
            }
            if (!string.IsNullOrEmpty(email))
            {
                TempData["Email"] = email;
            }
            if (!string.IsNullOrEmpty(imagePath))
            {
                TempData["ImagePath"] = imagePath;
            }
            if (UserId is not null)
            {
                TempData["UserId"] = UserId;
            }
            if (ChefId is not null)
            {
                TempData["ChefId"] = ChefId;
            }
            if (PersonId is not null)
            {
                TempData["PersonId"] = PersonId;
            }
        }
    }
}
