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
        public override Version Version { get; } = new Version(1, 0, 9);

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

            PollName = name;
            Votes = new int[2] { 0, 0 };
            AlreadyVoted = new List<Player>();

            BroadcastToAllPlayers(BroadcastTime, PollsPlugin.Instance.Config.PollStartedBroadcast.Replace("{message}", PollName));
            EndPoll(PollDuration);
        }

        private void EndPoll(int delay)
        {
            ActiveCoro = Timing.CallDelayed(delay, () =>
            {
                BroadcastToAllPlayers(BroadcastTime, PollsPlugin.Instance.Config.PollEndedBroadcast.Replace("{numYes}", Votes[0].ToString()).Replace("{numNo}", Votes[1].ToString()));
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