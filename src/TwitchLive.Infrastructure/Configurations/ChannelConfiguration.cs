using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitchLive.Domain.Channels;
using TwitchLive.Domain.Channels.Properties;

namespace TwitchLive.Infrastructure.Configurations;



public sealed class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.ToTable("channels");
        builder.HasKey(z=>z.Id);
        builder.Property(z=>z.UserTwitchId)
            .HasConversion(z=>z.Value, value => new UserTwitchId(value));
        builder.Property(z=>z.Login)
            .HasConversion(z=>z.Value, value => new UserTwitchLogin(value));
           builder.Property(z=>z.DisplayName)
            .HasConversion(z=>z.Value, value => new UserTwitchDisplayName(value));
            builder.Property(z=>z.Type)
                    .HasConversion(z=>z.Value, value => new UserTwitchType(value));
            builder.Property(z=>z.BroadcasterType)
                    .HasConversion(z=>z.Value, value => new UserTwitchBroadcasterType(value));
            builder.Property(z=>z.Description)
                    .HasConversion(z=>z.Value, value => new UserTwitchDescription(value));
            builder.Property(z=>z.ProfileImageUrl)
                    .HasConversion(z=>z.Value, value => new UserTwitchProfileImageUrl(value));
            builder.Property(z=>z.OfflineImageUrl)
                    .HasConversion(z=>z.Value, value => new UserTwitchOfflineImageUrl(value));
            builder.Property(z=>z.Email)
                    .HasConversion(z=>z.Value, value => new UserTwitchEmail(value));
            builder.Property(z=>z.ProfileImageUrl)
                    .HasConversion(z=>z.Value, value => new UserTwitchProfileImageUrl(value));
        
            builder.HasIndex(z=>z.Login);
            builder.HasIndex(z=>z.UserTwitchId);
    }
}