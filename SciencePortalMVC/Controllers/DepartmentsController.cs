using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;

namespace SciencePortalMVC.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index(string? searchString, int pageNumber = 1)
        {
            ViewData["CurrentFilter"] = searchString;
            var query = _departmentRepository.GetDepartmentsAsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(d => d.Name.Contains(searchString) || d.Profile.Contains(searchString));
            }

            int pageSize = 10;
            return View(await PaginatedList<Department>.CreateAsync(query, pageNumber, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var department = await _departmentRepository.GetByIdAsync(id.Value);
            return department == null ? NotFound() : View(department);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("DepartmentId,Name,Profile")] Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentRepository.AddAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var department = await _departmentRepository.GetByIdAsync(id.Value);
            return department == null ? NotFound() : View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,Name,Profile")] Department department)
        {
            if (id != department.DepartmentId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentRepository.UpdateAsync(department);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _departmentRepository.ExistsAsync(department.DepartmentId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var department = await _departmentRepository.GetByIdAsync(id.Value);
            return department == null ? NotFound() : View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}