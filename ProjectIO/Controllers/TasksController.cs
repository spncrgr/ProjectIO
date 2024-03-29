﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectIO.Data;
using ProjectIO.Models.ViewModels;
using Task = ProjectIO.Models.Task;

namespace ProjectIO.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tasks.Include(task => task.Project).ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Timers)
                .ThenInclude(timer => timer.User)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }


            var vm = new TaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Timers = task.Timers,
                Project = task.Project
            };

            return View(vm);
        }

        // GET: Tasks/Create
        public async Task<IActionResult> Create()
        {
            var vm = new TaskViewModel
            {
                Projects = await _context.Projects
                    .Select(project => new SelectListItem {Text = project.Name, Value = project.Id.ToString()})
                    .ToListAsync()
            };
            return View(vm);
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,SelectedProjectId")] TaskViewModel task)
        {
            if (!ModelState.IsValid || !int.TryParse(task.SelectedProjectId, out var selectedProjectId))
                return View(task);

            var newTask = new Task
            {
                Name = task.Name,
                Description = task.Description,
                Project = await _context.Projects.FindAsync(selectedProjectId)
            };

            _context.Add(newTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            var vm = new TaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Projects = await _context.Projects
                    .Select(project => new SelectListItem {Text = project.Name, Value = project.Id.ToString()})
                    .ToListAsync(),
                SelectedProjectId = task.Project.Id.ToString()
            };
            return View(vm);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,SelectedProjectId")] TaskViewModel tvm)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid && int.TryParse(tvm.SelectedProjectId, out var selectedProjectId))
            {
                try
                {
                    task.Name = tvm.Name;
                    task.Description = tvm.Description;
                    task.Project = await _context.Projects.FindAsync(selectedProjectId);

                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(tvm.Id))
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
            return View(tvm);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
