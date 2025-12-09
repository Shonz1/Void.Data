using Void.Data.Minecraft.Registry;
using Void.Minecraft.Network;

namespace Void.Data.Api.Minecraft;

public class MinecraftMenuRegistry
{
  private static readonly Identifier inventory = "minecraft:inventory";
  private static readonly string container = "minecraft:container";

  public static int GetId(ProtocolVersion protocolVersion, Identifier identifier)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return -1;

    var identifierString = identifier.ToString();

    if (registry.MinecraftMenuRegistry.Entries.TryGetValue(identifierString, out var entry))
      return entry.ProtocolId;

    return -1;
  }

  public static string GetLegacyId(ProtocolVersion protocolVersion, Identifier identifier)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return container;

    var identifierString = identifier.ToString();

    if (registry.MinecraftMenuRegistry.Entries.TryGetValue(identifierString, out var entry))
      return entry.LegacyId ?? container;

    return container;
  }

  public static int GetSlotCount(ProtocolVersion protocolVersion, Identifier identifier)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return 0;

    var identifierString = identifier.ToString();

    return registry.MinecraftMenuRegistry.Entries.TryGetValue(identifierString, out var entry) ? entry.SlotCount : 0;
  }

  public static Identifier GetIdentifier(ProtocolVersion protocolVersion, int id, int slotCount = 0)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return inventory;

    var match = registry.MinecraftMenuRegistry.Entries.FirstOrDefault(i => i.Value.ProtocolId == id && i.Value.SlotCount == slotCount);

    return match.Key != null ? Identifier.FromString(match.Key) : inventory;
  }

  public static Identifier GetIdentifier(ProtocolVersion protocolVersion, string id, int slotCount)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return inventory;

    var match = registry.MinecraftMenuRegistry.Entries.FirstOrDefault(i => i.Value.LegacyId == id && i.Value.SlotCount == slotCount);

    return match.Key != null ? Identifier.FromString(match.Key) : inventory;
  }
}
