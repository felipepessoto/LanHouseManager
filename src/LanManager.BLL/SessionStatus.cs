using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanManager.DAL;

namespace LanManager.BLL
{
    internal static class SessionStatus
    {
        internal static LanManager.DAL.Client LoggedClient { get; set; }

        internal static LanManager.DAL.ClientSession CurrentSession { get; set; }
    }
}
