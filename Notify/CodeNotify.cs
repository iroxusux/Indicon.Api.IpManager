using Engine.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicon.Api.IpManager.Notify
{
    internal struct CodeNotify
    {
        public CodeNotify() { }
        internal static readonly NotifyMessage FileNotFound = new(1_000, "File Not Found", "Ip Manager File Could Not Be Found. If this is not expected, please contact the author of this tool. If you have NOT used this tool before, a file will be created as you create managed addresses.");
        internal static readonly NotifyMessage NoPingAdapter = new(1_001, "Cannot ping network!", "No valid IP address associated with selected Network Adapter. Please verify network is enabled and cable is plugged in!");
    }
}
