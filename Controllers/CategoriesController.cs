using Microsoft.AspNetCore.Mvc;
using MillionBackend.Models;
using MillionBackend.Repositories;

namespace MillionBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _repository;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryRepository repository, ILogger<CategoriesController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        try
        {
            var categories = await _repository.GetAllAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving categories");
            return StatusCode(500, new { message = "An error occurred while retrieving categories" });
        }
    }

    // GET: api/categories/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(string id)
    {
        try
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new { message = $"Category with ID '{id}' not found" });
            }

            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving category {CategoryId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the category" });
        }
    }
}
