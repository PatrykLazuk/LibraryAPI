using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Api.DTOs;
using Library.Api.Extensions;
using Library.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<BookDto>>> GetAll(int pageNumber = 1, int pageSize = 20, string? sortBy = null, bool asc = true)
        {
            var result = await _service.GetAsync(pageNumber, pageSize, sortBy, asc);
            return Ok(new PaginatedResult<BookDto>(
                result.Items.Select(b => b.ToDto()).ToList(),
                result.TotalCount,
                result.PageNumber,
                result.PageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var book = await _service.GetByIdAsync(id);
            return book is null ? NotFound() : Ok(book.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> Create(CreateBookDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto.ToEntity());
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> Update(int id, UpdateBookDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto.ToEntityForUpdate());
                return Ok(updated.ToDto());
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}