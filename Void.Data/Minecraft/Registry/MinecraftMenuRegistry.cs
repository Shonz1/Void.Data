using System.Text.Json.Serialization;

namespace Void.Data.Minecraft.Registry;

internal class MinecraftMenuRegistry
{
  [JsonPropertyName("entries")]
  public required Dictionary<string, MinecraftMenu> Entries { get; init; }
}
