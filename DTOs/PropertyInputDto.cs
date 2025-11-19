using System.ComponentModel.DataAnnotations;

namespace MillionBackend.DTOs;

public class PropertyInputDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required")]
    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string AddressProperty { get; set; } = string.Empty;

    [Required(ErrorMessage = "Type is required")]
    [StringLength(100, ErrorMessage = "Type cannot exceed 100 characters")]
    public string Type { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
    public decimal PriceProperty { get; set; }

    [Url(ErrorMessage = "Invalid URL format")]
    public string? ImageUrl { get; set; }

    public bool Active { get; set; } = true;

    public string? IdOwner { get; set; }
}
