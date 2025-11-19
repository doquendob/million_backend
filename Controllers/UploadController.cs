using Microsoft.AspNetCore.Mvc;

namespace MillionBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly ILogger<UploadController> _logger;
    private readonly IWebHostEnvironment _environment;

    public UploadController(ILogger<UploadController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    // POST: api/upload/image
    [HttpPost("image")]
    public async Task<ActionResult<object>> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded" });
        }

        // Validate file type
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(new { message = "Invalid file type. Only images are allowed (jpg, jpeg, png, gif, webp)" });
        }

        // Validate file size (max 5MB)
        const long maxFileSize = 5 * 1024 * 1024;
        if (file.Length > maxFileSize)
        {
            return BadRequest(new { message = "File size exceeds 5MB limit" });
        }

        try
        {
           // Handle null WebRootPath (common in development)
            var rootPath = _environment.WebRootPath ?? _environment.ContentRootPath;
            var uploadsFolder = Path.Combine(rootPath, "uploads", "properties");

            // Ensure directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return URL
            var imageUrl = $"/uploads/properties/{fileName}";

            _logger.LogInformation("Image uploaded successfully: {FileName}", fileName);

            return Ok(new { imageUrl, fileName });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image");
            return StatusCode(500, new { message = "An error occurred while uploading the image" });
        }
    }

    // DELETE: api/upload/image
    [HttpDelete("image")]
    public IActionResult DeleteImage([FromQuery] string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return BadRequest(new { message = "File name is required" });
        }

        try
        {
            var rootPath = _environment.WebRootPath ?? _environment.ContentRootPath;
            var filePath = Path.Combine(rootPath, "uploads", "properties", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { message = "File not found" });
            }

            System.IO.File.Delete(filePath);
            _logger.LogInformation("Image deleted successfully: {FileName}", fileName);

            return Ok(new { message = "Image deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting image {FileName}", fileName);
            return StatusCode(500, new { message = "An error occurred while deleting the image" });
        }
    }
}
