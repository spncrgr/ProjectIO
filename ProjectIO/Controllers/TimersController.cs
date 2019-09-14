using System;
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
    public class TimersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Timers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Timers.ToListAsync());
        }

        // GET: Timers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timer = await _context.Timers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timer == null)
            {
                return NotFound();
            }

            return View(timer);
        }

        // GET: Timers/Create
        public async Task<IActionResult> Create()
        {
            var vm = new TimersViewModel
            {
                StartTime = DateTime.Now,
                StopTime = null,
                Tasks = await _context.Tasks
                    .Select(task => new SelectListItem {Text = task.Description, Value = task.Id.ToString()})
                    .ToListAsync(),
                Users = await _context.Users
                    .Select(user => new SelectListItem {Text = user.UserName, Value = user.Id})
                    .ToListAsync()
            };

            return View(vm);
        }

        // POST: Timers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimersViewModel timer)
        {
            if (!ModelState.IsValid || 
                !int.TryParse(timer.SelectedTaskId, out var selectedTaskId) ||
                !int.TryParse(timer.SelectedUserId, out var selectedUserId))
                return View(timer);

            var newTimer = new Timer
            {
                StartTime = DateTime.Now,
                User = await _context.Users.FindAsync(selectedUserId),
                Task = await _context.Tasks.FindAsync(selectedTaskId)
            };

            _context.Add(newTimer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Timers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timer = await _context.Timers.FindAsync(id);
            if (timer == null)
            {
                return NotFound();
            }
            return View(timer);
        }

        // POST: Timers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,StopTime")] Timer timer)
        {
            if (id != timer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimerExists(timer.Id))
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
            return View(timer);
        }

        // GET: Timers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timer = await _context.Timers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timer == null)
            {
                return NotFound();
            }

            return View(timer);
        }

        // POST: Timers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timer = await _context.Timers.FindAsync(id);
            _context.Timers.Remove(timer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimerExists(int id)
        {
            return _context.Timers.Any(e => e.Id == id);
        }
    }
}
