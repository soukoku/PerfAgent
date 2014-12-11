using PerfAgent.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace PerfAgent
{
    public class Agent : Disposable
    {
        Timer _timer;
        bool _running;

        public Agent()
            : this(".")
        {
        }
        public Agent(string machineName)
        {
            _timer = new Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 1000;
            MachineName = machineName;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
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
                _timer.Enabled = _running;
            }
        }

        public string MachineName { get; private set; }

        private Processor _processor;
        public Processor Processor
        {
            get { return _processor ?? (_processor = new Processor(MachineName)); }
        }

        private Memory _memory;
        public Memory Memory
        {
            get { return _memory ?? (_memory = new Memory(MachineName)); }
        }

        private NetworkInterface _networkInterface;
        public NetworkInterface NetworkInterface
        {
            get { return _networkInterface ?? (_networkInterface = new NetworkInterface(MachineName)); }
        }

        private PhysicalDisk _physicalDisk;
        public PhysicalDisk PhysicalDisk
        {
            get { return _physicalDisk ?? (_physicalDisk = new Counters.PhysicalDisk(MachineName)); }
        }


        public TimeSpan Interval
        {
            get { return TimeSpan.FromMilliseconds(_timer.Interval); }
            set { _timer.Interval = value.TotalMilliseconds; }
        }

        public void Start()
        {
            _timer.Enabled = _running = true;
        }
        public void Stop()
        {
            _timer.Enabled = _running = false;
        }

        public event EventHandler Elapsed;
    }
}
