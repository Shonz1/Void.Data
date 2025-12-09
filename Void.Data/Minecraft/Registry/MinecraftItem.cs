using System.Text.Json.Serialization;

namespace Void.Data.Minecraft.Registry;

internal class MinecraftItem
{
  [JsonPropertyName("protocol_id")]
  public int ProtocolId { get; init; }

  [JsonPropertyName("meta")]
  public int Meta { get; init; }
}
