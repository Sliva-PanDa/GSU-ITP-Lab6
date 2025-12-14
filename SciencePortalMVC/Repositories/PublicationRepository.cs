using Microsoft.EntityFrameworkCore;
using SciencePortalMVC.Data;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciencePortalMVC.Repositories
{
    public class PublicationRepository : IPublicationRepository
    {
        private readonly SciencePortalDbContext _context;

        public PublicationRepository(SciencePortalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Publication>> GetAllAsync()
        {
            return await _context.Publications.Include(p => p.Teachers).ToListAsync();
        }

        public IQueryable<Publication> GetPublicationsAsQueryable()
        {
            return _context.Publications.Include(p => p.Teachers).AsQueryable();
        }

        public async Task<Publication> GetByIdAsync(int id)
        {
            return await _context.Publications.Include(p => p.Teachers)
                .FirstOrDefaultAsync(p => p.PublicationId == id);
        }

        public async Task AddAsync(Publication publication)
        {
            await _context.Publications.AddAsync(publication);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Publication publication)
        {
            _context.Publications.Update(publication);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication != null)
            {
                _context.Publications.Remove(publication);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Publications.AnyAsync(e => e.PublicationId == id);
        }
    }
}