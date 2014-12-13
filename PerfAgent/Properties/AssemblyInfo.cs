using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PerfAgent")]
[assembly: AssemblyDescription("Wraps common performance counters for easy use.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Yin-Chun Wang")]
[assembly: AssemblyProduct("PerfAgent")]
[assembly: AssemblyCopyright("Copyright © Yin-Chun Wang 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("fc3d776f-f497-4c26-9d0a-f605861ece02")]

[assembly: CLSCompliant(true)]

[assembly: AssemblyVersion(PerfAgent.Consts.Release)]
[assembly: AssemblyFileVersion(PerfAgent.Consts.Build)]
[assembly: AssemblyInformationalVersion(PerfAgent.Consts.Build)]

namespace PerfAgent
{
   static class Consts
    {
        public const string Release = "1.0.0.0"; // keep this same in major (breaking) releases

        public const string Build = "1.1.0.0"; // change this for each nuget release
    }
}