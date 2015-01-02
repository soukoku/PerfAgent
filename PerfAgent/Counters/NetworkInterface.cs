using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PerfAgent.Counters
{
    /// <summary>
    /// Contains common performance counters from the NetworkInterface category.
    /// </summary>
    public class NetworkInterface : Disposable
    {
        string _machine;
        IList<string> _instanceNames;
        PerformanceCounter[] _receivedCounters;
        PerformanceCounter[] _sentCounters;
        PerformanceCounter[] _outQueueCounters;
        PerformanceCounter[] _bandwidthCounters;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkInterface"/> class.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        public NetworkInterface(string machineName)
        {
            _machine = machineName;
            _instanceNames = new PerformanceCounterCategory("Network Interface", machineName).GetInstanceNames()
                .Where(n => !(n.StartsWith("isatap", StringComparison.OrdinalIgnoreCase) || n.Contains("Loopback") || n.Contains("Pseudo"))).ToArray();
            _receivedCounters = new PerformanceCounter[_instanceNames.Count];
            _sentCounters = new PerformanceCounter[_instanceNames.Count];
            _bandwidthCounters = new PerformanceCounter[_instanceNames.Count];
            _outQueueCounters = new PerformanceCounter[_instanceNames.Count];
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _receivedCounters.DisposeList();
                _sentCounters.DisposeList();
                _outQueueCounters.DisposeList();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the interface names.
        /// </summary>
        /// <value>
        /// The names.
        /// </value>
        public IList<string> Names { get { return _instanceNames; } }

        /// <summary>
        /// Gets the bytes received per sec for an interface.
        /// </summary>
        /// <param name="networkNumber">The network number.</param>
        /// <returns></returns>
        public double GetBytesReceivedPerSec(int networkNumber)
        {
            return GetReceiveCounter(networkNumber).NextValue();
        }

        PerformanceCounter GetReceiveCounter(int instanceIdx)
        {
            var pc = _receivedCounters[instanceIdx];
            if (pc == null)
            {
                _receivedCounters[instanceIdx] = pc =
                    new PerformanceCounter("Network Interface", "Bytes Received/sec", _instanceNames[instanceIdx], _machine);
            }
            return pc;
        }

        /// <summary>
        /// Gets the bytes sent per sec for an interface.
        /// </summary>
        /// <param name="networkNumber">The network number.</param>
        /// <returns></returns>
        public double GetBytesSentPerSec(int networkNumber)
        {
            return GetSentCounter(networkNumber).NextValue();
        }

        PerformanceCounter GetSentCounter(int instanceIdx)
        {
            var pc = _sentCounters[instanceIdx];
            if (pc == null)
            {
                _sentCounters[instanceIdx] = pc =
                    new PerformanceCounter("Network Interface", "Bytes Sent/sec", _instanceNames[instanceIdx], _machine);
            }
            return pc;
        }

        /// <summary>
        /// Gets the bandwidth in bytes for an interface.
        /// </summary>
        /// <param name="networkNumber">The network number.</param>
        /// <returns></returns>
        public double GetBandwidth(int networkNumber)
        {
            return GetGetBandwidthCounter(networkNumber).NextValue() / 8;
        }

        PerformanceCounter GetGetBandwidthCounter(int instanceIdx)
        {
            var pc = _bandwidthCounters[instanceIdx];
            if (pc == null)
            {
                _bandwidthCounters[instanceIdx] = pc =
                    new PerformanceCounter("Network Interface", "Current Bandwidth", _instanceNames[instanceIdx], _machine);
            }
            return pc;
        }

        /// <summary>
        /// Gets the length of the output queue for an interface.
        /// </summary>
        /// <param name="networkNumber">The network number.</param>
        /// <returns></returns>
        public float GetOutputQueueLength(int networkNumber)
        {
            return GetOutQueueCounter(networkNumber).NextValue();
        }

        PerformanceCounter GetOutQueueCounter(int instanceIdx)
        {
            var pc = _outQueueCounters[instanceIdx];
            if (pc == null)
            {
                _outQueueCounters[instanceIdx] = pc =
                    new PerformanceCounter("Network Interface", "Output Queue Length", _instanceNames[instanceIdx], _machine);
            }
            return pc;
        }
    }
}
