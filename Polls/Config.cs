using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}