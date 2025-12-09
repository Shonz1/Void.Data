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

    if (!registry.MinecraftItemRegistry.Entries.TryGetValue(identifierString, out var item))
      item = registry.MinecraftItemRegistry.Entries[registry.MinecraftItemRegistry.Default];

    return item.ProtocolId;
  }

  public static int GetMeta(ProtocolVersion protocolVersion, Identifier identifier)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return 0;

    var identifierString = identifier.ToString();

    if (!registry.MinecraftItemRegistry.Entries.TryGetValue(identifierString, out var item))
      item = registry.MinecraftItemRegistry.Entries[registry.MinecraftItemRegistry.Default];

    return item.Meta;
  }

  public static Identifier GetIdentifier(ProtocolVersion protocolVersion, int itemId, int meta = 0)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return air;

    var match = registry.MinecraftItemRegistry.Entries.FirstOrDefault(i => i.Value.ProtocolId == itemId);

    return match.Key != null ? Identifier.FromString(match.Key) : air;
  }
}
