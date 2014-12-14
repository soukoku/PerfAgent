using PerfAgent.Counters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace PerfAgent
{
    /// <summary>
    /// Runner agent with timer and common counters.
    /// </summary>
    public class Agent : Disposable
    {
        Timer _timer;
        Stopwatch _watch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Agent"/> class.
        /// </summary>
        public Agent()
            : this(".")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Agent"/> class.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        public Agent(string machineName)
        {
            _timer = new Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 1000;
            _watch = new Stopwatch();
            MachineName = machineName;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _watch.Stop();
                _timer.Dispose();
                if (_processor != null) { _processor.Dispose(); }
                if (_memory != null) { _memory.Dispose(); }
                if (_networkInterface != null) { _networkInterface.Dispose(); }
            }
            base.Dispose(disposing);
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;
            try
            {
                var hand = Elapsed;
                if (hand != null) { hand(this, EventArgs.Empty); }
            }
            finally
            {
                _timer.Enabled = IsRunning;
            }
        }

        /// <summary>
        /// Gets the machine name used to create the agent.
        /// </summary>
        /// <value>
        /// The name of the machine.
        /// </value>
        public string MachineName { get; private set; }

        private DeviceInfo _deviceInfo;
        /// <summary>
        /// Gets the additional device information.
        /// </summary>
        /// <value>
        /// The device information.
        /// </value>
        public DeviceInfo DeviceInfo
        {
            get { return _deviceInfo ?? (_deviceInfo = new DeviceInfo(MachineName)); }
        }


        private Processor _processor;
        /// <summary>
        /// Gets the processor counter.
        /// </summary>
        /// <value>
        /// The processor counter.
        /// </value>
        public Processor Processor
        {
            get { return _processor ?? (_processor = new Processor(MachineName)); }
        }

        private Memory _memory;
        /// <summary>
        /// Gets the memory counter.
        /// </summary>
        /// <value>
        /// The memory counter.
        /// </value>
        public Memory Memory
        {
            get { return _memory ?? (_memory = new Memory(MachineName)); }
        }

        private NetworkInterface _networkInterface;
        /// <summary>
        /// Gets the network interface counter.
        /// </summary>
        /// <value>
        /// The network interface counter.
        /// </value>
        public NetworkInterface NetworkInterface
        {
            get { return _networkInterface ?? (_networkInterface = new NetworkInterface(MachineName)); }
        }

        private PhysicalDisk _physicalDisk;
        /// <summary>
        /// Gets the physical disk counter.
        /// </summary>
        /// <value>
        /// The physical disk.
        /// </value>
        public PhysicalDisk PhysicalDisk
        {
            get { return _physicalDisk ?? (_physicalDisk = new Counters.PhysicalDisk(MachineName)); }
        }


        /// <summary>
        /// Gets or sets the <see cref="Elapsed"/> event interval.
        /// </summary>
        /// <value>
        /// The timer interval.
        /// </value>
        public TimeSpan ElapsedInterval
        {
            get { return TimeSpan.FromMilliseconds(_timer.Interval); }
            set { _timer.Interval = value.TotalMilliseconds; }
        }

        /// <summary>
        /// Gets the time elapsed since <see cref="Start"/> has been called.
        /// </summary>
        /// <value>
        /// The time since start.
        /// </value>
        public TimeSpan TimeSinceStart { get { return _watch.Elapsed; } }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _watch.Reset();
            _watch.Start();
            _timer.Enabled = IsRunning = true;
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _watch.Stop();
            _timer.Enabled = IsRunning = false;
        }

        /// <summary>
        /// Occurs when the specified <see cref="ElapsedInterval"/> has elapsed.
        /// </summary>
        public event EventHandler Elapsed;
    }
}
