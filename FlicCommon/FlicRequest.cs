using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicCommon
{
    public enum FlicAction
    {
        Unknown,
        Click,
        DoubleClick,
        Hold
    }

    public record FlicRequest(string Id, FlicAction Action, DateTimeOffset Timestamp)
    {
        public static FlicAction GetAction(string actionString) => Enum.TryParse<FlicAction>(actionString, out var act) ? act : FlicAction.Unknown;
    }
}
