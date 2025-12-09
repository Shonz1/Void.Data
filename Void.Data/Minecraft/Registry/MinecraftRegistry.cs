using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using Void.Minecraft.Network;

namespace Void.Data.Minecraft.Registry;

internal class MinecraftRegistry
{
  private static readonly Dictionary<ProtocolVersion, MinecraftRegistry> Cache = new ();

  public static MinecraftRegistry? GetRegistry(ProtocolVersion protocolVersion)
  {
    var assembly = typeof(MinecraftRegistry).Assembly;
    var versionName = protocolVersion.VersionIntroducedIn;

    lock (Cache)
    {
      if (!Cache.ContainsKey(protocolVersion))
      {
        using var stream = assembly.GetManifestResourceStream($"Resources/{versionName}/reports/registries.json.gz");
        if (stream == null)
          return null;

        using var gzip = new GZipStream(stream, CompressionMode.Decompress);

        var parsedRegistry = JsonSerializer.Deserialize<MinecraftRegistry>(gzip);
        if (parsedRegistry == null)
          return null;

        Cache.Add(protocolVersion, parsedRegistry);
      }

      return Cache[protocolVersion];
    }
  }

  [JsonPropertyName("minecraft:item")]
  public required MinecraftItemRegistry MinecraftItemRegistry { get; init; }

  [JsonPropertyName("minecraft:menu")]
  public required MinecraftMenuRegistry MinecraftMenuRegistry { get; init; }
}
