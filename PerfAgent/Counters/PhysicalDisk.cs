using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PerfAgent.Counters
{
    /// <summary>
    /// Contains common performance counters from the PhysicalDisk category.
    /// </summary>
    public class PhysicalDisk : Disposable
    {
        string _machine;
        IList<string> _fullInstanceNames;
        IList<string> _dispInstanceNames;
        PerformanceCounter[] _bytesPerSecCounters;
        PerformanceCounter[] _avgQueueCounters;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalDisk"/> class.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        public PhysicalDisk(string machineName)
        {
            _machine = machineName;
            _fullInstanceNames = new PerformanceCounterCategory("PhysicalDisk", machineName).GetInstanceNames()
                .Where(n => n != "_Total").OrderBy(n => n).ToArray();
            _dispInstanceNames = _fullInstanceNames.Select(n => n.Substring(2)).ToArray();
            _bytesPerSecCounters = new PerformanceCounter[_fullInstanceNames.Count];
            _avgQueueCounters = new PerformanceCounter[_fullInstanceNames.Count];
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bytesPerSecCounters.DisposeList();
                _avgQueueCounters.DisposeList();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the names of the disks.
        /// </summary>
        /// <value>
        /// The names.
        /// </value>
        public IList<string> Names { get { return _dispInstanceNames; } }

        /// <summary>
        /// Gets the bytes per sec for a disk.
        /// </summary>
        /// <param name="diskNumber">The disk number.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the average length of the queue for a disk.
        /// </summary>
        /// <param name="diskNumber">The disk number.</param>
        /// <returns></returns>
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
