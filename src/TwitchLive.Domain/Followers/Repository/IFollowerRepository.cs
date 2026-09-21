using TwitchLive.Domain.Followers.Properties;

namespace TwitchLive.Domain.Followers.Repository;


public interface IFollowerRepository
{
    void Add(Follower follower);
    Task<Follower?> GetFollowerByIdAsync(Guid id);

    Task<bool> IsFollowerAlreadyExist(FromUserId fromUserId, ToUserId toUserId);
}