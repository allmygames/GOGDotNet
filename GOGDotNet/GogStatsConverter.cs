using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using GOGDotNet.Models;

namespace GOGDotNet
{
  /// <summary>
  /// Converts Gog `stats` property
  /// </summary>
  /// <remarks>
  /// GOG returns an empty array when there are no stats, but an hash map with a single stats object when there are stats.
  /// </remarks> 
  public class GogStatsConverter : JsonConverter<Dictionary<string, GogStats>>
  {
    public override Dictionary<string, GogStats> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      if (reader.TokenType == JsonTokenType.StartArray)
      {
        reader.Read();
        if (reader.TokenType == JsonTokenType.EndArray)
        {
          return null;
        }

        throw new JsonException("Expected array or object for stats.");
      }

      return JsonSerializer.Deserialize<Dictionary<string, GogStats>>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, GogStats> value, JsonSerializerOptions options)
    {
      if (value == null || value.Count == 0)
      {
        writer.WriteStartArray();
        writer.WriteEndArray();
      }
      else
      {
        JsonSerializer.Serialize(writer, value, options);
      }
    }
  }
}