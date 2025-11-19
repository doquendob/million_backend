using Microsoft.AspNetCore.Mvc;
using MillionBackend.DTOs;
using MillionBackend.Models;
using MillionBackend.Repositories;

namespace MillionBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyRepository _repository;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(IPropertyRepository repository, ILogger<PropertiesController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // GET: api/properties
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropertyResponseDto>>> GetProperties(
        [FromQuery] string? name,
        [FromQuery] string? address,
        [FromQuery] decimal? priceMin,
        [FromQuery] decimal? priceMax,
        [FromQuery] string? type,
        [FromQuery] bool? active)
    {
        try
        {
            var properties = await _repository.GetFilteredAsync(name, address, priceMin, priceMax, type, active);
            var response = properties.Select(PropertyResponseDto.FromProperty);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving properties");
            return StatusCode(500, new { message = "An error occurred while retrieving properties" });
        }
    }

    // GET: api/properties/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyResponseDto>> GetProperty(string id)
    {
        try
        {
            var property = await _repository.GetByIdAsync(id);

            if (property == null)
            {
                return NotFound(new { message = $"Property with ID '{id}' not found" });
            }

            return Ok(PropertyResponseDto.FromProperty(property));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving property {PropertyId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the property" });
        }
    }

    // POST: api/properties
    [HttpPost]
    public async Task<ActionResult<PropertyResponseDto>> CreateProperty([FromBody] PropertyInputDto input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var property = new Property
            {
                Name = input.Name,
                Description = input.Description,
                AddressProperty = input.AddressProperty,
                Type = input.Type,
                PriceProperty = input.PriceProperty,
                ImageUrl = input.ImageUrl,
                Active = input.Active,
                IdOwner = input.IdOwner
            };

            var created = await _repository.CreateAsync(property);
            var response = PropertyResponseDto.FromProperty(created);
            return CreatedAtAction(nameof(GetProperty), new { id = created.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating property");
            return StatusCode(500, new { message = "An error occurred while creating the property" });
        }
    }

    // PUT: api/properties/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<PropertyResponseDto>> UpdateProperty(string id, [FromBody] PropertyInputDto input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                return NotFound(new { message = $"Property with ID '{id}' not found" });
            }

            // Update properties
            existing.Name = input.Name;
            existing.Description = input.Description;
            existing.AddressProperty = input.AddressProperty;
            existing.Type = input.Type;
            existing.PriceProperty = input.PriceProperty;
            existing.ImageUrl = input.ImageUrl;
            existing.Active = input.Active;
            existing.IdOwner = input.IdOwner;

            await _repository.UpdateAsync(id, existing);

            var response = PropertyResponseDto.FromProperty(existing);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating property {PropertyId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the property" });
        }
    }

    // DELETE: api/properties/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProperty(string id)
    {
        try
        {
            var result = await _repository.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"Property with ID '{id}' not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting property {PropertyId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the property" });
        }
    }
}
