using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Polls
{
    public sealed class Config : IConfig
    {
        [Description("Whether or not the plugin is enabled on this server.")]
        public bool IsEnabled { get; set; } = true;

        [Description("The number of seconds which a poll will be displayed on top of players' screens.")]
        public ushort PollTextDuration { get; set; } = 5;

        [Description("The number of seconds which polls should last for.")]
        public int PollDuration { get; set; } = 30;

        [Description("Message that will be broadcast when a poll starts.")]
        public string PollStartedBroadcast { get; set; } = "Poll: {message}\nType \".vote yes\" or \".vote no\" in the console to vote!";

        [Description("Message that will be broadcast when a poll Ends.")]
        public string PollEndedBroadcast { get; set; } = "The poll has ended! {numYes} voted yes and {numNo} voted no!";
    }
}