using Exiled.API.Features;
using System;
using System.Collections.Generic;
using MEC;

namespace Polls
{
    public class PollsPlugin : Plugin<Config>
    {
        public static PollsPlugin Instance { get; } = new PollsPlugin();

        public override Version RequiredExiledVersion { get; } = new Version(2, 10, 0);
        public override Version Version { get; } = new Version(1, 0, 8);

        public Poll ActivePoll = null;

        private PollsPlugin()
        { }

        public override void OnEnabled()
        {
        }

        public override void OnDisabled()
        {
            if (!(ActivePoll is null)) { Timing.KillCoroutines(ActivePoll.ActiveCoro); }
        }
    }

    public class Poll
    {
        public string PollName;
        public int[] Votes;
        public List<Player> AlreadyVoted;
        public CoroutineHandle ActiveCoro;
        private ushort BroadcastTime;
        private int PollDuration;

        public Poll(string name)
        {
            BroadcastTime = PollsPlugin.Instance.Config.PollTextDuration;
            PollDuration = PollsPlugin.Instance.Config.PollDuration;

            Log.Debug($"New Poll created! BroadcastTime is {BroadcastTime} and PollDuration is {PollDuration}");
            Log.Debug($"Config.PollTextDuration is {PollsPlugin.Instance.Config.PollTextDuration}");
            Log.Debug($"Config.PollDuration is {PollsPlugin.Instance.Config.PollDuration}");

            PollName = name;
            Votes = new int[2] { 0, 0 };
            AlreadyVoted = new List<Player>();

            BroadcastToAllPlayers(BroadcastTime, $"Poll: {PollName}\nType \".vote yes\" or \".vote no\" in the console to vote!");
            EndPoll(PollDuration);
        }

        private void EndPoll(int delay)
        {
            ActiveCoro = Timing.CallDelayed(delay, () =>
            {
                BroadcastToAllPlayers(BroadcastTime, $"The poll has ended! {Votes[0]} voted yes and {Votes[1]} voted no!");
                PollsPlugin.Instance.ActivePoll = null;
            });
        }

        private void BroadcastToAllPlayers(ushort time, string message)
        {
            foreach (var player in Player.List)
            {
                player.ClearBroadcasts();
                player.Broadcast(time, message);
            }
        }
    }
}