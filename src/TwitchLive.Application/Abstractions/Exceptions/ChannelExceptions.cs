namespace TwitchLive.Application.Abstractions.Exceptions;
public sealed class ChannelExceptions : Exception
{
    public ChannelExceptions(
        string Message,
        Exception? innerException) : base(Message,innerException)
    {
        
    }
}