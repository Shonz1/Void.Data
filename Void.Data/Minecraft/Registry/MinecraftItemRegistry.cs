using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using Void.Minecraft.Network;

namespace Void.Data.Minecraft.Registry;

public class MinecraftItem
{
  [JsonPropertyName("protocol_id")] public int ProtocolId { get; init; }

  [JsonPropertyName("meta")] public int Meta { get; init; }
}

public class MinecraftItemRegistry
{
  private static readonly Identifier air = "minecraft:air";

  public static int GetId(ProtocolVersion protocolVersion, Identifier identifier)
  {
    var assembly = typeof(MinecraftItemRegistry).Assembly;
    var versionName = protocolVersion.VersionIntroducedIn;

    using var stream = assembly.GetManifestResourceStream($"Resources/{versionName}/reports/registries.json.gz");
    if (stream == null)
      return 0;

    using var gzip = new GZipStream(stream, CompressionMode.Decompress);

    var registry = JsonSerializer.Deserialize<MinecraftRegistry>(gzip);
    if (registry == null)
      return 0;

    var item = registry.MinecraftItemRegistry.Entries[registry.MinecraftItemRegistry.Default];

    var identifierString = identifier.ToString();

    if (registry.MinecraftItemRegistry.Entries.ContainsKey(identifierString))
      item = registry.MinecraftItemRegistry.Entries[identifierString];

    return item.ProtocolId;
  }

  public static int GetMeta(ProtocolVersion protocolVersion, Identifier identifier)
  {
    var assembly = typeof(MinecraftItemRegistry).Assembly;
    var versionName = protocolVersion.VersionIntroducedIn;

    using var stream = assembly.GetManifestResourceStream($"Resources/{versionName}/reports/registries.json.gz");
    if (stream == null)
      return 0;

    using var gzip = new GZipStream(stream, CompressionMode.Decompress);

    var registry = JsonSerializer.Deserialize<MinecraftRegistry>(gzip);
    if (registry == null)
      return 0;

    var item = registry.MinecraftItemRegistry.Entries[registry.MinecraftItemRegistry.Default];

    var identifierString = identifier.ToString();

    if (registry.MinecraftItemRegistry.Entries.ContainsKey(identifierString))
      item = registry.MinecraftItemRegistry.Entries[identifierString];

    return item.Meta;
  }

  public static Identifier GetIdentifier(ProtocolVersion protocolVersion, int itemId, int meta = 0)
  {
    var assembly = typeof(MinecraftItemRegistry).Assembly;
    var versionName = protocolVersion.VersionIntroducedIn;

    using var stream = assembly.GetManifestResourceStream($"Resources/{versionName}/reports/registries.json.gz");
    if (stream == null)
      return air;

    using var gzip = new GZipStream(stream, CompressionMode.Decompress);

    var registry = JsonSerializer.Deserialize<MinecraftRegistry>(gzip);
    if (registry == null)
      return air;

    foreach (var entry in registry.MinecraftItemRegistry.Entries)
    {
      if (entry.Value.ProtocolId == itemId && entry.Value.Meta == meta)
        return entry.Key;
    }

    return air;
  }

  [JsonPropertyName("default")] public required string Default { get; init; }

  [JsonPropertyName("entries")] public required Dictionary<string, MinecraftItem> Entries { get; init; }
}
