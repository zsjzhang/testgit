using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Domain
{
    public static class FlowNo
    {
        public static string GetFlowNo()
        {
            Int64 seed = Int64.Parse(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString("0"));
            return Interlocked.Increment(ref seed) + string.Empty;
        }
    }
}
