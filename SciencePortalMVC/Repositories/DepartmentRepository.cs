using Microsoft.EntityFrameworkCore;
using SciencePortalMVC.Data;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciencePortalMVC.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly SciencePortalDbContext _context;

        public DepartmentRepository(SciencePortalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        // РЕАЛИЗАЦИЯ НЕДОСТАЮЩЕГО МЕТОДА
        public IQueryable<Department> GetDepartmentsAsQueryable()
        {
            return _context.Departments.AsQueryable();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Departments.AnyAsync(e => e.DepartmentId == id);
        }
    }
}