using MongoDB.Driver;
using MillionBackend.Configuration;
using MillionBackend.Models;
using Microsoft.Extensions.Options;

namespace MillionBackend.Repositories;

public interface IPropertyRepository
{
    Task<List<Property>> GetAllAsync();
    Task<List<Property>> GetFilteredAsync(string? name, string? address, decimal? priceMin, decimal? priceMax, string? type, bool? active);
    Task<Property?> GetByIdAsync(string id);
    Task<Property> CreateAsync(Property property);
    Task<bool> UpdateAsync(string id, Property property);
    Task<bool> DeleteAsync(string id);
}

public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _properties;

    public PropertyRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _properties = database.GetCollection<Property>(mongoDbSettings.Value.PropertiesCollectionName);
    }

    public async Task<List<Property>> GetAllAsync()
    {
        return await _properties
            .Find(_ => true)
            .SortByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Property>> GetFilteredAsync(string? name, string? address, decimal? priceMin, decimal? priceMax, string? type, bool? active)
    {
        var filterBuilder = Builders<Property>.Filter;
        var filters = new List<FilterDefinition<Property>>();

        // Always start with a base filter
        filters.Add(filterBuilder.Empty);

        // Apply filters based on parameters
        if (!string.IsNullOrWhiteSpace(name))
        {
            filters.Add(filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i")));
        }

        if (!string.IsNullOrWhiteSpace(address))
        {
            filters.Add(filterBuilder.Regex(p => p.AddressProperty, new MongoDB.Bson.BsonRegularExpression(address, "i")));
        }

        if (priceMin.HasValue)
        {
            filters.Add(filterBuilder.Gte(p => p.PriceProperty, priceMin.Value));
        }

        if (priceMax.HasValue)
        {
            filters.Add(filterBuilder.Lte(p => p.PriceProperty, priceMax.Value));
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            filters.Add(filterBuilder.Eq(p => p.Type, type));
        }

        if (active.HasValue)
        {
            filters.Add(filterBuilder.Eq(p => p.Active, active.Value));
        }

        var combinedFilter = filterBuilder.And(filters);

        return await _properties
            .Find(combinedFilter)
            .SortByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(string id)
    {
        return await _properties
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Property> CreateAsync(Property property)
    {
        property.Id = Guid.NewGuid().ToString();
        property.CreatedAt = DateTime.UtcNow;
        await _properties.InsertOneAsync(property);
        return property;
    }

    public async Task<bool> UpdateAsync(string id, Property property)
    {
        var result = await _properties.ReplaceOneAsync(p => p.Id == id, property);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _properties.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }
}
