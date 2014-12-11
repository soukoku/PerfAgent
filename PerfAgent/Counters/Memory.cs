using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PerfAgent.Counters
{
    public class Memory : Disposable
    {
        PerformanceCounter _availableKB;
        PerformanceCounter _pagesPerSec;
        string _machine;

        public Memory(string machineName)
        {
            _machine = machineName;
            TotalBytes = (long)new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
        }

        protected override void Dispose(bool disposing)
        {
            if (_availableKB != null) { _availableKB.Dispose(); }
            if (_pagesPerSec != null) { _pagesPerSec.Dispose(); }

            base.Dispose(disposing);
        }

        public long TotalBytes { get; private set; }

        public float AvailableBytes
        {
            get
            {
                return (_availableKB ??
                    (_availableKB = new PerformanceCounter("Memory", "Available Bytes", string.Empty, _machine)))
                    .NextValue();
            }
        }

        public float PagesPerSec
        {
            get
            {
                return (_pagesPerSec ??
                    (_pagesPerSec = new PerformanceCounter("Memory", "Pages/sec", string.Empty, _machine)))
                    .NextValue();
            }
        }
    }
}
