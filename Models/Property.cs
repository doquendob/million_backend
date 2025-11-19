using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MillionBackend.Models;

public class Property
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("addressProperty")]
    public string AddressProperty { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("priceProperty")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PriceProperty { get; set; }

    [BsonElement("imageUrl")]
    public string? ImageUrl { get; set; }

    [BsonElement("active")]
    public bool Active { get; set; } = true;

    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("idOwner")]
    public string? IdOwner { get; set; }
}
