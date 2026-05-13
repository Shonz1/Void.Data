using System.Text.Json.Serialization;

namespace Void.Data.Minecraft.Registry;

internal class MinecraftDataComponentType
{
  [JsonPropertyName("protocol_id")]
  public int ProtocolId { get; init; }
}
