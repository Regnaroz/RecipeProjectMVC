using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBlogProject.Models;
using Rotativa.AspNetCore;

namespace RecipeProject.Controllers
{
    public class FilesController : BaseController
    {
        private readonly ModelContext _context;
        public FilesController(ModelContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GeneratePDF()
        {
            // Sample model data, replace with actual data retrieval logic
            var model = await _context.Recipepayments.FirstOrDefaultAsync();
            // Pass the model to the view
            var pdf = new ViewAsPdf("Receipt", model)
            {
                FileName = "Receipt.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,

            };
            return pdf;
        }
        public async Task<IActionResult> GenerateUserOrderPDFAndSaveToOrders()
        {
            // Sample model data, replace with actual data retrieval logic
            var model = await _context.Recipepayments.FirstOrDefaultAsync();
            // Pass the model to the view
            var pdf = new ViewAsPdf("Receipt", model)
            {
                FileName = "Receipt.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,

            };
            // Get the byte array of the generated PDF
            var pdfData = await pdf.BuildFile(ControllerContext);

            // Define the path where the PDF will be saved
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Orders", "Receipt2.pdf");

            // Ensure the Orders directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Save the PDF file
            await System.IO.File.WriteAllBytesAsync(filePath, pdfData);
            return pdf;
        }
        // View action to display the HTML view
        public IActionResult Receipt()
        {
            // Sample model data, replace with actual data retrieval logic
            var model = new Recipepayment
            {
                // Initialize model properties here
            };

            return View(model);
        }
    }
}
