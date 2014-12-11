using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PerfAgent.Counters
{
    public class Processor : Disposable
    {
        string _machine;
        string[] _instanceNames;
        PerformanceCounter[] _processorTimeCounters;

        public Processor(string machineName)
        {
            _machine = machineName;
            _instanceNames = new PerformanceCounterCategory("Processor", machineName).GetInstanceNames();
            _processorTimeCounters = new PerformanceCounter[_instanceNames.Length];
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var pc in _processorTimeCounters)
            {
                if (pc != null) { pc.Dispose(); }
            }
            base.Dispose(disposing);
        }

        public int Processors { get { return _processorTimeCounters.Length - 1; } }

        public float TotalProcessorTime
        {
            get
            {
                return GetTimeCounter(0).NextValue();
            }
        }

        public float GetProcessorTime(int processorNumber)
        {
            var idx = processorNumber + 1;
            return GetTimeCounter(idx).NextValue();
        }

        PerformanceCounter GetTimeCounter(int instanceIdx)
        {
            var pc = _processorTimeCounters[instanceIdx];
            if (pc == null)
            {
                _processorTimeCounters[instanceIdx] = pc =
                    new PerformanceCounter("Processor", "% Processor Time", _instanceNames[instanceIdx], _machine);
            }
            return pc;
        }
    }
}
