using System.IO.Compression;
using System.Text.Json;
using Void.Minecraft.Network;
using Void.Proxy.Api.Network;

namespace Void.Data.Minecraft.Packet;

internal class MinecraftPacketRegistry
{
  private static readonly Dictionary<ProtocolVersion, MinecraftPacketPhaseRegistry> Cache = new();

  public static MinecraftPacketPhaseRegistry? GetRegistry(ProtocolVersion protocolVersion)
  {
    var assembly = typeof(MinecraftPacketRegistry).Assembly;
    var versionName = protocolVersion.VersionIntroducedIn;

    lock (Cache)
    {
      if (!Cache.TryGetValue(protocolVersion, out var cachedRegistry))
      {
        using var stream = assembly.GetManifestResourceStream($"Resources/{versionName}/reports/packets.json.gz");
        if (stream == null)
          return null;

        using var gzip = new GZipStream(stream, CompressionMode.Decompress);

        var parsedRegistry = JsonSerializer.Deserialize<MinecraftPacketPhaseRegistry>(gzip);
        if (parsedRegistry == null)
          return null;

        Cache.Add(protocolVersion, parsedRegistry);

        return parsedRegistry;
      }

      return cachedRegistry;
    }
  }

  public static MinecraftPacketDirectionRegistry GetDirectionRegistry(MinecraftPacketPhaseRegistry registry,
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

  public static Dictionary<string, MinecraftPacket>? GetPacketRegistry(MinecraftPacketDirectionRegistry registry,
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
