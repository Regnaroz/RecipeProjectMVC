using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeBlogProject.Common;
using RecipeBlogProject.Models;
using RecipeProject.Models.DataModels;

namespace RecipeProject.Controllers.ChefControllers
{
    public class RecipesController : BaseController
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecipesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Recipes.Include(r => r.Chef).Where(X => X.ChefId == (int) Roles.Chef);
            return View(await modelContext.ToListAsync());
        }

        
        // GET: Recipes/Create
        public async Task<IActionResult> Create()
        {
            var model = new CreateRecepieModel();
            model.Recipe = new Recipe();
            model.CategoryList = await _context.Categories.ToListAsync();
            ViewData["ChefId"] = new SelectList(_context.Chefs, "id", "FirstName");
            return View(model);
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecepieModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Recipe.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + model.Recipe.ImageFile.FileName;
                    string fullPath = Path.Combine(wwwRootPath + "/Images/recipes-images/", imageName);
                    using (var filestream = new FileStream(fullPath, FileMode.Create))
                    {
                        await model.Recipe.ImageFile.CopyToAsync(filestream);
                    }
                    model.Recipe.Recipecategories.Add(new() { CategoryId = model.SelectedCategoryId });
                    model.Recipe.ImagePath = imageName;
                }

                model.Recipe.ChefId = HttpContext.Session.GetInt32("ChefId");
                _context.Add(model.Recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyRecipes", "Chef");
            }
            ViewData["ChefId"] = new SelectList(_context.Chefs, "id", "id", model.Recipe.ChefId);
            return View(model);
        }

        private bool RecipeExists(int id)
        {
          return (_context.Recipes?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
