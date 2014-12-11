using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PerfAgent
{
    static class Util
    {
        public static void DisposeList(this IEnumerable<PerformanceCounter> list)
        {
            foreach (var pc in list)
            {
                if (pc != null) { pc.Dispose(); }
            }
        }
    }
}
