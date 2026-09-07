using TwitchLive.Domain.Abstractions.Channels.Errors;
using TwitchLive.Domain.Abstractions.Results;
using TwitchLive.Domain.Channels.DomainEvents;
using TwitchLive.Domain.Channels.Properties;
using TwitchLive.Domain.Entities;

namespace TwitchLive.Domain.Channels;


public sealed class Channel :Entity
{
    private Channel(
        Guid id, UserTwitchId userTwitchId, UserTwitchLogin login, UserTwitchDisplayName displayName, DateTime createdAt,
        UserTwitchType type, UserTwitchBroadcasterType broadcasterType, UserTwitchDescription description,
        UserTwitchProfileImageUrl profileImageUrl, UserTwitchOfflineImageUrl offlineImageUrl, long viewCount,
        UserTwitchEmail email, ChannelStatus status) : base(id)
    {
        UserTwitchId = userTwitchId;
        Login = login;
        DisplayName = displayName;
        CreatedAt = createdAt;
        Type = type;
        BroadcasterType = broadcasterType;
        Description = description;
        ProfileImageUrl = profileImageUrl;
        OfflineImageUrl = offlineImageUrl;
        ViewCount = viewCount;
        Email = email;
        Status = status;
    }

    public UserTwitchId UserTwitchId { get; private set; }
    public UserTwitchLogin Login { get; private set; }
    public UserTwitchDisplayName DisplayName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public UserTwitchType Type { get; private set; }
    public UserTwitchBroadcasterType BroadcasterType { get; private set; }
    public UserTwitchDescription Description { get; private set; }
    public UserTwitchProfileImageUrl ProfileImageUrl { get; private set; }
    public UserTwitchOfflineImageUrl OfflineImageUrl { get; private set; }
    public long ViewCount { get; private set; }
    public UserTwitchEmail Email { get; private set; }
    public ChannelStatus Status { get; private set; }
    public JoinStatus JoinStatus { get; private set; }
    public static Channel Create(
        UserTwitchId userTwitchId,
        UserTwitchLogin login,
        UserTwitchDisplayName displayName,
        DateTime createdAt,
        UserTwitchType type,
        UserTwitchBroadcasterType broadcasterType,
        UserTwitchDescription description,
        UserTwitchProfileImageUrl profileImageUrl,
        UserTwitchOfflineImageUrl offlineImageUrl,
        long viewCount,
        UserTwitchEmail email,
        ChannelStatus status)
    {
        var channel = new Channel(
            Guid.NewGuid(),
            userTwitchId,
            login,
            displayName,
            createdAt,
            type,
            broadcasterType,
            description,
            profileImageUrl,
            offlineImageUrl,
            viewCount,
            email,
            status);
        channel.RaiseDomainEvent(new ChannelCreatedDomainEvent(channel));
        return channel;
    }


    public void JoinChannel()
    {

        RaiseDomainEvent(new JoinedToChannelDomainEvent(Id));
        JoinStatus = JoinStatus.Joined;
    }

    public Result CanSendChat()
    {
        if(Status != ChannelStatus.Live)
            return Result.Failure(ChannelErrors.ChannelNotOnline);

        if(JoinStatus != JoinStatus.Joined)
            return Result.Failure(ChannelErrors.ChannelNotJoined);
    
        return Result.Success();
    }


    public void SetChannelStatus(ChannelStatus status)
    {
        if(status != this.Status)
        {
            Status = status;
            RaiseDomainEvent(new ChannelStatusUpdateDomainEvent(Id,status));
        }
    }

    public void LeaveChannel()
    {

        RaiseDomainEvent(new JoinedToChannelDomainEvent(Id));
        JoinStatus = JoinStatus.Joined;
    }

}

