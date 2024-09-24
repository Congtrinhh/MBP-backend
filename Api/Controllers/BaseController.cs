using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : class
    {
        protected readonly IBaseService<T> BaseService;

        public BaseController(IBaseService<T> baseService)
        {
            BaseService = baseService;
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var entity = await BaseService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var entities = await BaseService.GetAllAsync();
            return Ok(entities);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add([FromBody] T entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            var result = await BaseService.AddAsync(entity);
            if (!result)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return CreatedAtAction(nameof(GetById), new { id = entity.GetType().GetProperty("Id")?.GetValue(entity) }, entity);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(int id, [FromBody] T entity)
        {
            if (entity == null || id != (int)entity.GetType().GetProperty("Id")?.GetValue(entity))
            {
                return BadRequest();
            }

            var result = await BaseService.UpdateAsync(entity);
            if (!result)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var result = await BaseService.DeleteAsync(id);
            if (!result)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpPost("multiple")]
        public virtual async Task<IActionResult> AddMultiple([FromBody] IEnumerable<T> entities)
        {
            if (entities == null)
            {
                return BadRequest();
            }

            var result = await BaseService.AddMultipleAsync(entities);
            if (!result)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return Ok();
        }

        [HttpPut("multiple")]
        public virtual async Task<IActionResult> UpdateMultiple([FromBody] IEnumerable<T> entities)
        {
            if (entities == null)
            {
                return BadRequest();
            }

            var result = await BaseService.UpdateMultipleAsync(entities);
            if (!result)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return Ok();
        }

        [HttpDelete("multiple")]
        public virtual async Task<IActionResult> DeleteMultiple([FromBody] IEnumerable<int> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var result = await BaseService.DeleteMultipleAsync(ids);
            if (!result)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return Ok();
        }

        [HttpGet("paged")]
        public virtual async Task<IActionResult> GetPagedData(
            [FromQuery] string filter,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10)
        {
            var result = await BaseService.GetPagedDataAsync(filter, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet("find-by-field")]
        public virtual async Task<IActionResult> FindByField([FromQuery] string fieldName, [FromQuery] object fieldValue)
        {
            var entity = await BaseService.FindByFieldAsync(fieldName, fieldValue);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}
