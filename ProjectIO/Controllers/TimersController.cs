using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectIO.Data;
using ProjectIO.Models;
using ProjectIO.Models.ViewModels;
using Task = ProjectIO.Models.Task;

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
            return View(await _context.Timers
                .Include(t => t.Task)
                .Include(t => t.User)
                .ToListAsync());
        }

        // GET: Timers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timer = await _context.Timers
                .Include(t => t.Task)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timer == null)
            {
                return NotFound();
            }

            return View(timer);
        }

        // GET: Timers/Create
        public async Task<IActionResult> Create(int? taskId)
        {
            Task task;
            if (taskId != null)
            {
                task = await _context.Tasks.FindAsync(taskId);
            }
            else
            {
                task = null;
            }

            var vm = new TimersViewModel
            {
                StartTime = DateTime.Now,
                StopTime = null,
                Tasks = await _context.Tasks
                    .Select(t => new SelectListItem {Text = t.Name, Value = t.Id.ToString()})
                    .ToListAsync(),
                Users = await _context.Users
                    .Select(u => new SelectListItem {Text = u.UserName, Value = u.Id})
                    .ToListAsync()
            };

            if (User.Identity.IsAuthenticated)
            {
                var query = from user in _context.Users
                    where user.UserName == User.Identity.Name
                    select user.Id;

                vm.SelectedUserId = query.FirstOrDefault();
            }

            if (task != null)
            {
                vm.Task = task;
                vm.SelectedTaskId = task.Id.ToString();
            }

            return View(vm);
        }

        // POST: Timers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimersViewModel timer)
        {
            var selectedUserId = timer.SelectedUserId;
            if (!ModelState.IsValid ||
                !int.TryParse(timer.SelectedTaskId, out var selectedTaskId) ||
                selectedUserId == null)
            {
                return View(timer);
            }

            var newTimer = new Timer
            {
                StartTime = timer.StartTime,
                StopTime = timer.StopTime,
                Duration = CalculateDuration(timer),
                User = await _context.Users.FindAsync(selectedUserId),
                Task = await _context.Tasks.FindAsync(selectedTaskId)
            };

            _context.Add(newTimer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tasks", new {Id = timer.SelectedTaskId});
        }

        // GET: Timers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timer = await _context.Timers
                .Include(t => t.Task)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (timer == null)
            {
                return NotFound();
            }

            var selectedUserId = timer.User.Id;
            var selectedTaskId = timer.Task.Id.ToString();

            var vm = new TimersViewModel
            {
                Id = timer.Id,
                StartTime = timer.StartTime,
                StopTime = timer.StopTime,
                Duration = timer.Duration,
                SelectedTaskId = selectedTaskId,
                SelectedUserId = selectedUserId,
                User = await _context.Users.FindAsync(selectedUserId),
                Tasks = await _context.Tasks
                    .Select(t => new SelectListItem {Text = t.Name, Value = t.Id.ToString()})
                    .ToListAsync(),
                Users = await _context.Users
                    .Select(u => new SelectListItem {Text = u.UserName, Value = u.Id})
                    .ToListAsync()
            };

            return View(vm);
        }

        // POST: Timers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,StopTime,SelectedTaskId,SelectedUserId")] TimersViewModel timerVM)
        {
            var timer = await _context.Timers.FindAsync(id);
            if (timer == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid &&
                int.TryParse(timerVM.SelectedTaskId, out var selectedTaskID))
            {
                try
                {
                    timer.StartTime = timerVM.StartTime;
                    timer.StopTime = timerVM.StopTime;
                    timer.User = await _context.Users.FindAsync(timerVM.SelectedUserId);
                    timer.Task = await _context.Tasks.FindAsync(selectedTaskID);

                    if (timerVM.StopTime != null)
                    {
                        timer.Duration = CalculateDuration(timerVM);
                    }

                    _context.Update(timer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimerExists(timerVM.Id))
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

            return View(timerVM);
        }

        private TimeSpan? CalculateDuration(ITimer timer)
        {
            if (timer.StopTime == null)
                return null;

            return timer.StopTime - timer.StartTime;
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

        public async Task<IActionResult> StopTimer(int id)
        {
            var timer = await _context.Timers.FindAsync(id);
            timer.StopTime = DateTime.Now;
            timer.Duration = CalculateDuration(timer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}