using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RecipeBlogProject.Common;
using RecipeBlogProject.Models;
using RecipeProject.Common;
using RecipeProject.Controllers;
using RecipeProject.Models.DataModels;

namespace RecipeBlogProject.Controllers.AdminControllers
{
    public class AdminController : BaseController
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("AdminId");
            TempData["RecipesCount"] = await _context.Recipes.CountAsync();
            TempData["RecipesRequestCount"] = await _context.Recipes.Where(c => !c.Isapproved).CountAsync();
            TempData["RecipesSalesCount"] = await _context.Recipepayments.CountAsync();
            TempData["RecipesSalesAmount"] = await _context.Recipepayments.SumAsync(c => c.Totalamount);
            TempData["ChefCount"] = await _context.Chefs.CountAsync();
            TempData["UsersCount"] = await _context.Systemusers.CountAsync();
            ViewBag.EarningsJson = await GetAreaChartData();
            ViewBag.CategorySales = await GetSalesByCategoryData();
            if (id == null)
            {
                return RedirectToAction("Login_Index", "Login");
            }
            var personInformation = await _context.Persons.FirstOrDefaultAsync(c => c.id == id);
            TempData["Name"] = personInformation.Firstname + " " + personInformation.Lastname;
            TempData["Email"] = personInformation.Email;
            TempData["ImagePath"] = personInformation.ImagePath;
            var admin = await _context.Admins.Where(X => X.id == id).SingleOrDefaultAsync();

            return View(admin);
        }

        private async Task<List<SalesByCategory>> GetSalesByCategoryData()
        {
            var result = new List<SalesByCategory>();
            var sales = await _context.Recipepayments.ToListAsync();
            var totalSales = await _context.Recipepayments.SumAsync(c=>c.Totalamount);
            var groupedPayments = sales
                    .Where(p => p.Totalamount.HasValue)
                    .GroupBy(p => p.Recipe.Recipecategories.FirstOrDefault().Category.Categoryname)
                    .Select(g => new
                    {
                        Month = g.Key,
                        TotalAmount = g.Sum(p => p.Totalamount.Value)
                    });
            foreach (var item in groupedPayments)
            {
                result.Add(new()
                {
                    CategoryName = item.Month,
                    Value = Math.Round((item.TotalAmount / totalSales.Value) * 100, 1)

                });
            }
            return result;
        }

        private async Task<string> GetAreaChartData()
            {// Fetch payments from the database
                var payments = await _context.Recipepayments.ToListAsync();

                // Initialize an array to hold earnings for each month
                var monthlyEarnings = new double[12];

                // Group payments by month and calculate the total amount for each month
                var groupedPayments = payments
                    .Where(p => p.Totalamount.HasValue)
                    .GroupBy(p => p.CreatedDate.Value.Month)
                    .Select(g => new
                    {
                        Month = g.Key,
                        TotalAmount = g.Sum(p => p.Totalamount.Value)
                    });

                // Fill the monthlyEarnings array with calculated values
                foreach (var item in groupedPayments)
                {
                    monthlyEarnings[item.Month - 1] = item.TotalAmount;
                }

                // Pass the earnings data to the view using ViewBag
                return JsonConvert.SerializeObject(monthlyEarnings);
            }
        

        public IActionResult GetAllUsers()
        {
            return RedirectToAction("Index", "AdminAllUsers");
        }


        public async Task<IActionResult> GetChefs()
        {
            var chefs = await _context.Chefs.Include(c=>c.Person).ToListAsync();
            return View(chefs);
        }
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories.IgnoreQueryFilters().ToListAsync();
            return View(categories);
        }
        public async Task<IActionResult> CategoryEdit(int id)
        {
            var category = await _context.Categories.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.id ==id);
            return View(category);
        }
        public async Task<IActionResult> CategoryDetails(int id)
        {
            var category = await _context.Categories.IgnoreQueryFilters().Where(c => c.id == id)
                                  .Select(c=> new CategoryRecepies()
                                  {
                                      Category = c,
                                      Recipes =  c.Recipecategories.Select(c=>c.Recipe).ToList()
                                  })
                                  .FirstOrDefaultAsync();
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category, IFormFile CategoryImage)
        {
            var categoryData = await _context.Categories.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.id == category.id);
            if (categoryData == null)
            {
                return NotFound();
            }

            categoryData.Categoryname = category.Categoryname;
            categoryData.IsDeleted = category.IsDeleted;

            if (CategoryImage != null && CategoryImage.Length > 0)
            {
                // Get the root path of the web project
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // Create a unique file name using GUID and the category name
                string imageName = Guid.NewGuid().ToString() + "_" + category.Categoryname + Path.GetExtension(CategoryImage.FileName);

                // Combine the root path with the desired directory and file name
                string imageDirectory = Path.Combine(wwwRootPath, "categories-images");
                string fullPath = Path.Combine(imageDirectory, imageName);

                // Ensure the directory exists
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                // Save the file
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await CategoryImage.CopyToAsync(stream);
                }

                // Optionally, delete the old image if necessary
                if (!string.IsNullOrEmpty(categoryData.CategoryImagePath))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, categoryData.CategoryImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Store the relative path in the category object
                categoryData.CategoryImagePath = Path.Combine("categories-images", imageName);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // Handle the exception appropriately
                throw;
            }

            return RedirectToAction(nameof(Categories));
        }

        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category, IFormFile CategoryImage)
        {
            if (CategoryImage != null && CategoryImage.Length > 0)
            {
                // Get the root path of the web project
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // Create a unique file name using GUID and the category name
                string imageName = Guid.NewGuid().ToString() + "_" + category.Categoryname + Path.GetExtension(CategoryImage.FileName);

                // Combine the root path with the desired directory and file name
                string imageDirectory = Path.Combine(wwwRootPath, "categories-images");
                string fullPath = Path.Combine(imageDirectory, imageName);

                // Ensure the directory exists
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                // Save the file
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await CategoryImage.CopyToAsync(stream);
                }

                // Store the relative path in the category object
                category.CategoryImagePath = Path.Combine("categories-images", imageName);
            }

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // Handle the exception appropriately
                throw;
            }

            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> CategoryDelete(int id)
        {
            await _context.Categories.IgnoreQueryFilters().Where(c => c.id == id).ExecuteUpdateAsync(c => c.SetProperty(c => c.IsDeleted , true));
            return RedirectToAction(nameof(Categories));
        }
        public async Task<IActionResult> CategoryUnDelete(int id)
        {
            await _context.Categories.IgnoreQueryFilters().Where(c => c.id == id).ExecuteUpdateAsync(c => c.SetProperty(c => c.IsDeleted, false));
            return RedirectToAction(nameof(Categories));
        }
        public IActionResult GetAdminCategories()
        {
            return RedirectToAction("Index", "AdminCategories");
        }

        public async Task<IActionResult> AdminRecipes()
        {
            var recepies = await _context.Recipes.IgnoreQueryFilters().ToListAsync();
            return View(recepies);
        }
        
        public async Task<IActionResult> AdminEditRecipes(int? id)
        {
            var recepies = await _context.Recipes.FindAsync(id);
            return View(recepies);
        }

        [HttpPost]
        public async Task<IActionResult> AdminEditRecipePost(Recipe recipe)
        {
            var recepieData = await _context.Recipes.FindAsync(recipe.id);
            recepieData.Ingredients = recipe.Ingredients;
            recepieData.Receipename = recipe.Receipename;
            recepieData.Price = recipe.Price;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminRecipes));
        }
        public async Task<IActionResult> AdminRecipeDetails(int? id)
        {
            var recepies = await _context.Recipes.FindAsync(id);
            return View(recepies);
        }

        public async Task<IActionResult> AdminChefRecepies()
        {
            var recepies = await _context.Recipes.Where(r=> !r.Isapproved).ToListAsync();
            return View(recepies);
        }
        public async Task<IActionResult> AdminUserRecepieRequest()
        {
            var recepies = await _context.Recipepayments.ToListAsync();
            return View(recepies);
        }
        [HttpPost]
        public async Task<IActionResult> AdminUserRecepieRequest(DateTime? from, DateTime? to)
        {
            var recepies = await _context.Recipepayments.IgnoreQueryFilters()
                .Where(r => 
                        (from.HasValue? from.Value <= r.CreatedDate : true)
                        && (to.HasValue ? to.Value >= r.CreatedDate : true)
                        )
                .ToListAsync();

            if (from.HasValue || to.HasValue)
            {
                TempData["From"] = from.HasValue ? from.Value.ToString("yyyy-MM-dd") : null;
                TempData["To"] = to.HasValue ? to.Value.ToString("yyyy-MM-dd") : null;
            }
            else
            {
                TempData["From"] = null;
                TempData["To"] = null;
            }
            return View(recepies);
        }
        public async Task<IActionResult> UnApproveRecepie(int? id)
        {
            var recepie = await _context.Recipes.FindAsync(id);
            if (recepie is not null)
            {
                recepie.Isapproved = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminRecipes));
        }
        public async Task<IActionResult> ApproveRecepie(int? id)
        {
            var recepie = await _context.Recipes.FindAsync(id);
            if(recepie is not null)
            {
                recepie.Isapproved = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminRecipes));
        }
        public async Task<IActionResult> DisableRecepie(int? id)
        {
            var recepie = await _context.Recipes.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.id == id);
            if (recepie is not null)
            {
                recepie.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminRecipes));
        }
        public async Task<IActionResult> DeleteChef(int? id)
        {
            var chef = await _context.Chefs.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.id == id);
            chef.IsDeleted = true;
            var person = await _context.Persons.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.id == chef.PersonId);
            person.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetChefs));
        }
        public async Task<IActionResult> EnableChef(int? id)
        {
            var chef = await _context.Chefs.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.id == id);
            chef.IsDeleted = false;
            var person = await _context.Persons.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.id == chef.PersonId);
            person.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetChefs));
        }
        public async Task<IActionResult> EnableRecepie(int? id)
        {
            var recepie = await _context.Recipes.IgnoreQueryFilters().FirstOrDefaultAsync(x=>x.id ==id);
            if (recepie is not null)
            {
                recepie.IsDeleted = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminRecipes));
        }
        public async Task<IActionResult> AcceptNewChef(int? id)
        {
            var chef = await _context.Chefs.Include(c=>c.Person).IgnoreQueryFilters().FirstOrDefaultAsync(c=>c.id ==id);
            chef.IsDeleted = false;
            chef.Person.IsDeleted = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ChefsRegisteration));
        }
        public async Task<IActionResult> AdminChefDetails(int? id)
        {
            var chef = await _context.Chefs.Include(c=>c.Person).IgnoreQueryFilters().FirstOrDefaultAsync(x=>x.id == id);
            return View(chef);
        }


        public IActionResult GetCharts()
        {
            return View();
        }

        public IActionResult GetAdminProfilePage() 
        { 
            return View();
        }
        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChef([Bind("Firstname,Lastname,Gender,Email,Phone,Password,CreatedDate,ModifiedDate,CreatedBy,ModifiedBy,IsDeleted,id")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.RoleId = (int)Roles.Chef;
                _context.Add(person);
                await _context.SaveChangesAsync();
                var personData = await _context.Persons.FirstAsync(c => c.Email == person.Email);
                var chef = new Chef()
                {
                    PersonId = personData.id
                };
                await _context.Chefs.AddAsync(chef);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetChefs));
            }
            return View(person);
        }

        public async Task<IActionResult> ChefsRegisteration()
        {
            var newChefs = await _context.Chefs.Include(c => c.Person).Where(c=>c.IsDeleted == true).IgnoreQueryFilters().ToListAsync();
            return View(newChefs);
        }
        public IActionResult Logout()
        {
           var t = _context.Recipepayments.Where(c => c.UserId == 1).Select(c => c.Recipe);
            HttpContext.Session.Clear();
            return RedirectToAction("Login_Index", "Login");
        }

        public async Task<IActionResult> AdminTestmonials()
        {
            var result = await _context.Testimonials.ToListAsync();
            return View(result);
        }
        public async Task<IActionResult> AdminHomePageDetails()
        {
            var data = await _context.Websitedetails.ToListAsync();
            var result = new WebisteDetailsModel();

            var homeData = data.FirstOrDefault(c => c.Texttype == (int)WebsiteContent.HomePageMainParagraph);
            result.HomePageHeader = homeData.Websitetext;
            var footer = data.FirstOrDefault(c => c.Texttype == (int)WebsiteContent.FooterContent);
            result.HomePageFooter = footer.Websitetext;
            var contact = data.FirstOrDefault(c => c.Texttype == (int)WebsiteContent.ContactMessage);
            result.HomePageContactUs = contact.Websitetext;
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> AdminHomePageDetails(WebisteDetailsModel model)
        {
            var result = await _context.Websitedetails.ToListAsync();
            var homeData = await _context.Websitedetails.FirstOrDefaultAsync(c => c.Texttype == (int)WebsiteContent.HomePageMainParagraph);
            homeData.Websitetext = model.HomePageHeader;
            var contactData = await _context.Websitedetails.FirstOrDefaultAsync(c => c.Texttype == (int)WebsiteContent.ContactMessage);
            contactData.Websitetext = model.HomePageContactUs;
            var footerData = await _context.Websitedetails.FirstOrDefaultAsync(c => c.Texttype == (int)WebsiteContent.FooterContent);
            footerData.Websitetext = model.HomePageFooter;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminHomePageDetails));
        }

        public async Task<IActionResult> HideTestmonial(int id)
        {
            var result = await _context.Testimonials.FindAsync(id);
            result.IsShown = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminTestmonials));
        }
        public async Task<IActionResult> ShowTestmonial(int id)
        {
            var result = await _context.Testimonials.FindAsync(id);
            result.IsShown = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminTestmonials));
        }

    }
}
