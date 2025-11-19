using MongoDB.Driver;
using MillionBackend.Configuration;
using MillionBackend.Models;
using Microsoft.Extensions.Options;

namespace MillionBackend.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(string id);
    Task<Category> CreateAsync(Category category);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly IMongoCollection<Category> _categories;

    public CategoryRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _categories = database.GetCollection<Category>(mongoDbSettings.Value.CategoriesCollectionName);
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _categories
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(string id)
    {
        return await _categories
            .Find(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Category> CreateAsync(Category category)
    {
        category.Id = Guid.NewGuid().ToString();
        await _categories.InsertOneAsync(category);
        return category;
    }
}
