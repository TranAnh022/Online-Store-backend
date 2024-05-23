using System.Text.Json.Serialization;

namespace Ecommerce.Core.src.ValueObjects
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Pending,
        Shipped,
        Cancelled,

    }
}