using TwitchLive.Domain.Abstractions.UnitOfWork;
using TwitchLive.Domain.Followers;
using TwitchLive.Domain.Followers.Properties;
using TwitchLive.Domain.Followers.Repository;

namespace TwitchLive.Application.Followers.CreateNewFollower;

public sealed class CreateNewFollowerCommandHandler
{

    private readonly IFollowerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateNewFollowerCommandHandler(IFollowerRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateNewFollowerCommand command, CancellationToken token)
    {

        var isExist = await _repository.IsFollowerAlreadyExist(new FromUserId(command.follower.FromUserId), new ToUserId(command.follower.ToUserId));
        if (!isExist)
        {

            DateTime followatDate = DateTimeOffset.Parse(command.follower.FollowedAt).DateTime;
               var follower =  Follower.Create(
                    new FromUserId(command.follower.FromUserId),
                    new FromLogin(command.follower.FromLogin),
                    new FromUserName(command.follower.FromUserName),
                    new ToUserId(command.follower.ToUserId),
                    new ToLogin(command.follower.ToLogin),
                    new ToUserName(command.follower.ToUserName),
                   followatDate);


                _repository.Add(follower);
                await _unitOfWork.SaveChangesAsync(token);  
        }
         


        
     

    }
}