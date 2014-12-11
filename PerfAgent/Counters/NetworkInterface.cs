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
        string[] _instanceNames;
        PerformanceCounter[] _receivedCounters;
        PerformanceCounter[] _sentCounters;

        public NetworkInterface(string machineName)
        {
            _machine = machineName;
            _instanceNames = new PerformanceCounterCategory("Network Interface", machineName).GetInstanceNames();
            _receivedCounters = new PerformanceCounter[_instanceNames.Length];
            _sentCounters = new PerformanceCounter[_instanceNames.Length];
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var pc in _receivedCounters)
            {
                if (pc != null) { pc.Dispose(); }
            }
            foreach (var pc in _sentCounters)
            {
                if (pc != null) { pc.Dispose(); }
            }
            base.Dispose(disposing);
        }

        public IList<string> Names { get { return _instanceNames; } }

        public float GetBytesReceivedPerSec(int networkNumber)
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

        public float GetBytesSentPerSec(int networkNumber)
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
    }
}
