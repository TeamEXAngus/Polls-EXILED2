using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using MEC;

namespace Polls
{
    public class Polls : Plugin<Config>
    {
        private static Polls singleton = new Polls();
        public static Polls Instance => singleton;

        public override PluginPriority Priority { get; } = PluginPriority.Medium;

        public override Version RequiredExiledVersion { get; } = new Version(2, 10, 0);
        public override Version Version { get; } = new Version(1, 0, 4);

        public Poll ActivePoll = null;

        public Polls()
        { }

        public override void OnEnabled()
        { }

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
        private readonly ushort BroadcastTime = Polls.Instance.Config.PollTextDuration;
        private readonly int PollDuration = Polls.Instance.Config.PollDuration;

        public Poll(string name)
        {
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
                Polls.Instance.ActivePoll = null;
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