using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SciencePortalMVC.Data;
using SciencePortalMVC.Models;
using SciencePortalMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SciencePortalMVC.Controllers
{
    public class TeachersController : Controller
    {
        private readonly SciencePortalDbContext _context;

        public TeachersController(SciencePortalDbContext context)
        {
            _context = context;
        }

        // GET: Teachers
        [ResponseCache(Duration = 272, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index(string? fullName, int? departmentId, bool reset = false, int pageNumber = 1)
        {
            TeacherFilterViewModel filter;
            var sessionKey = "TeacherFilter";
            int pageSize = 10; 

            if (reset)
            {
                HttpContext.Session.Remove(sessionKey);
                filter = new TeacherFilterViewModel();
            }
            else if (fullName != null || departmentId.HasValue)
            {
                filter = new TeacherFilterViewModel { FullName = fullName, DepartmentId = departmentId };
                HttpContext.Session.SetString(sessionKey, JsonSerializer.Serialize(filter));
            }
            else
            {
                var sessionFilter = HttpContext.Session.GetString(sessionKey);
                filter = sessionFilter != null ? JsonSerializer.Deserialize<TeacherFilterViewModel>(sessionFilter) ?? new TeacherFilterViewModel() : new TeacherFilterViewModel();
            }

            ViewData["CurrentFilterName"] = filter.FullName;
            ViewData["CurrentFilterDeptId"] = filter.DepartmentId;

            var teachersQuery = _context.Teachers.AsQueryable(); 

            if (!string.IsNullOrEmpty(filter.FullName))
            {
                teachersQuery = teachersQuery.Where(t => t.FullName.Contains(filter.FullName));
            }

            if (filter.DepartmentId.HasValue)
            {
                teachersQuery = teachersQuery.Where(t => t.DepartmentId == filter.DepartmentId.Value);
            }

            // ѕолучаем общее количество записей дл€ пагинации
            var totalItems = await teachersQuery.CountAsync();
            // ¬ыбираем данные только дл€ текущей страницы
            var teachers = await teachersQuery.Include(t => t.Department)
                                              .Skip((pageNumber - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToListAsync();

            // ѕередаем в представление информацию о пагинации
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(totalItems / (double)pageSize);

            ViewData["DepartmentIdSelectList"] = new SelectList(_context.Departments, "DepartmentId", "Name", filter.DepartmentId);

            return View(teachers);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Department)
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name");
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("TeacherId,FullName,Position,Degree,DepartmentId")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", teacher.DepartmentId);
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", teacher.DepartmentId);
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,FullName,Position,Degree,DepartmentId")] Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherId))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", teacher.DepartmentId);
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Department)
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.TeacherId == id);
        }
    }
}
