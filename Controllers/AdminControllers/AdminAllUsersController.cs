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
    public class AdminAllUsersController : BaseController
    {
        private readonly ModelContext _context;

        public AdminAllUsersController(ModelContext context)
        {
            _context = context;
        }

        // GET: AdminUsers
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Persons.Include(p => p.Role).IgnoreQueryFilters().Where(user => user.RoleId == (int) Roles.RegisteredUser);
            return View(await modelContext.ToListAsync());
        }

        // GET: AdminUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Systemusers
                .Include(c=>c.Person)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: AdminUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Userroles, "id", "RoleName", person.RoleId);
            return View(person);
        }

        // POST: AdminUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Firstname,Lastname,Username,Password,Gender,Email,Phone,RoleId,CreatedDate,ModifiedDate,CreatedBy,ModifiedBy,IsDeleted,id")] Person person)
        {
            if (id != person.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Userroles, "id", "id", person.RoleId);
            return View(person);
        }

        // GET: AdminUsers/Delete/5
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

        // POST: AdminUsers/Delete/5
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

        public async Task<IActionResult> DeletePerson (int? id)
        {
            var person = await _context.Persons.IgnoreQueryFilters().FirstOrDefaultAsync(u=>u.id == id);
            person.IsDeleted = true;
            var user = await _context.Systemusers.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.PersonId == id);
            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> EnablePerson(int? id)
        {
            var person = await _context.Persons.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.id == id);
            person.IsDeleted = false;
            var user = await _context.Systemusers.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.PersonId == id);
            user.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
