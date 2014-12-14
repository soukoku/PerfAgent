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
            ObjectQuery query = new ObjectQuery("SELECT Manufacturer, Model, Name FROM Win32_ComputerSystem");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                foreach (var m in searcher.Get())
                {
                    DeviceName = m["Name"].ToString();
                    DeviceManufacturer = m["Manufacturer"].ToString();
                    DeviceModel = m["Model"].ToString();
                    m.Dispose();
                }
            }
            
            query = new ObjectQuery("SELECT Caption, Version FROM Win32_OperatingSystem");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                foreach (var m in searcher.Get())
                {
                    OSName = m["Caption"].ToString();
                    OSVersion = m["Version"].ToString();
                    m.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the name of the os.
        /// </summary>
        /// <value>
        /// The name of the os.
        /// </value>
        public string OSName { get; private set; }

        /// <summary>
        /// Gets the os version.
        /// </summary>
        /// <value>
        /// The os version.
        /// </value>
        public string OSVersion { get; private set; }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>
        /// The device name.
        /// </value>
        public string DeviceName { get; private set; }

        /// <summary>
        /// Gets the device manufacturer name.
        /// </summary>
        /// <value>
        /// The device manufacturer.
        /// </value>
        public string DeviceManufacturer { get; private set; }

        /// <summary>
        /// Gets the device model.
        /// </summary>
        /// <value>
        /// The device model.
        /// </value>
        public string DeviceModel { get; private set; }
    }
}
