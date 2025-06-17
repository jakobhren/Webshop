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
    public class CategoryController : ControllerBase
    {
        protected CategoryRepository Repository { get; }

        public CategoryController(CategoryRepository repository)
        {
            Repository = repository;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Category> GetCategory([FromRoute] int id)
        {
            Category category = Repository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            return Ok(Repository.GetCategories());
        }

        [HttpPost]
        public ActionResult Post([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category info not correct");
            }
            bool status = Repository.InsertCategory(category);
            if (status)
            {
                // Optionally, return the created Category along with a 201 status and the URL to access it
                return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
            }
            return BadRequest("Something went wrong while inserting the category.");
        }

        [HttpPut]
        public ActionResult UpdateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category info not correct");
            }
            Category existingCategory = Repository.GetCategoryById(category.CategoryId);
            if (existingCategory == null)
            {
                return NotFound($"Category with id {category.CategoryId} not found");
            }
            bool status = Repository.UpdateCategory(category);
            if (status)
            {
                return Ok();
            }
            return BadRequest("Something went wrong while updating the category.");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCategory([FromRoute] int id)
        {
            Category existingCategory = Repository.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound($"Category with id {id} not found");
            }
            bool status = Repository.DeleteCategory(id);
            if (status)
            {
                return NoContent();
            }
            return BadRequest($"Unable to delete category with id {id}");
        }
    }
}
