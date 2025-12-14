using Microsoft.AspNetCore.Mvc;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciencePortalMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersApiController : ControllerBase
    {
        private readonly ITeacherRepository _teacherRepo;

        public TeachersApiController(ITeacherRepository teacherRepo)
        {
            _teacherRepo = teacherRepo;
        }

        // GET: api/TeachersApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherApiDto>>> GetTeachers()
        {
            // Используем IQueryable для эффективности
            var teachers = _teacherRepo.GetTeachersAsQueryable();

            var teacherDtos = teachers.Select(t => new TeacherApiDto
            {
                TeacherId = t.TeacherId,
                FullName = t.FullName
            }).OrderBy(t => t.FullName); // Сортируем по имени для удобства

            return Ok(teacherDtos);
        }
    }
}