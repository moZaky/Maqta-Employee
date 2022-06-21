using System.Text.Json.Serialization;

namespace MAQTA.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserType
    {
        Admin,
        User
    }
}
