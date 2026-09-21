using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitchLive.Domain.Followers;
using TwitchLive.Domain.Followers.Properties;

namespace TwitchLive.Infrastructure.Configurations;

public sealed class FollowerConfiguration : IEntityTypeConfiguration<Follower>
{
    public void Configure(EntityTypeBuilder<Follower> builder)
    {
        builder.ToTable("followers");
        builder.HasKey(z=>z.Id);
        builder.Property(z=>z.FromUserId)
            .HasConversion(z=>z.Value, value => new FromUserId(value));
              builder.Property(z=>z.FromLogin)
            .HasConversion(z=>z.Value, value => new FromLogin(value));
         builder.Property(z=>z.FromUserName)
            .HasConversion(z=>z.Value, value => new FromUserName(value));
         builder.Property(z=>z.ToUserId)
            .HasConversion(z=>z.Value, value => new ToUserId(value));
         builder.Property(z=>z.ToLogin)
            .HasConversion(z=>z.Value, value => new ToLogin(value));
         builder.Property(z=>z.ToUserName)
            .HasConversion(z=>z.Value, value => new ToUserName(value));
    }
}