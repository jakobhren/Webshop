using AdminSystem.Model.Entities;
using AdminSystem.Model.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EbookController : ControllerBase
    {

        protected EbookRepository Repository { get; }

        public EbookController(EbookRepository repository)
        {
            Repository = repository;
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Ebook> GetEbook([FromRoute] int id)
        {
            Ebook ebook = Repository.GetEbookById(id);
            if (ebook == null)
            {
                return NotFound();
            }
            return Ok(ebook);
        }
       
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Ebook>> GetEbooks()
        {
            return Ok(Repository.GetEbooks());
        }

        [HttpPost]
        public ActionResult Post([FromBody] Ebook ebook)
        {
            if (ebook == null)
            {
                return BadRequest("Ebook info not correct");
            }
            bool status = Repository.InsertEbook(ebook);
            if (status)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        public ActionResult UpdateEbook([FromBody] Ebook ebook)
        {
            if (ebook == null)
            {
                return BadRequest("Ebook info not correct");
            }
            Ebook existingEbook = Repository.GetEbookById(ebook.EbookId);
            if (existingEbook == null)
            {
                return NotFound($"Ebook with id {ebook.EbookId} not found");
            }
            bool status = Repository.UpdateEbook(ebook);
            if (status)
            {
                return Ok();
            }
            return BadRequest("Something went wrong");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEbook([FromRoute] int id)
        {
            Ebook existingEbook = Repository.GetEbookById(id);
            if (existingEbook == null)
            {
                return NotFound($"Ebook with id {id} not found");
            }
            bool status = Repository.DeleteEbook(id);
            if (status)
            {
                return NoContent();
            }
            return BadRequest($"Unable to delete ebook with id {id}");
        }
    }
}
