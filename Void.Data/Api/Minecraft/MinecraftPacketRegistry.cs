using Void.Minecraft.Network;
using Void.Proxy.Api.Network;

namespace Void.Data.Api.Minecraft;

public class MinecraftPacketRegistry
{
  public static int GetId(ProtocolVersion protocolVersion, Phase phase, Direction direction, Identifier identifier)
  {
    var registry = Void.Data.Minecraft.Packet.MinecraftPacketRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return -1;

    var directionRegistry = Void.Data.Minecraft.Packet.MinecraftPacketRegistry.GetDirectionRegistry(registry, phase);
    var packetRegistry = Void.Data.Minecraft.Packet.MinecraftPacketRegistry.GetPacketRegistry(directionRegistry, direction);
    if (packetRegistry == null)
      return -1;

    var identifierString = identifier.ToString();

    if (!packetRegistry.TryGetValue(identifierString, out var packet))
      return -1;

    return packet.ProtocolId;
  }

  public static Identifier? GetIdentifier(ProtocolVersion protocolVersion, Phase phase, Direction direction, int protocolId)
  {
    var registry = Void.Data.Minecraft.Packet.MinecraftPacketRegistry.GetRegistry(protocolVersion);
    if (registry == null)
      return null;

    var directionRegistry = Void.Data.Minecraft.Packet.MinecraftPacketRegistry.GetDirectionRegistry(registry, phase);
    var packetRegistry = Void.Data.Minecraft.Packet.MinecraftPacketRegistry.GetPacketRegistry(directionRegistry, direction);
    if (packetRegistry == null)
      return null;

    var match = packetRegistry.FirstOrDefault(i => i.Value.ProtocolId == protocolId);

    return match.Key != null ? Identifier.FromString(match.Key) : null;
  }
}
