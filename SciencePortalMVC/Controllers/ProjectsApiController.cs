using Microsoft.AspNetCore.Mvc;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;
using SciencePortalMVC.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciencePortalMVC.Controllers
{
    [Route("api/[controller]")] // Определяет базовый маршрут для контроллера: /api/ProjectsApi
    [ApiController] // Указывает, что это API-контроллер с дополнительными возможностями
    public class ProjectsApiController : ControllerBase // Наследуемся от ControllerBase для API
    {
        private readonly IProjectRepository _projectRepo;
        private readonly ITeacherRepository _teacherRepo;

        public ProjectsApiController(IProjectRepository projectRepo, ITeacherRepository teacherRepo)
        {
            _projectRepo = projectRepo;
            _teacherRepo = teacherRepo;
        }

        // GET: api/ProjectsApi
        // Получить все проекты
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectApiDto>>> GetProjects()
        {
            var projects = await _projectRepo.GetAllAsync();
            var projectDtos = projects.Select(p => new ProjectApiDto
            {
                ProjectId = p.ProjectId,
                Name = p.Name,
                Number = p.Number,
                FundingOrg = p.FundingOrg,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                LeaderName = p.Leader?.FullName ?? "Не назначен" // Безопасный доступ к имени
            });
            return Ok(projectDtos.OrderByDescending(p => p.StartDate)); // Сортировка для удобства
        }

        // GET: api/ProjectsApi/5
        // Получить проект по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectApiDto>> GetProject(int id)
        {
            var project = await _projectRepo.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound(); // 404 Not Found
            }

            var projectDto = new ProjectApiDto
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                Number = project.Number,
                FundingOrg = project.FundingOrg,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                LeaderName = project.Leader?.FullName ?? "Не назначен"
            };
            return Ok(projectDto); // 200 OK
        }

        // POST: api/ProjectsApi
        // Создать новый проект
        [HttpPost]
        public async Task<ActionResult<ProjectApiDto>> PostProject(CreateProjectApiDto createDto)
        {
            // Проверка существования руководителя
            if (!await _teacherRepo.TeacherExistsAsync(createDto.LeaderId))
            {
                return BadRequest("Руководитель с указанным ID не найден."); // 400 Bad Request
            }

            var project = new Project
            {
                Name = createDto.Name,
                Number = createDto.Number,
                FundingOrg = createDto.FundingOrg,
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                LeaderId = createDto.LeaderId
            };

            await _projectRepo.AddAsync(project); // Сохраняем в базу данных

            // После создания, получаем его обратно с заполненным объектом Leader для DTO
            var createdProject = await _projectRepo.GetByIdAsync(project.ProjectId);

            var resultDto = new ProjectApiDto
            {
                ProjectId = createdProject.ProjectId,
                Name = createdProject.Name,
                Number = createdProject.Number,
                FundingOrg = createdProject.FundingOrg,
                StartDate = createdProject.StartDate,
                EndDate = createdProject.EndDate,
                LeaderName = createdProject.Leader?.FullName ?? "Не назначен"
            };

            // Возвращаем 201 Created с ссылкой на новый ресурс
            return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, resultDto);
        }

        // PUT: api/ProjectsApi/5
        // Обновить существующий проект
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, UpdateProjectApiDto updateDto)
        {
            if (id != updateDto.ProjectId)
            {
                return BadRequest("ID проекта в URL не совпадает с ID в теле запроса.");
            }

            var projectToUpdate = await _projectRepo.GetByIdAsync(id);
            if (projectToUpdate == null)
            {
                return NotFound(); // 404 Not Found
            }

            // Проверка существования нового руководителя
            if (!await _teacherRepo.TeacherExistsAsync(updateDto.LeaderId))
            {
                return BadRequest("Руководитель с указанным ID не найден.");
            }

            projectToUpdate.Name = updateDto.Name;
            projectToUpdate.Number = updateDto.Number;
            projectToUpdate.FundingOrg = updateDto.FundingOrg;
            projectToUpdate.StartDate = updateDto.StartDate;
            projectToUpdate.EndDate = updateDto.EndDate;
            projectToUpdate.LeaderId = updateDto.LeaderId;

            await _projectRepo.UpdateAsync(projectToUpdate);

            return NoContent(); // 204 No Content - успешное обновление без возврата тела
        }

        // DELETE: api/ProjectsApi/5
        // Удалить проект по ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _projectRepo.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound(); // 404 Not Found
            }

            await _projectRepo.DeleteAsync(id);
            return NoContent(); // 204 No Content - успешное удаление без возврата тела
        }
    }
}