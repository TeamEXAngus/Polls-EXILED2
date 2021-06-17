using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace Polls.Commands
{
    public class Vote
    {
        private static Polls Plugin => Polls.Instance;

        public void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if (ev.Name != "vote") { return; }

            if (Plugin.ActivePoll is null) { ev.ReturnMessage = "There is no currently active poll!"; return; }

            if (Plugin.ActivePoll.AlreadyVoted.Contains(ev.Player)) { ev.ReturnMessage = "You've already voted on this poll!"; return; }

            switch (ev.Arguments[0].ToLower())
            {
                case "yes":
                case "y":
                    Plugin.ActivePoll.Votes[0]++;
                    Plugin.ActivePoll.AlreadyVoted.Add(ev.Player);
                    break;

                case "no":
                case "n":
                    Plugin.ActivePoll.Votes[1]++;
                    Plugin.ActivePoll.AlreadyVoted.Add(ev.Player);
                    break;

                default:
                    break;
            }
        }
    }
}