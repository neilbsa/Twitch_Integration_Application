namespace TwitchLive.Domain.Abstractions.UnitOfWork;



public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken token);
} 