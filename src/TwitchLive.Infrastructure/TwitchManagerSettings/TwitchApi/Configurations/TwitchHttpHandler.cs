using System.Net.Http.Headers;
using System.Text;
using TwitchLib.Api.Core;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Core.Interfaces;

namespace TwitchLive.Infrastructure.TwitchManagerSettings.TwitchApi.Configurations;

public sealed class TwitchCustomHttpHandler : IHttpCallHandler
{
    private readonly HttpClient _httpClient;
    private readonly IRateLimitHandlers _limitHandlers;
    public TwitchCustomHttpHandler(IRateLimitHandlers limitHandlers,
        HttpClient? httpClient = null)
    {

        _httpClient = httpClient ?? new HttpClient();
        _limitHandlers = limitHandlers;
    }

    public async Task<KeyValuePair<int, string>> GeneralRequestAsync(
        string url,
        string method,
        string payload = null,
        ApiVersion api = ApiVersion.Helix,
        string clientId = null,
        string accessToken = null)
    {
        using var request = new HttpRequestMessage(
            new HttpMethod(method),
            url);

        AddHeaders(
            request,
            clientId,
            accessToken);

        if (!string.IsNullOrEmpty(payload))
        {
            request.Content = new StringContent(
                payload,
                Encoding.UTF8,
                "application/json");
        }

        using var response =
            await _httpClient.SendAsync(request);

        await UpdateRateLimitAsync(response);

        var responseBody =
            await response.Content.ReadAsStringAsync();

        return new KeyValuePair<int, string>(
            (int)response.StatusCode,
            responseBody);
    }

    public async Task PutBytesAsync(
        string url,
        byte[] payload)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Put,
            url);

        request.Content = new ByteArrayContent(payload);

        using var response =
            await _httpClient.SendAsync(request);

        await UpdateRateLimitAsync(response);

        response.EnsureSuccessStatusCode();
    }

    public async Task<int> RequestReturnResponseCodeAsync(
        string url,
        string method,
        List<KeyValuePair<string, string>> getParams = null)
    {
        var requestUrl = BuildUrl(url, getParams);

        using var request = new HttpRequestMessage(
            new HttpMethod(method),
            requestUrl);

        using var response =
            await _httpClient.SendAsync(request);

        await UpdateRateLimitAsync(response);

        return (int)response.StatusCode;
    }

    private static void AddHeaders(
        HttpRequestMessage request,
        string clientId,
        string accessToken)
    {
        if (!string.IsNullOrWhiteSpace(clientId))
        {
            request.Headers.TryAddWithoutValidation(
                "Client-Id",
                clientId);
        }

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    accessToken);
        }
    }

    private static string BuildUrl(
        string url,
        List<KeyValuePair<string, string>> getParams)
    {
        if (getParams == null || getParams.Count == 0)
            return url;

        var query = string.Join(
            "&",
            getParams.Select(x =>
                $"{Uri.EscapeDataString(x.Key)}=" +
                $"{Uri.EscapeDataString(x.Value)}"));

        return url.Contains("?")
            ? $"{url}&{query}"
            : $"{url}?{query}";
    }

    private async Task UpdateRateLimitAsync(
        HttpResponseMessage response)
    {
        var limit = GetHeaderInt(
            response,
            "Ratelimit-Limit");

        var remaining = GetHeaderInt(
            response,
            "Ratelimit-Remaining");

        var reset = GetHeaderLong(
            response,
            "Ratelimit-Reset");

        Console.WriteLine(
            $"Twitch API: {response.StatusCode} | " +
            $"Limit={limit} | " +
            $"Remaining={remaining} | " +
            $"Reset={reset}");

        if (limit.HasValue &&
            remaining.HasValue &&
            reset.HasValue)
        {

            var now = DateTimeOffset.UtcNow;
            var resetTime =
                DateTimeOffset.FromUnixTimeSeconds(
                    reset.Value);

          Console.WriteLine(
                $"Twitch API: Rate limit reset at {resetTime} UTC now: {now}  limit: {limit.Value}  remaining:{remaining.Value} ");



            _limitHandlers.UpdateRateLimit(limit.Value,remaining.Value,now - resetTime);
          
        }

        await Task.CompletedTask;
    }

    private static int? GetHeaderInt(
        HttpResponseMessage response,
        string name)
    {
        if (!response.Headers.TryGetValues(
                name,
                out var values))
        {
            return null;
        }

        return int.TryParse(
            values.FirstOrDefault(),
            out var result)
            ? result
            : null;
    }

    private static long? GetHeaderLong(
        HttpResponseMessage response,
        string name)
    {
        if (!response.Headers.TryGetValues(
                name,
                out var values))
        {
            return null;
        }

        return long.TryParse(
            values.FirstOrDefault(),
            out var result)
            ? result
            : null;
    }
}