using System.Text.Json.Serialization;

namespace Void.Data.Minecraft.Registry;

internal class MinecraftItemRegistry
{
  [JsonPropertyName("default")]
  public required string Default { get; init; }

  [JsonPropertyName("entries")]
  public required Dictionary<string, MinecraftItem> Entries { get; init; }
}
