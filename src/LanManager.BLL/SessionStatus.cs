using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LanManager.BLL
{
    internal static class SessionStatus
    {
        internal static Client LoggedClient { get; set; }

        internal static ClientSession CurrentSession { get; set; }
    }
}
