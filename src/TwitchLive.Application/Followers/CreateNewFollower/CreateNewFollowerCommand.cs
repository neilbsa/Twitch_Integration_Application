using System.IO.Pipes;

namespace TwitchLive.Application.Followers.CreateNewFollower;


public record CreateNewFollowerCommand(FollowerDTO follower);
