using Microsoft.AspNetCore.Mvc;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;
using SciencePortalMVC.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciencePortalMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsApiController : ControllerBase
    {
        private readonly IDepartmentRepository _repo;

        public DepartmentsApiController(IDepartmentRepository repo)
        {
            _repo = repo;
        }

        // GET: api/DepartmentsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentApiDto>>> GetDepartments()
        {
            var departments = await _repo.GetAllAsync();
            var dtos = departments.Select(d => new DepartmentApiDto
            {
                DepartmentId = d.DepartmentId,
                Name = d.Name,
                Profile = d.Profile
            });
            return Ok(dtos);
        }

        // POST: api/DepartmentsApi
        [HttpPost]
        public async Task<ActionResult<DepartmentApiDto>> PostDepartment(CreateUpdateDepartmentDto dto)
        {
            var department = new Department { Name = dto.Name, Profile = dto.Profile };
            await _repo.AddAsync(department);
            var resultDto = new DepartmentApiDto { DepartmentId = department.DepartmentId, Name = department.Name, Profile = department.Profile };
            return CreatedAtAction(nameof(GetDepartments), new { id = department.DepartmentId }, resultDto);
        }

        // DELETE: api/DepartmentsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            if (!await _repo.ExistsAsync(id))
            {
                return NotFound();
            }
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}