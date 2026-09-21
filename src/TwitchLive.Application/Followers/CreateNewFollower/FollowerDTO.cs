namespace TwitchLive.Application.Followers.CreateNewFollower;



public sealed class FollowerDTO{
    public string FromUserId { get;  set; }
    public string FromLogin { get;  set; }
    public string FromUserName { get;  set; }
    public string ToUserId { get;  set; }
    public string ToLogin { get;  set; }
    public string ToUserName { get;  set; }
    public string FollowedAt { get;  set; }
}