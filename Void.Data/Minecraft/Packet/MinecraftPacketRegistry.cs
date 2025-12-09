using System.IO.Compression;
using System.Text.Json;
using Void.Data.Minecraft.Registry;
using Void.Minecraft.Network;
using Void.Proxy.Api.Network;

namespace Void.Data.Minecraft.Packet;

public class MinecraftPacketRegistry
{
  private static readonly Dictionary<ProtocolVersion, MinecraftPacketPhaseRegistry> Cache = new ();

  public static int GetId(ProtocolVersion protocolVersion, Phase phase, Direction direction, Identifier identifier)
  {
    var packetRegistry = GetPacketRegistry(protocolVersion, phase, direction);
    if (packetRegistry == null)
      return -1;

    var identifierString = identifier.ToString();

    if (!packetRegistry.TryGetValue(identifierString, out var packet))
      return -1;

    return packet.ProtocolId;
  }

  public static Identifier? GetIdentifier(ProtocolVersion protocolVersion, Phase phase, Direction direction, int protocolId)
  {
    var packetRegistry = GetPacketRegistry(protocolVersion, phase, direction);

    if (packetRegistry == null)
      return null;

    var match = packetRegistry.FirstOrDefault(i => i.Value.ProtocolId == protocolId);

    return match.Key != null ? Identifier.FromString(match.Key) : null;
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

  private static Dictionary<string, MinecraftPacket>? GetPacketRegistry(ProtocolVersion protocolVersion, Phase phase, Direction direction)
  {
    var assembly = typeof(MinecraftPacketRegistry).Assembly;
    var versionName = protocolVersion.VersionIntroducedIn;

    lock (Cache)
    {
      if (!Cache.ContainsKey(protocolVersion))
      {
        using var stream = assembly.GetManifestResourceStream($"Resources/{versionName}/reports/packets.json.gz");
        if (stream == null)
          return null;

        using var gzip = new GZipStream(stream, CompressionMode.Decompress);

        var parsedRegistry = JsonSerializer.Deserialize<MinecraftPacketPhaseRegistry>(gzip);
        if (parsedRegistry == null)
          return null;

        Cache.Add(protocolVersion, parsedRegistry);
      }

      var registry = Cache[protocolVersion];
      var directionRegistry = GetDirectionRegistry(registry, phase);

      return direction switch
      {
        Direction.Clientbound => directionRegistry.Clientbound,
        Direction.Serverbound => directionRegistry.Serverbound,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
      };
    }
  }
}
