using System.Text.Json;

namespace APISteam;

public static class JsonElementExtensions
{
    public static T GetPropertyOrDefault<T>(this JsonElement element, string propertyName, T defaultValue)
    {
        if (element.TryGetProperty(propertyName, out var property))
        {
            try
            {
                if (typeof(T) == typeof(int))
                    return (T)(object)property.GetInt32();
                if (typeof(T) == typeof(string))
                    return (T)(object)property.GetString();
                if (typeof(T) == typeof(bool))
                    return (T)(object)property.GetBoolean();
                if (typeof(T) == typeof(decimal))
                    return (T)(object)property.GetDecimal();
            }
            catch 
            {
                // Ignore exception and return default value
            }
        }
        return defaultValue;
    }
}