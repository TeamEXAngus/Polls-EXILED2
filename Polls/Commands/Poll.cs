using CommandSystem;
using Exiled.Permissions.Extensions;
using System;

namespace Commands.StartPoll
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Poll : ICommand
    {
        public string Command { get; } = "poll";

        public string[] Aliases { get; } = { };

        public string Description { get; } = "Starts a poll.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            //Permission checking
            if (!(sender as CommandSender).CheckPermission("poll.poll"))
            {
                response = "You do not have permission to use ths command! Missing permission \"poll.poll\"";
                return false;
            }

            if (!(Polls.PollsPlugin.Instance.ActivePoll is null))
            {
                response = "There is an already active poll!";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "You must specify a message for the poll!";
                return false;
            }

            string message = "";

            foreach (var arg in arguments)
            {
                message += arg + " ";
            }

            Polls.PollsPlugin.Instance.ActivePoll = new Polls.Poll(message);

            response = $"Succesfully created poll: {message}";
            return true;
        }
    }
}