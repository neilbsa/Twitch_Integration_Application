using System.Net;
using TwitchLive.Domain.Abstractions.Results;


namespace TwitchLive.Domain.Abstractions.Channels.Errors;
public static class ChannelErrors
{
    public static  Error ChannelNotFound = new Error(
        "Channel not found",
        "The specified channel could not be found.");

    public static Error AlreadyJoinedToChannel = new Error("Channel already joined", "You already joined to channel");
public static Error ChannelNotOnline = new Error("Channel not online","Invalid Operation. The channel is not online");
    public static  Error ChannelAlreadyExists = new Error("Channel already exists", "The specified channel already exists.");
    public static  Error ChannelNotJoined = new Error("Channel not joined", "The specified channel is not joined.");
    public static  Error ChannelNotMonitored = new Error("Channel not monitored", "The specified channel is not being monitored.");
        public static  Error TwitchChannelNotValid = new Error("Twitch Channel not Valid", "Twitch has no record of this channel id");
}