using Engine.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicon.Api.IpManager.Notify
{
    internal struct CodeFatal
    {
        public CodeFatal() { }
        internal static readonly NotifyMessage NullFormError = new(10_000, "Null Form Error", "Null Form Was Passed To Ip Manager, Cannot Continue!");
    }
}
