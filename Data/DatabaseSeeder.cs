using MillionBackend.Configuration;
using MillionBackend.Models;
using MillionBackend.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MillionBackend.Data;

public class DatabaseSeeder
{
    private readonly IMongoCollection<Property> _properties;
    private readonly IMongoCollection<Category> _categories;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        IOptions<MongoDbSettings> mongoDbSettings,
        ILogger<DatabaseSeeder> logger)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        
        _properties = database.GetCollection<Property>(mongoDbSettings.Value.PropertiesCollectionName);
        _categories = database.GetCollection<Category>(mongoDbSettings.Value.CategoriesCollectionName);
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            await SeedCategoriesAsync();
            await SeedPropertiesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding database");
        }
    }

    private async Task SeedCategoriesAsync()
    {
        var count = await _categories.CountDocumentsAsync(_ => true);
        if (count > 0)
        {
            _logger.LogInformation("Categories already seeded");
            return;
        }

        var categories = new List<Category>
        {
            new() { Id = "cat-001", Name = "House", Color = "#10b981" },
            new() { Id = "cat-002", Name = "Apartment", Color = "#3b82f6" },
            new() { Id = "cat-003", Name = "Villa", Color = "#f59e0b" },
            new() { Id = "cat-004", Name = "Townhouse", Color = "#8b5cf6" },
            new() { Id = "cat-005", Name = "Estate", Color = "#ec4899" }
        };

        await _categories.InsertManyAsync(categories);
        _logger.LogInformation("Categories seeded successfully");
    }

    private async Task SeedPropertiesAsync()
    {
        var count = await _properties.CountDocumentsAsync(_ => true);
        if (count > 0)
        {
            _logger.LogInformation("Properties already seeded");
            return;
        }

        var properties = new List<Property>
        {
            new()
            {
                Id = "prop-001",
                Name = "Modern Downtown Loft",
                Description = "Stunning contemporary loft in the heart of downtown with floor-to-ceiling windows, exposed brick, and high ceilings. Walking distance to restaurants and entertainment.",
                AddressProperty = "245 Market Street, San Francisco, CA 94102",
                Type = "Apartment",
                PriceProperty = 1250000,
                ImageUrl = "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=800",
                Active = true,
                CreatedAt = DateTime.Parse("2025-11-15T10:30:00.000Z").ToUniversalTime(),
                IdOwner = "owner-123"
            },
            new()
            {
                Id = "prop-002",
                Name = "Spacious Family Home",
                Description = "Beautiful 4-bedroom, 3-bathroom family home with large backyard, updated kitchen, and master suite. Perfect for growing families in excellent school district.",
                AddressProperty = "1834 Elm Avenue, Austin, TX 78704",
                Type = "House",
                PriceProperty = 875000,
                ImageUrl = "https://images.unsplash.com/photo-1568605114967-8130f3a36994?w=800",
                Active = true,
                CreatedAt = DateTime.Parse("2025-11-14T14:20:00.000Z").ToUniversalTime(),
                IdOwner = "owner-456"
            },
            new()
            {
                Id = "prop-003",
                Name = "Luxury Penthouse Suite",
                Description = "Exclusive penthouse with panoramic city views, private elevator, rooftop terrace, and premium finishes throughout. Ultra-luxury living at its finest.",
                AddressProperty = "88 Central Park West, New York, NY 10023",
                Type = "Apartment",
                PriceProperty = 4500000,
                ImageUrl = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=800",
                Active = true,
                CreatedAt = DateTime.Parse("2025-11-13T09:15:00.000Z").ToUniversalTime(),
                IdOwner = "owner-789"
            },
            new()
            {
                Id = "prop-004",
                Name = "Cozy Suburban Starter Home",
                Description = "Charming 2-bedroom, 1-bathroom home perfect for first-time buyers. Recently renovated with modern appliances and a fenced yard.",
                AddressProperty = "567 Oak Lane, Portland, OR 97210",
                Type = "House",
                PriceProperty = 425000,
                ImageUrl = "https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=800",
                Active = true,
                CreatedAt = DateTime.Parse("2025-11-12T16:45:00.000Z").ToUniversalTime(),
                IdOwner = "owner-234"
            },
            new()
            {
                Id = "prop-005",
                Name = "Beachfront Villa",
                Description = "Spectacular oceanfront property with direct beach access, infinity pool, 5 bedrooms, and breathtaking sunset views. Your dream vacation home awaits.",
                AddressProperty = "12 Ocean Drive, Miami Beach, FL 33139",
                Type = "Villa",
                PriceProperty = 3200000,
                ImageUrl = "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800",
                Active = true,
                CreatedAt = DateTime.Parse("2025-11-11T11:00:00.000Z").ToUniversalTime(),
                IdOwner = "owner-567"
            }
        };

        await _properties.InsertManyAsync(properties);
        _logger.LogInformation("Properties seeded successfully");
    }
}
