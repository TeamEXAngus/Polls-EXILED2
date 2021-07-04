using CommandSystem;
using Exiled.API.Features;
using System;

namespace Polls.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Vote : ICommand
    {
        private static Poll ActivePoll => PollsPlugin.Instance.ActivePoll;

        public string Command { get; } = "vote";

        public string[] Aliases { get; } = { };

        public string Description { get; } = "Creates a poll which server users can vote in.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var playerSender = Player.Get((sender as CommandSender)?.SenderId);

            if (ActivePoll is null) { response = "There is no currently active poll!"; return false; }
            if (ActivePoll.AlreadyVoted.Contains(playerSender)) { response = "You've already voted on this poll!"; return false; }

            switch (arguments.At(0))
            {
                case "yes":
                case "y":
                    ActivePoll.Votes[0]++;
                    ActivePoll.AlreadyVoted.Add(playerSender);
                    response = "Voted yes!";
                    return true;

                case "no":
                case "n":
                    ActivePoll.Votes[1]++;
                    ActivePoll.AlreadyVoted.Add(playerSender);
                    response = "Voted no!";
                    return true;

                default:
                    response = "Invalid option! Choose \"yes\" or \"no\"!";
                    return false;
            }
        }
    }
}