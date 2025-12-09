using System.Text.Json.Serialization;

namespace Void.Data.Minecraft.Registry;

internal class MinecraftMenu
{
  [JsonPropertyName("protocol_id")]
  public int ProtocolId { get; init; }

  [JsonPropertyName("legacy_id")]
  public string? LegacyId { get; init; }

  [JsonPropertyName("slot_count")]
  public int SlotCount { get; init; }
}
