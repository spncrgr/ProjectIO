using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectIO.Data;
using ProjectIO.Models;
using ProjectIO.Models.ViewModels;

namespace ProjectIO.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.Include(project => project.Customer).ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            var vm = new ProjectsViewModel
            {
                Customers = await _context.Customers
                    .Select(customer => new SelectListItem {Text = customer.Name, Value = customer.Id.ToString()})
                    .ToListAsync()
            };
            return View(vm);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectsViewModel project)
        {
            if (!ModelState.IsValid || !int.TryParse(project.SelectedCustomerId, out var selectedCustomerId))
                return View(project);

            var newProject = new Project
            {
                Name = project.Name,
                Description = project.Description,
                Customer = await _context.Customers.FindAsync(selectedCustomerId)
            };
            _context.Add(newProject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            var vm = new ProjectsViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Customers = await _context.Customers
                    .Select(customer => new SelectListItem {Text = customer.Name, Value = customer.Id.ToString()})
                    .ToListAsync(),
                SelectedCustomerId = project.Customer.Id.ToString()
            };

            return View(vm);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,SelectedCustomerId")]
            ProjectsViewModel pvm)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid && int.TryParse(pvm.SelectedCustomerId, out var selectedCustomerId))
            {
                try
                {
                    project.Name = pvm.Name;
                    project.Description = pvm.Description;
                    project.Customer = await _context.Customers.FindAsync(selectedCustomerId);

                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(pvm.Id))
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

            return View(pvm);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}