using SciencePortalMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciencePortalMVC.Interfaces
{
    public interface ITeacherRepository
    {
        IQueryable<Teacher> GetTeachersAsQueryable();
        Task<Teacher> GetByIdAsync(int id);
        Task AddAsync(Teacher teacher);
        Task UpdateAsync(Teacher teacher);
        Task DeleteAsync(int id);
        Task<bool> TeacherExistsAsync(int id); 
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    }
}