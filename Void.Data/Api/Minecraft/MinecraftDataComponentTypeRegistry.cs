using Void.Data.Minecraft.Registry;
using Void.Minecraft.Network;

namespace Void.Data.Api.Minecraft;

public static class MinecraftDataComponentTypeRegistry
{
  public static int GetId(ProtocolVersion protocolVersion, Identifier identifier)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return -1;

    var identifierString = identifier.ToString();

    if (registry.MinecraftDataComponentTypeRegistry.Entries.TryGetValue(identifierString, out var item))
      return item.ProtocolId;

    return -1;
  }

  public static Identifier? GetIdentifier(ProtocolVersion protocolVersion, int id)
  {
    var registry = MinecraftRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return null;

    var match = registry.MinecraftDataComponentTypeRegistry.Entries.FirstOrDefault(i => i.Value.ProtocolId == id);

    return match.Key != null ? Identifier.FromString(match.Key) : null;
  }
}
