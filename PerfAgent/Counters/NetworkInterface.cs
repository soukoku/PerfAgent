using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PerfAgent.Counters
{
    public class NetworkInterface : Disposable
    {
        string _machine;
        IList<string> _instanceNames;
        PerformanceCounter[] _receivedCounters;
        PerformanceCounter[] _sentCounters;
        PerformanceCounter[] _outQueueCounters;

        public NetworkInterface(string machineName)
        {
            _machine = machineName;
            _instanceNames = new PerformanceCounterCategory("Network Interface", machineName).GetInstanceNames()
                .Where(n => !n.StartsWith("isatap", StringComparison.OrdinalIgnoreCase)).ToArray();
            _receivedCounters = new PerformanceCounter[_instanceNames.Count];
            _sentCounters = new PerformanceCounter[_instanceNames.Count];
            _outQueueCounters = new PerformanceCounter[_instanceNames.Count];
        }

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

        public IList<string> Names { get { return _instanceNames; } }

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
