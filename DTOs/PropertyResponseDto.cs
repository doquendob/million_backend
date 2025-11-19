using MillionBackend.Models;

namespace MillionBackend.DTOs;

public class PropertyResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AddressProperty { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal PriceProperty { get; set; }
    public string? ImageUrl { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? IdOwner { get; set; }

    public static PropertyResponseDto FromProperty(Property property)
    {
        return new PropertyResponseDto
        {
            Id = property.Id,
            Name = property.Name,
            Description = property.Description,
            AddressProperty = property.AddressProperty,
            Type = property.Type,
            PriceProperty = property.PriceProperty,
            ImageUrl = property.ImageUrl,
            Active = property.Active,
            CreatedAt = property.CreatedAt,
            IdOwner = property.IdOwner
        };
    }
}
