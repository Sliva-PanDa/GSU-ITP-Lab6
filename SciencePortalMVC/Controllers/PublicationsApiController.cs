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
    public class PublicationsApiController : ControllerBase
    {
        private readonly IPublicationRepository _repo;

        public PublicationsApiController(IPublicationRepository repo)
        {
            _repo = repo;
        }

        // GET: api/PublicationsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicationApiDto>>> GetPublications()
        {
            var publications = await _repo.GetAllAsync();
            var dtos = publications.Select(p => new PublicationApiDto
            {
                PublicationId = p.PublicationId,
                Title = p.Title,
                Type = p.Type,
                Year = p.Year,
                AuthorNames = p.Teachers.Select(t => t.FullName).ToList()
            });
            return Ok(dtos);
        }

        // POST: api/PublicationsApi
        [HttpPost]
        public async Task<ActionResult<PublicationApiDto>> PostPublication(CreateUpdatePublicationDto dto)
        {
            var publication = new Publication { Title = dto.Title, Type = dto.Type, Year = dto.Year };
            await _repo.AddAsync(publication);
            var resultDto = new PublicationApiDto { PublicationId = publication.PublicationId, Title = publication.Title, Type = publication.Type, Year = publication.Year };
            return CreatedAtAction(nameof(GetPublications), new { id = publication.PublicationId }, resultDto);
        }

        // PUT: api/PublicationsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublication(int id, CreateUpdatePublicationDto dto)
        {
            var publicationToUpdate = await _repo.GetByIdAsync(id);
            if (publicationToUpdate == null)
            {
                return NotFound();
            }

            publicationToUpdate.Title = dto.Title;
            publicationToUpdate.Type = dto.Type;
            publicationToUpdate.Year = dto.Year;

            await _repo.UpdateAsync(publicationToUpdate);

            return NoContent();
        }

        // DELETE: api/PublicationsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublication(int id)
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