using Livekit.Server.Sdk.Dotnet;
using LoreWeaver.Data;
using LoreWeaver.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LoreWeaver.Features.Comms;

/// <summary>
/// Only one game session runs at a time (5-person table, single campaign) - there's
/// no per-user access model to scope multiple concurrent sessions to, so this service
/// doesn't try to support that.
/// </summary>
public class GameSessionService(
    IDbContextFactory<LoreWeaverDbContext> dbFactory,
    RoomServiceClient roomServiceClient,
    IOptions<LiveKitOptions> options)
{
    public async Task<GameSession> CreateSessionAsync(string startedByName, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var active = await db.GameSessions.FirstOrDefaultAsync(s => s.EndedAt == null, ct);
        if (active is not null)
        {
            return active;
        }

        var roomName = $"session-{DateTimeOffset.UtcNow:yyyyMMdd-HHmmss}-{Guid.NewGuid().ToString("N")[..8]}";
        await roomServiceClient.CreateRoom(new CreateRoomRequest { Name = roomName });

        var session = new GameSession
        {
            Id = Guid.NewGuid(),
            RoomName = roomName,
            StartedByName = startedByName,
        };
        db.GameSessions.Add(session);
        await db.SaveChangesAsync(ct);
        return session;
    }

    public async Task<GameSession?> GetActiveSessionAsync(CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await db.GameSessions.FirstOrDefaultAsync(s => s.EndedAt == null, ct);
    }

    public async Task EndSessionAsync(Guid sessionId, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var session = await db.GameSessions.FindAsync([sessionId], ct)
            ?? throw new InvalidOperationException($"Game session {sessionId} not found.");

        if (session.EndedAt is not null) return;

        // Explicit teardown rather than relying solely on LiveKit's empty-room timeout -
        // see deploy/livekit/livekit.yaml.template for that backstop.
        await roomServiceClient.DeleteRoom(new DeleteRoomRequest { Room = session.RoomName });

        session.EndedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
    }

    /// <summary>Mints a room-scoped join token. No account system exists yet, so
    /// identity is derived from the display name the participant types in plus a
    /// random suffix - unique per connection, stable for nothing beyond one session.</summary>
    public string CreateParticipantToken(string roomName, string displayName)
    {
        var identity = $"{Slugify(displayName)}-{Guid.NewGuid().ToString("N")[..6]}";

        var token = new AccessToken(options.Value.ApiKey, options.Value.ApiSecret)
            .WithIdentity(identity)
            .WithName(displayName)
            .WithGrants(new VideoGrants
            {
                RoomJoin = true,
                Room = roomName,
                CanPublish = true,
                CanSubscribe = true,
                CanPublishData = false,
            });

        return token.ToJwt();
    }

    public string WebSocketUrl => options.Value.WebSocketUrl;

    private static string Slugify(string name)
    {
        var chars = name.Trim().ToLowerInvariant()
            .Select(c => char.IsLetterOrDigit(c) ? c : '-')
            .ToArray();
        var slug = string.Join('-', new string(chars).Split('-', StringSplitOptions.RemoveEmptyEntries));
        return string.IsNullOrEmpty(slug) ? "player" : slug;
    }
}
