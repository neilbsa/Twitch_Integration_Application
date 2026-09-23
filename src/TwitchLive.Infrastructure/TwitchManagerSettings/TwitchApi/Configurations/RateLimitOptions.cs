namespace TwitchLive.Infrastructure.TwitchManagerSettings.TwitchApi.Configurations;



public sealed class RateLimitDto
{
    public int MaxRequests { get; set; }
    public int Remaining { get; set; }
    public TimeSpan DateRefresh { get; set; }
}

public interface IRateLimitHandlers
{
    
    void UpdateRateLimit(int MaxRequest, int Remaining, TimeSpan ResetTime);
    RateLimitDto GetRateLimit();

}
public sealed class RateLimitHandlers : IRateLimitHandlers
{

    private RateLimitDto _rate {get;set;} = new RateLimitDto(){ DateRefresh= TimeSpan.FromMinutes(3), MaxRequests = 20, Remaining=20 };
    public RateLimitDto GetRateLimit()
    {
        return _rate;
    }

    public void UpdateRateLimit(int MaxRequest, int Remaining, TimeSpan ResetTime)
    {
        _rate = new RateLimitDto(){ DateRefresh= ResetTime, MaxRequests = MaxRequest, Remaining=Remaining };
    }
}

