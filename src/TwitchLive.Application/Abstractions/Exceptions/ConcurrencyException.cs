namespace TwitchLive.Application.Abstractions.Exceptions;

public sealed class ConcurrencyException : Exception
{
    public ConcurrencyException(
        string Message,
        Exception? innerException) : base(Message,innerException)
    {
        
    }
}