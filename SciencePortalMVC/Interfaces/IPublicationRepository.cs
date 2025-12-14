using SciencePortalMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciencePortalMVC.Interfaces
{
    public interface IPublicationRepository
    {
        Task<IEnumerable<Publication>> GetAllAsync();
        IQueryable<Publication> GetPublicationsAsQueryable(); 
        Task<Publication> GetByIdAsync(int id);
        Task AddAsync(Publication publication);
        Task UpdateAsync(Publication publication);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}