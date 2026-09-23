using Microsoft.Extensions.Options;
using TwitchLib.Api.Core.Interfaces;
using TwitchLive.Infrastructure.TwitchManagerSettings;

namespace TwitchLive.Infrastructure.TwitchManagerSettings.TwitchApi.Configurations;




public class TwitchRateLimit : IRateLimiter
{
    // private readonly int MaxRequests = 500;

    // private readonly TimeSpan _interval = TimeSpan.FromMinutes(.4);

    private readonly IRateLimitHandlers _limiter;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private readonly Queue<DateTimeOffset> _requests = new();

    public TwitchRateLimit(IRateLimitHandlers limiter)
    {
        _limiter = limiter;
    }




    public async Task Perform(
        Func<Task> perform,
        CancellationToken cancellationToken)
    {
        await WaitAsync(cancellationToken);

        await perform();
    }
    public async Task Perform(Func<Task> perform)
    {
        await WaitAsync(CancellationToken.None);

        await perform();
    }
    public async Task<T> Perform<T>(
        Func<Task<T>> perform)
    {
        await WaitAsync(CancellationToken.None);

        return await perform();
    }

    public async Task<T> Perform<T>(
        Func<Task<T>> perform,
        CancellationToken cancellationToken)
    {
        await WaitAsync(cancellationToken);

        return await perform();
    }

    public async Task Perform(Action perform)
    {
        await WaitAsync(CancellationToken.None);

        perform();
    }

    public async Task Perform(
        Action perform,
        CancellationToken cancellationToken)
    {
        await WaitAsync(cancellationToken);

        perform();
    }

    public async Task<T> Perform<T>(Func<T> perform)
    {
        await WaitAsync(CancellationToken.None);

        return perform();
    }

    public async Task<T> Perform<T>(
        Func<T> perform,
        CancellationToken cancellationToken)
    {
        await WaitAsync(cancellationToken);

        return perform();
    }
    private async Task WaitAsync(CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);

        try
        {
            var now = DateTimeOffset.UtcNow;
            var limiter = _limiter.GetRateLimit();
           // Remove requests outside the 1-minute window
           while (_requests.Count > 0 && now - _requests.Peek() >= limiter.DateRefresh)
            {
                _requests.Dequeue();
            }

            if (_requests.Count >= limiter.MaxRequests)
            {

                Console.WriteLine("Rate limit reached. Waiting for the next available slot...");
                var oldestRequest = _requests.Peek();

                var waitTime =
                    (limiter.DateRefresh - (now - oldestRequest));

                if (waitTime > TimeSpan.Zero)
                {
                    // Release lock while waiting
                    _lock.Release();

                    try
                    {
                        await Task.Delay(
                            waitTime,
                            cancellationToken);
                    }
                    finally
                    {
                        await _lock.WaitAsync(
                            cancellationToken);
                    }

                    // Re-check after waiting
                    now = DateTimeOffset.UtcNow;

                    while (_requests.Count > 0 &&
                           now - _requests.Peek() >= limiter.DateRefresh)
                    {
                        _requests.Dequeue();
                    }
                }
            }
            Console.WriteLine($"Performing request at {now}. Requests in the last minute: {_requests.Count + 1}");
            _requests.Enqueue(DateTimeOffset.UtcNow);
        }
        finally
        {
            _lock.Release();
        }
    }
}