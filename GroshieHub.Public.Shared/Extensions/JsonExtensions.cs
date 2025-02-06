using System.Text.Json;

namespace GroshieHub.Public.Shared.Extensions;

public static class JsonExtensions
{
	/// <summary>
	/// Retrieves a nested <see cref="JsonElement"/> property from the current <see cref="JsonElement"/>
	/// using a dot-separated path.
	/// </summary>
	/// <param name="jsonElement">The <see cref="JsonElement"/> to search within.</param>
	/// <param name="path">
	/// A dot-separated string representing the path to the desired property.
	/// Each segment of the path corresponds to a property name in the JSON hierarchy.
	/// </param>
	/// <returns>
	/// The <see cref="JsonElement"/> found at the specified path.
	/// </returns>
	/// <exception cref="JsonException">
	/// Thrown if the <paramref name="path"/> is null, empty, or whitespace,
	/// or if any segment of the <paramref name="path"/> does not exist in the JSON structure.
	/// </exception>
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

	/// <summary>
	/// Retrieves a nested <see cref="JsonElement"/> property from the current <see cref="JsonElement"/>
	/// using a dot-separated path, and deserializes its value to the specified type.
	/// </summary>
	/// <typeparam name="T">
	/// The type to which the value of the JSON property should be deserialized.
	/// </typeparam>
	/// <param name="jsonElement">The <see cref="JsonElement"/> to search within.</param>
	/// <param name="path">
	/// A dot-separated string representing the path to the desired property.
	/// Each segment of the path corresponds to a property name in the JSON hierarchy.
	/// </param>
	/// <param name="value">
	/// When this method returns, contains the deserialized value of the JSON property,
	/// if the property is found and successfully deserialized.
	/// </param>
	/// <returns>
	/// The <see cref="JsonElement"/> found at the specified path.
	/// </returns>
	/// <exception cref="JsonException">
	/// Thrown if the <paramref name="path"/> is null, empty, or whitespace,
	/// if the property does not exist in the JSON structure,
	/// or if the property's value cannot be deserialized into the specified type <typeparamref name="T"/>.
	/// </exception>
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

		value = jsonElement.Deserialize<T>()
			?? throw new JsonException($"JSON property with name '{path} doesn't match the expected type '{typeof(T).Name}.");

		return jsonElement;
	}
}