using System.Text.Json.Serialization;

namespace Void.Data.Minecraft.Packet;

internal class MinecraftPacketDirectionRegistry
{
  [JsonPropertyName("clientbound")]
  public Dictionary<string, MinecraftPacket>? Clientbound { get; init; }

  [JsonPropertyName("serverbound")]
  public Dictionary<string, MinecraftPacket>? Serverbound { get; init; }
}
