using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using RecipeBlogProject.Models;
using RecipeProject.Common;
using RecipeProject.Controllers;
using RecipeProject.EmailService;
using RecipeProject.EmailService.Emails;
using RecipeProject.Models.DataModels;
using Rotativa.AspNetCore;
using System;
using System.Diagnostics;
using System.Net;

namespace TestProject.Controllers.HomeControllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(ILogger<HomeController> logger, ModelContext context, IMailService mailService, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _mailService = mailService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var personId = HttpContext.Session.GetInt32("PersonId");
            TempData["PersonId"] = personId;
            var model = new WebisteDetailsModel();
            ViewData["HomeMainText"] = (await _context.Websitedetails.FirstOrDefaultAsync(c => c.Texttype == (int)WebsiteContent.HomePageMainParagraph))?.Websitetext;
            ViewData["HomePageFooter"]  = (await _context.Websitedetails.FirstOrDefaultAsync(c => c.Texttype == (int)WebsiteContent.FooterContent))?.Websitetext;
            ViewData["HomePageContactUs"] = (await _context.Websitedetails.FirstOrDefaultAsync(c => c.Texttype == (int)WebsiteContent.ContactMessage))?.Websitetext;
            model.Testimonials = await _context.Testimonials.Where(c=>c.IsShown).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> GetRecipeCategories()
        {
            ViewBag.categories = await _context.Categories.ToListAsync();

            return View();
        }

        public async Task<IActionResult> GetAllRecipes()
        {
             var recipes = await _context.Recipes.ToListAsync();

            return View(recipes);
        }
        [HttpPost]
        public async Task<IActionResult> GetAllRecipes(string name)
        {
            var recipes = await _context.Recipes
                                    .Where(r =>
                                           (name == null || r.Receipename.ToLower().Contains(name.ToLower())))
                                    .ToListAsync();
            ViewData["SearchData"] = name;
            return View(recipes);
        }



        public async Task<IActionResult> GetChefInfo()
        {
            ViewBag.chefs = await _context.Chefs.ToListAsync();

            return View();
        }

        public async Task<IActionResult> GetChefRecipes(int id)
        {
            var chefRecipes = await _context.Recipes.Where(X => X.ChefId == id).ToListAsync();

            return View(chefRecipes);
        }

        public async Task<IActionResult> GetRecipesByCategory(int Id)
        {
            var recipesInCategory = await _context.Recipecategories.Where(X => X.CategoryId == Id)
                .Select(Y => Y.Recipe).ToListAsync();
                          
            return View(recipesInCategory);
        }


        public async Task<IActionResult> CheckUserCardInfo(int? id)
        {
            TempData["RecipeId"]=id;
            var userId = HttpContext.Session.GetInt32("UserId");
            if(userId is null)
            {
                return RedirectToAction("Login_Index", "Login");
            }
            var checkVisaCardInfo = await _context.Visacards.
                Where(X => X.UserId == userId).FirstOrDefaultAsync();

            if (checkVisaCardInfo == null)
                return RedirectToAction("UserVisaCardInformation",new { recipeId = id});
            else
                return RedirectToAction("CreateVisaCard", new { recipeId = id });
           
        }

        public async Task<IActionResult> CreateVisaCard(int recipeId)
        {
            var result = new BuyRecipieModel();
            if (recipeId is 0  && TempData["RecipeId"] is not null)
            {
                recipeId = (int)TempData["RecipeId"];
            }
            result.Recipe = await _context.Recipes.FirstOrDefaultAsync(c => c.id == recipeId);
            //TempData["RecipeId"] = result.Recipe.id;
            result.Visacard = await _context.Visacards.FirstOrDefaultAsync(c => c.UserId == HttpContext.Session.GetInt32("UserId"));
            result.User = await _context.Systemusers.FindAsync(HttpContext.Session.GetInt32("UserId"));
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisaCard(Visacard visacard)
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var checkVisaCardInfo =  await _context.Visacards.
                Where(X => X.Cardnumber == visacard.Cardnumber).FirstOrDefaultAsync();

            if (checkVisaCardInfo == null)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(visacard);
                    await _context.SaveChangesAsync();
                }
            }
            


            return RedirectToAction("UserVisaInformation");
        }
        public async Task<IActionResult> UserVisaCardInformation(int recipeId)
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var visa = await _context.Visacards.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == id);
            return View(visa);
        }
            public  async Task<IActionResult> UserVisaInformation(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var result = new BuyRecipieModel();
            var data = await _context.Visacards.FirstOrDefaultAsync(c=>c.UserId == userId);
            var recipe = await _context.Recipes.FindAsync(id);
            result.Recipe = recipe;
            if(data == null)
            {
                result.User = await _context.Systemusers.FindAsync(userId);
                return View(result);

            }
            return RedirectToAction("CreateVisaCard", new { recipeId = id });
        }

        [HttpPost]
        public async Task<IActionResult> UserVisaInformation(Visacard visacard)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var card = await _context.Visacards.Where(c => c.UserId == userId).FirstOrDefaultAsync();
            var isNew = false;
            if(card is null )
            {
                isNew = true;
                card = new Visacard();
                card.UserId = userId;
            }

            card.Firstname = visacard.Firstname;
            card.Lastname = visacard.Lastname;
            card.Cardnumber = visacard.Cardnumber;
            card.Pin = visacard.Pin;
            card.Cvv = visacard.Cvv;
            card.Expirydate = visacard.Expirydate;
            if (isNew)
            {
                _context.Visacards.Add(card);
            }
            await _context.SaveChangesAsync();
            if (TempData["RecipeId"] is not null)
            {
                return RedirectToAction(nameof(CreateVisaCard), new { recipeId = TempData["RecipeId"] });
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PurchasedRecipes()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var paymentDetails = await _context.Recipepayments.Where(X => X.UserId == userId ).ToListAsync();

          return View(paymentDetails);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> BuyRecipe (int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            var userId =  HttpContext.Session.GetInt32("UserId");
            var user =  await _context.Systemusers.FindAsync(userId);
            var recipePayment = new Recipepayment();
            recipePayment.UserId = userId;
            recipePayment.RecipeId = id;
            recipePayment.Totalamount = recipe.Price;
            recipePayment.Paymentfilepath = await SaveUserPaymentFile(recipe, user);
           
            _context.Recipepayments.Add(recipePayment);
            await _context.SaveChangesAsync();
            string imageSource = "https://icon-library.com/images/recipe-icon-png/recipe-icon-png-7.jpg";
            byte[] imageBytes;
            using (var client = new WebClient())
            {
                imageBytes = client.DownloadData(imageSource);
            }
            string base64Image = Convert.ToBase64String(imageBytes);
            string embeddedImage = $"data:image/jpeg;base64,{base64Image}";
            var emailRequest = new MailRequest()
            {
                ToEmail = user.Person.Email,
                Subject = "Order Details",
                Attachments = GetPdfAsFormFile(recipePayment.Paymentfilepath),
                Body = $"\r\n    <!-- start preheader -->\r\n    <div class=\"preheader\" style=\"display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;\">\r\n        A preheader is the short summary text that follows the subject line when an email is viewed in the inbox.\r\n    </div>\r\n    <!-- end preheader -->\r\n    <!-- start body -->\r\n    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n\r\n        <!-- start logo -->\r\n        <tr>\r\n            <td align=\"center\" bgcolor=\"#D2C7BA\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td align=\"center\" valign=\"top\" style=\"padding: 36px 24px;\">\r\n                              \r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n                <!--[if (gte mso 9)|(IE)]>\r\n                </td>\r\n                </tr>\r\n                </table>\r\n                <![endif]-->\r\n            </td>\r\n        </tr>\r\n        <!-- end logo -->\r\n        <!-- start hero -->\r\n        <tr>\r\n            <td align=\"center\" bgcolor=\"#D2C7BA\">\r\n                <!--[if (gte mso 9)|(IE)]>\r\n                <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n                <tr>\r\n                <td align=\"center\" valign=\"top\" width=\"600\">\r\n                <![endif]-->\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;\">\r\n                            <h1 style=\"margin: 0; font-size: 32px; font-weight: 700; line-height: 48px;\">Thank you for your order!</h1>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n                <!--[if (gte mso 9)|(IE)]>\r\n                </td>\r\n                </tr>\r\n                </table>\r\n                <![endif]-->\r\n            </td>\r\n        </tr>\r\n        <!-- end hero -->\r\n        <!-- start copy block -->\r\n        <tr>\r\n            <td align=\"center\" bgcolor=\"#D2C7BA\">\r\n                <!--[if (gte mso 9)|(IE)]>\r\n                <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n                <tr>\r\n                <td align=\"center\" valign=\"top\" width=\"600\">\r\n                <![endif]-->\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n\r\n                    <!-- start copy -->\r\n                    <tr>\r\n                        <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">\r\n                            <p style=\"margin: 0;\">Here is a summary of your recent order.</p>\r\n                        </td>\r\n                    </tr>\r\n                    <!-- end copy -->\r\n                    <!-- start receipt table -->\r\n                    <tr>\r\n                        <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">\r\n                            <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                <tr>\r\n                                    <td align=\"left\" bgcolor=\"#D2C7BA\" width=\"75%\" style=\"padding: 12px;font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\"><strong>Order #</strong></td>\r\n                                    <td align=\"left\" bgcolor=\"#D2C7BA\" width=\"25%\" style=\"padding: 12px;font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\"><strong>[Id]</strong></td>\r\n                                </tr>\r\n                                <tr>\r\n                                    <td align=\"left\" width=\"75%\" style=\"padding: 6px 12px;font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">Recepie Name</td>\r\n                                    <td align=\"left\" width=\"25%\" style=\"padding: 6px 12px;font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">[Receipename]</td>\r\n                                </tr>\r\n                            <tr>\r\n                                <td align=\"left\" width=\"75%\" style=\"padding: 6px 12px;font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">Recepie Price</td>\r\n                                <td align=\"left\" width=\"25%\" style=\"padding: 6px 12px;font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">[Price]</td>\r\n                            </tr>\r\n                                <tr>\r\n                                    <td align=\"left\" width=\"75%\" style=\"padding: 12px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-top: 2px dashed #D2C7BA; border-bottom: 2px dashed #D2C7BA;\"><strong>Total</strong></td>\r\n                                    <td align=\"left\" width=\"25%\" style=\"padding: 12px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-top: 2px dashed #D2C7BA; border-bottom: 2px dashed #D2C7BA;\"><strong>[Total] $</strong></td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n                    <!-- end reeipt table -->\r\n\r\n                </table>\r\n                <!--[if (gte mso 9)|(IE)]>\r\n                </td>\r\n                </tr>\r\n                </table>\r\n                <![endif]-->\r\n            </td>\r\n        </tr>\r\n        <!-- end copy block -->\r\n        <!-- start receipt address block -->\r\n        <tr>\r\n            <td align=\"center\" bgcolor=\"#D2C7BA\" valign=\"top\" width=\"100%\">\r\n                <!--[if (gte mso 9)|(IE)]>\r\n                <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n                <tr>\r\n                <td align=\"center\" valign=\"top\" width=\"600\">\r\n                <![endif]-->\r\n         \r\n                <!--[if (gte mso 9)|(IE)]>\r\n                </td>\r\n                </tr>\r\n                </table>\r\n                <![endif]-->\r\n            </td>\r\n        </tr>\r\n        <!-- end receipt address block -->\r\n        <!-- start footer -->\r\n        <tr>\r\n            <td align=\"center\" bgcolor=\"#D2C7BA\" style=\"padding: 24px;\">\r\n                <!--[if (gte mso 9)|(IE)]>\r\n                <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n                <tr>\r\n                <td align=\"center\" valign=\"top\" width=\"600\">\r\n                <![endif]-->\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n\r\n                    <!-- start permission -->\r\n                    <tr>\r\n                        <td align=\"center\" bgcolor=\"#D2C7BA\" style=\"padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;\">\r\n                        <p style=\"margin: 0;\">If you have any questions or concerns about your order, please <a href=\"https://localhost:7265/\">contact us</a>.</p>\r\n                            <p style=\"margin: 0;\">You received this email because we received a request for [type_of_action] for your account. If you didn't request [type_of_action] you can safely delete this email.</p>\r\n                        </td>\r\n                    </tr>\r\n                    <!-- end permission -->\r\n                    <!-- start unsubscribe -->\r\n                  \r\n                    <!-- end unsubscribe -->\r\n\r\n                </table>\r\n                <!--[if (gte mso 9)|(IE)]>\r\n                </td>\r\n                </tr>\r\n                </table>\r\n                <![endif]-->\r\n            </td>\r\n        </tr>\r\n        <!-- end footer -->\r\n\r\n    </table>\r\n"
            };
            emailRequest.Body = emailRequest.Body.Replace("[Id]", recipePayment.id.ToString())
                                .Replace("[Receipename]", recipe.Receipename)
                                .Replace("[Price]", recipe.Price.ToString())
                                .Replace("[Total]", recipe.Price.ToString());
            await _mailService.SendEmailAsync(emailRequest);

            return RedirectToAction("Index", "Home");


        }

        private async Task<string> SaveUserPaymentFile(Recipe recipe, Systemuser? user)
        {
            var model = new Recipepayment() { RecipeId = recipe.id, Recipe = recipe, UserId = user.id, User = user, Totalamount = recipe.Price };
            var pdf = new ViewAsPdf("OrderReceipt", model)
            {
                FileName = "Receipt.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
            };
            var pdfData = await pdf.BuildFile(ControllerContext);
            var fileName = $"{user.id}-{recipe.id}-Receipt.pdf";

            var relativePath = Path.Combine("Orders", fileName);
            var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));

            await System.IO.File.WriteAllBytesAsync(absolutePath, pdfData);
            return relativePath;
        }

        private List<IFormFile> GetPdfAsFormFile(string relativePdfPath)
        {
            // Combine the relative path with the web root path to get the absolute path
            var absolutePdfPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePdfPath);

            // Step 1: Read the PDF from the file system
            byte[] pdfBytes = System.IO.File.ReadAllBytes(absolutePdfPath);

            // Step 2: Convert it to a stream and create an IFormFile object
            var memoryStream = new MemoryStream(pdfBytes);
            IFormFile pdfFormFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", Path.GetFileName(absolutePdfPath))
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            // Step 3: Add the IFormFile to a List<IFormFile>
            List<IFormFile> formFiles = new List<IFormFile> { pdfFormFile };

            return formFiles;
        }

        public IActionResult OrderReceipt()
        {
            // Sample model data, replace with actual data retrieval logic
            var model = new Recipepayment
            {
                // Initialize model properties here
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitTestimonial(TestimonialViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var userTestimonial = new Testimonial()
                {
                    UserId = userId,
                    Rating = model.Rating,
                    IsShown = false,
                    Usercomment = model.Message
                };
                _context.Testimonials.Add(userTestimonial);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }

}
