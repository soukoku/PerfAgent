using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PerfAgent.Counters
{
    /// <summary>
    /// Contains common performance counters from the Processor category.
    /// </summary>
    public class Processor : Disposable
    {
        string _machine;
        IList<string> _instanceNames;
        PerformanceCounter[] _processorTimeCounters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Processor"/> class.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        public Processor(string machineName)
        {
            _machine = machineName;
            _instanceNames = new PerformanceCounterCategory("Processor", machineName).GetInstanceNames()
                .OrderBy(n => n).ToArray();
            _processorTimeCounters = new PerformanceCounter[_instanceNames.Count];
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _processorTimeCounters.DisposeList();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the number of processors.
        /// </summary>
        /// <value>
        /// The number of processors.
        /// </value>
        public int Processors { get { return _processorTimeCounters.Length - 1; } }

        /// <summary>
        /// Gets the processor time over all processors.
        /// </summary>
        /// <value>
        /// The total processor time.
        /// </value>
        public float TotalProcessorTime
        {
            get
            {
                return GetTimeCounter(0).NextValue();
            }
        }

        /// <summary>
        /// Gets the processor time on a processor.
        /// </summary>
        /// <param name="processorNumber">The processor number.</param>
        /// <returns></returns>
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
