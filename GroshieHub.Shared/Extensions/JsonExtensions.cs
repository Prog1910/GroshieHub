using System.Text.Json;

namespace GroshieHub.Shared.Extensions;

public static class JsonExtensions
{
	public static JsonElement GetProperty(this JsonElement jsonElement, string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			throw new JsonException("JSON element path can't be empty.");
		}

		var propertiesNames = path.Trim().Split('.');
		foreach (var propertyName in propertiesNames)
		{
			if (jsonElement.ValueKind != JsonValueKind.Object || !jsonElement.TryGetProperty(propertyName, out jsonElement))
			{
				throw new JsonException($"JSON doesn't contain element at path '{path}'.");
			}
		}

		return jsonElement;
	}

	public static JsonElement GetProperty<T>(this JsonElement jsonElement, string path, out T value)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			throw new JsonException("JSON property name can't be empty.");
		}

		if (path.Split(".").Length > 1)
		{
			jsonElement = GetProperty(jsonElement, path);
		}
		else
		{
			path = path.Trim();
			if (!jsonElement.TryGetProperty(path, out jsonElement))
			{
				throw new JsonException($"JSON doesn't contain property with name '{path}'.");
			}
		}

		value = jsonElement.Deserialize<T>() ?? throw new JsonException($"JSON property with name '{path} doesn't match the expected type '{typeof(T).Name}.");

		return jsonElement;
	}
}