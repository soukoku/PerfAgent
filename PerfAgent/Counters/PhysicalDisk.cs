using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PerfAgent.Counters
{
    public class PhysicalDisk : Disposable
    {
        string _machine;
        IList<string> _fullInstanceNames;
        IList<string> _dispInstanceNames;
        PerformanceCounter[] _bytesPerSecCounters;
        PerformanceCounter[] _avgQueueCounters;

        public PhysicalDisk(string machineName)
        {
            _machine = machineName;
            _fullInstanceNames = new PerformanceCounterCategory("PhysicalDisk", machineName).GetInstanceNames()
                .Where(n => n != "_Total").OrderBy(n => n).ToArray();
            _dispInstanceNames = _fullInstanceNames.Select(n => n.Substring(2)).ToArray();
            _bytesPerSecCounters = new PerformanceCounter[_fullInstanceNames.Count];
            _avgQueueCounters = new PerformanceCounter[_fullInstanceNames.Count];
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bytesPerSecCounters.DisposeList();
                _avgQueueCounters.DisposeList();
            }
            base.Dispose(disposing);
        }

        public IList<string> Names { get { return _dispInstanceNames; } }

        public double GetBytesPerSec(int diskNumber)
        {
            return GetBytesCounter(diskNumber).NextValue();
        }

        PerformanceCounter GetBytesCounter(int instanceIdx)
        {
            var pc = _bytesPerSecCounters[instanceIdx];
            if (pc == null)
            {
                _bytesPerSecCounters[instanceIdx] = pc =
                    new PerformanceCounter("PhysicalDisk", "Disk Bytes/sec", _fullInstanceNames[instanceIdx], _machine);
            }
            return pc;
        }

        public float GetAvgQueueLength(int diskNumber)
        {
            return GetQueueCounter(diskNumber).NextValue();
        }

        PerformanceCounter GetQueueCounter(int instanceIdx)
        {
            var pc = _avgQueueCounters[instanceIdx];
            if (pc == null)
            {
                _avgQueueCounters[instanceIdx] = pc =
                    new PerformanceCounter("PhysicalDisk", "Avg. Disk Queue Length", _fullInstanceNames[instanceIdx], _machine);
            }
            return pc;
        }
    }
}
