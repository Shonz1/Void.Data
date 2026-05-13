using System.Text.Json.Serialization;

namespace Void.Data.Minecraft.Registry;

internal class MinecraftDataComponentTypeRegistry
{
  [JsonPropertyName("entries")]
  public required Dictionary<string, MinecraftDataComponentType> Entries { get; init; }
}
