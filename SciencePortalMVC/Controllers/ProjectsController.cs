using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;

namespace SciencePortalMVC.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        // Конструктор теперь СНОВА принимает ТОЛЬКО репозиторий
        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // GET: Projects
        [ResponseCache(Duration = 272, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index(string? searchString, int pageNumber = 1)
        {
            ViewData["CurrentFilter"] = searchString;
            var query = _projectRepository.GetProjectsAsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Name.Contains(searchString)
                                       || p.Number.Contains(searchString)
                                       || p.FundingOrg.Contains(searchString));
            }

            int pageSize = 10;
            // Добавляем сортировку по дате начала, чтобы самые новые проекты были сверху
            var sortedQuery = query.OrderByDescending(p => p.StartDate);

            return View(await PaginatedList<Project>.CreateAsync(sortedQuery, pageNumber, pageSize));
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var project = await _projectRepository.GetByIdAsync(id.Value);
            return project == null ? NotFound() : View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            // Используем репозиторий для получения списка преподавателей
            ViewData["LeaderId"] = new SelectList(await _projectRepository.GetAllTeachersAsync(), "TeacherId", "FullName");
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ProjectId,Name,Number,FundingOrg,StartDate,EndDate,LeaderId")] Project project)
        {
            if (ModelState.IsValid)
            {
                await _projectRepository.AddAsync(project);
                return RedirectToAction(nameof(Index));
            }
            ViewData["LeaderId"] = new SelectList(await _projectRepository.GetAllTeachersAsync(), "TeacherId", "FullName", project.LeaderId);
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var project = await _projectRepository.GetByIdAsync(id.Value);
            if (project == null) return NotFound();
            ViewData["LeaderId"] = new SelectList(await _projectRepository.GetAllTeachersAsync(), "TeacherId", "FullName", project.LeaderId);
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,Number,FundingOrg,StartDate,EndDate,LeaderId")] Project project)
        {
            if (id != project.ProjectId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _projectRepository.UpdateAsync(project);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _projectRepository.ExistsAsync(project.ProjectId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LeaderId"] = new SelectList(await _projectRepository.GetAllTeachersAsync(), "TeacherId", "FullName", project.LeaderId);
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var project = await _projectRepository.GetByIdAsync(id.Value);
            return project == null ? NotFound() : View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _projectRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}