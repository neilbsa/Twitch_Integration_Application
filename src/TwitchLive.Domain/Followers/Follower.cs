using TwitchLive.Domain.Entities;
using TwitchLive.Domain.Followers.DomainEvents;
using TwitchLive.Domain.Followers.Properties;
namespace TwitchLive.Domain.Followers;

public sealed class Follower : Entity
{
    private Follower(
        Guid id,
        FromUserId fromUserId,
        FromLogin fromLogin,
        FromUserName fromUserName,
        ToUserId toUserId,
        ToLogin toLogin,
        ToUserName toUserName,
        DateTime followedAt,
        RecognizeStatus recognition) : base(id)
    {
        FromUserId = fromUserId;
        FromLogin = fromLogin;
        FromUserName = fromUserName;
        ToUserId = toUserId;
        ToLogin = toLogin;
        ToUserName = toUserName;
        FollowedAt = followedAt;
        Recognition = recognition;
    }

    public FromUserId FromUserId { get; private set; }
    public FromLogin FromLogin { get; private set; }
    public FromUserName FromUserName { get; private set; }
    public ToUserId ToUserId { get; private set; }
    public ToLogin ToLogin { get; private set; }
    public ToUserName ToUserName { get; private set; }
    public DateTime FollowedAt { get; private set; }
    public RecognizeStatus Recognition { get; set; }
    public void SetRecognitionStatus(RecognizeStatus status)
    {
        Recognition = status;
        // RaiseDomainEvent(new RecognitionStatusChangedDomainEvent(Id));
    }

    public static Follower Create(FromUserId FromUserId,FromLogin FromLogin,FromUserName FromUserName,ToUserId ToUserId,ToLogin ToLogin, ToUserName ToUserName, DateTime DateTime)
    {
        var follower = new Follower(Guid.NewGuid(),FromUserId,FromLogin, FromUserName,ToUserId ,ToLogin , ToUserName ,  DateTime,RecognizeStatus.NotSent);
        follower.RaiseDomainEvent(new NewFollowerCreatedDomainEvent(follower.Id));
        return  follower;
    }
}
