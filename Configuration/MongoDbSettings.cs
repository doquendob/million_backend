namespace MillionBackend.Configuration;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "RealEstateDb";
    public string PropertiesCollectionName { get; set; } = "Properties";
    public string CategoriesCollectionName { get; set; } = "Categories";
}
