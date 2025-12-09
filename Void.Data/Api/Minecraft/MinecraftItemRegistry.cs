using Void.Data.Minecraft.Registry;
using Void.Minecraft.Network;

namespace Void.Data.Api.Minecraft;

public static class MinecraftItemRegistry
{
  private static readonly Identifier air = "minecraft:air";

  public static int GetId(ProtocolVersion protocolVersion, Identifier identifier)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return 0;

    var identifierString = identifier.ToString();

    if (registry.MinecraftItemRegistry.Entries.TryGetValue(identifierString, out var item))
      return item.ProtocolId;

    if (registry.MinecraftItemRegistry.Entries.TryGetValue(registry.MinecraftItemRegistry.Default, out item))
      return item.ProtocolId;

    return 0;
  }

  public static int GetMeta(ProtocolVersion protocolVersion, Identifier identifier)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return 0;

    var identifierString = identifier.ToString();

    if (registry.MinecraftItemRegistry.Entries.TryGetValue(identifierString, out var item))
      return item.Meta;

    if (registry.MinecraftItemRegistry.Entries.TryGetValue(registry.MinecraftItemRegistry.Default, out item))
      return item.Meta;

    return 0;
  }

  public static Identifier GetIdentifier(ProtocolVersion protocolVersion, int itemId, int meta = 0)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return air;

    var match = registry.MinecraftItemRegistry.Entries.FirstOrDefault(i => i.Value.ProtocolId == itemId && i.Value.Meta == meta);

    return match.Key != null ? Identifier.FromString(match.Key) : air;
  }
}
