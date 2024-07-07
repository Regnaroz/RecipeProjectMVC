using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeBlogProject.Common;
using RecipeBlogProject.Models;

namespace RecipeProject.Controllers.AdminControllers
{
    public class AdminAllChefsController : BaseController
    {
        private readonly ModelContext _context;

        public AdminAllChefsController(ModelContext context)
        {
            _context = context;
        }

        // GET: AdminAllChefs
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Persons.Include(p => p.Role).Where(user => user.RoleId == (int) Roles.Chef);
            return View(await modelContext.ToListAsync());
        }

        // GET: AdminAllChefs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .Include(p => p.Role)
                .FirstOrDefaultAsync(m => m.id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: AdminAllChefs/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Userroles, "id", "id");
            return View();
        }

        // POST: AdminAllChefs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Firstname,Lastname,Password,Gender,Email,Phone,RoleId,CreatedDate,ModifiedDate,CreatedBy,ModifiedBy,IsDeleted,id")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Userroles, "id", "RoleName", person.RoleId);
            return View(person);
        }


        // GET: AdminAllChefs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .Include(p => p.Role)
                .FirstOrDefaultAsync(m => m.id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: AdminAllChefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Persons == null)
            {
                return Problem("Entity set 'ModelContext.Persons'  is null.");
            }
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
          return (_context.Persons?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
