using Microsoft.EntityFrameworkCore;
using SciencePortalMVC.Data;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciencePortalMVC.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly SciencePortalDbContext _context;

        public TeacherRepository(SciencePortalDbContext context)
        {
            _context = context;
        }

        public IQueryable<Teacher> GetTeachersAsQueryable()
        {
            return _context.Teachers.Include(t => t.Department).AsQueryable();
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            return await _context.Teachers
                .Include(t => t.Department)
                .FirstOrDefaultAsync(m => m.TeacherId == id);
        }

        public async Task AddAsync(Teacher teacher)
        {
            _context.Add(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Teacher teacher)
        {
            _context.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TeacherExistsAsync(int id)
        {
            return await _context.Teachers.AnyAsync(e => e.TeacherId == id);
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }
    }
}