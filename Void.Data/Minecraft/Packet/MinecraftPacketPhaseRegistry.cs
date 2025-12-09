using System.Text.Json.Serialization;

namespace Void.Data.Minecraft.Packet;

internal class MinecraftPacketPhaseRegistry
{
  [JsonPropertyName("handshake")]
  public required MinecraftPacketDirectionRegistry Handshake { get; init; }

  [JsonPropertyName("status")]
  public required MinecraftPacketDirectionRegistry Status { get; init; }

  [JsonPropertyName("login")]
  public required MinecraftPacketDirectionRegistry Login { get; init; }

  [JsonPropertyName("configuration")]
  public required MinecraftPacketDirectionRegistry Configuration { get; init; }

  [JsonPropertyName("play")]
  public required MinecraftPacketDirectionRegistry Play { get; init; }
}
