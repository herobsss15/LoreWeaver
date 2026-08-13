namespace LoreWeaver.Features.Comms;

public class LiveKitOptions
{
    public const string SectionName = "LiveKit";

    /// <summary>wss:// URL the browser client connects to (public, via the tunnel).</summary>
    public required string WebSocketUrl { get; set; }

    /// <summary>http(s):// URL the server uses for room management calls. Can be an
    /// internal address (e.g. localhost) if the app and LiveKit share a host, since
    /// this traffic never needs to leave the server.</summary>
    public required string ApiUrl { get; set; }

    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}
