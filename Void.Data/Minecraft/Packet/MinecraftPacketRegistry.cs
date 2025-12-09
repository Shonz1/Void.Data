using System.IO.Compression;
using System.Text.Json;
using Void.Data.Minecraft.Registry;
using Void.Minecraft.Network;
using Void.Proxy.Api.Network;

namespace Void.Data.Minecraft.Packet;

public class MinecraftPacketRegistry
{
  public static int GetId(ProtocolVersion protocolVersion, Phase phase, Direction direction, Identifier identifier)
  {
    var assembly = typeof(MinecraftItemRegistry).Assembly;
    var versionName = protocolVersion.VersionIntroducedIn;

    using var stream = assembly.GetManifestResourceStream($"Resources/{versionName}/reports/packets.json.gz");
    if (stream == null)
      return -1;

    using var gzip = new GZipStream(stream, CompressionMode.Decompress);

    var registry = JsonSerializer.Deserialize<MinecraftPacketPhaseRegistry>(gzip);
    if (registry == null)
      return -1;

    var directionRegistry = GetDirectionRegistry(registry, phase);
    var packetRegistry = GetPacketRegistry(directionRegistry, direction);
    if (packetRegistry == null)
      return -1;

    var identifierString = identifier.ToString();
    var item = packetRegistry[identifierString];

    return item.ProtocolId;
  }

  public static Identifier? GetIdentifier(ProtocolVersion protocolVersion, Phase phase, Direction direction, int protocolId)
  {
    var assembly = typeof(MinecraftItemRegistry).Assembly;
    var versionName = protocolVersion.VersionIntroducedIn;

    using var stream = assembly.GetManifestResourceStream($"Resources/{versionName}/reports/packets.json.gz");
    if (stream == null)
      return null;

    using var gzip = new GZipStream(stream, CompressionMode.Decompress);

    var registry = JsonSerializer.Deserialize<MinecraftPacketPhaseRegistry>(gzip);
    if (registry == null)
      return null;

    var directionRegistry = GetDirectionRegistry(registry, phase);
    var packetRegistry = GetPacketRegistry(directionRegistry, direction);
    if (packetRegistry == null)
      return null;

    foreach (var entry in packetRegistry)
    {
      if (entry.Value.ProtocolId == protocolId)
        return entry.Key;
    }

    return null;
  }

  private static MinecraftPacketDirectionRegistry GetDirectionRegistry(MinecraftPacketPhaseRegistry registry,
    Phase phase)
  {
    return phase switch
    {
      Phase.Handshake => registry.Handshake,
      Phase.Status => registry.Status,
      Phase.Login => registry.Login,
      Phase.Configuration => registry.Configuration,
      Phase.Play => registry.Play,
      _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
    };
  }

  private static Dictionary<string, MinecraftPacket>? GetPacketRegistry(MinecraftPacketDirectionRegistry registry,
    Direction direction)
  {
    return direction switch
    {
      Direction.Clientbound => registry.Clientbound,
      Direction.Serverbound => registry.Serverbound,
      _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };
  }
}
