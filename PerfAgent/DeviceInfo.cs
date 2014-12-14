using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace PerfAgent
{
    /// <summary>
    /// Class for getting device info from WMI.
    /// </summary>
    public class DeviceInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInfo"/> class.
        /// </summary>
        public DeviceInfo()
            : this(null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInfo"/> class.
        /// </summary>
        /// <param name="machineName">Name of the machine to connect.</param>
        public DeviceInfo(string machineName)
        {
            ManagementScope scope = null;
            if (!string.IsNullOrEmpty(machineName) && !string.Equals(machineName, "."))
            {
                scope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", machineName));
                scope.Connect();
            }
            ObjectQuery query = new ObjectQuery("SELECT Manufacturer, Model, Name FROM Win32_ComputerSystem ");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                foreach (var m in searcher.Get())
                {
                    Name = m["Name"].ToString();
                    Manufacturer = m["Manufacturer"].ToString();
                    Model = m["Model"].ToString();
                    m.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>
        /// The device name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the manufacturer name.
        /// </summary>
        /// <value>
        /// The manufacturer.
        /// </value>
        public string Manufacturer { get; private set; }

        /// <summary>
        /// Gets the model info.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public string Model { get; private set; }
    }
}
