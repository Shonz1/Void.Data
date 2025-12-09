using System.Text.Json.Serialization;

namespace Void.Data.Minecraft.Packet;

internal class MinecraftPacket
{
  [JsonPropertyName("protocol_id")]
  public int ProtocolId { get; init; }
}
