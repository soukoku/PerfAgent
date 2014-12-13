using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PerfAgent.Counters
{
    /// <summary>
    /// Contains common performance counters from the Memory category.
    /// </summary>
    public class Memory : Disposable
    {
        PerformanceCounter _availableKB;
        PerformanceCounter _pagesPerSec;
        PerformanceCounter _curProcCounter;
        string _machine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Memory"/> class.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        public Memory(string machineName)
        {
            _machine = machineName;
            TotalBytes = (long)new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_availableKB != null) { _availableKB.Dispose(); }
                if (_pagesPerSec != null) { _pagesPerSec.Dispose(); }
                if (_curProcCounter != null) { _curProcCounter.Dispose(); }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the total memory in bytes.
        /// </summary>
        /// <value>
        /// The total bytes.
        /// </value>
        public long TotalBytes { get; private set; }

        /// <summary>
        /// Gets the available memory in bytes.
        /// </summary>
        /// <value>
        /// The available bytes.
        /// </value>
        public double AvailableBytes
        {
            get
            {
                return (_availableKB ??
                    (_availableKB = new PerformanceCounter("Memory", "Available Bytes", string.Empty, _machine)))
                    .NextValue();
            }
        }


        /// <summary>
        /// Gets the private working set used by current process.
        /// </summary>
        /// <value>
        /// The current process's private working set bytes.
        /// </value>
        public double CurrentProcessBytes
        {
            get
            {
                if (_curProcCounter == null)
                {
                    using (var proc = Process.GetCurrentProcess())
                    {
                        _curProcCounter = new PerformanceCounter("Process", "Working Set - Private", proc.ProcessName, _machine);
                    }
                }
                return _curProcCounter.NextValue();
            }
        }

        /// <summary>
        /// Gets the pages per sec.
        /// </summary>
        /// <value>
        /// The pages per sec.
        /// </value>
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
