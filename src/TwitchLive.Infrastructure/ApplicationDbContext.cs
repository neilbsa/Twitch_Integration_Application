using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TwitchLive.Application.Abstractions.Exceptions;
using TwitchLive.Domain.Abstractions.UnitOfWork;
using TwitchLive.Domain.Entities;
using Wolverine;

namespace TwitchLive.Infrastructure;


public sealed class ApplicationDbContext :DbContext,IUnitOfWork
{
    private readonly IMessageBus _publisher;
    public ApplicationDbContext(DbContextOptions opt, IMessageBus publisher)
    : base(opt)
    {
        _publisher = publisher;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      
      try
      {
        
        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;

      }catch(DbUpdateException ex)
      {
        throw new  ConcurrencyException("Concurrency exciption occured",ex);
      }
   
    }
    private async Task PublishDomainEventsAsync()
  {
      var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.PublishAsync(domainEvent);
        }
  }

}