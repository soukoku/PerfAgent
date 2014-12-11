using Humanizer;
using PerfAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var agent = new Agent())
            {
                agent.Elapsed += agent_Elapsed;
                agent.Start();
                Console.ReadLine();
            }
        }

        static void agent_Elapsed(object sender, EventArgs e)
        {
            Console.Clear();

            var a = (Agent)sender;

            Console.WriteLine("Cpu: total {0:N2} % with {1} processors", a.Processor.TotalProcessorTime, a.Processor.Processors);
            for (int i = 0; i < a.Processor.Processors; i++)
            {
                Console.WriteLine("\tcpu{0}: {1:N2} %", i, a.Processor.GetProcessorTime(i));
            }
            
            Console.WriteLine("Memory: {0:N2} % free @ {1:N2} pages/s", 100 * (a.Memory.AvailableBytes / a.Memory.TotalBytes), a.Memory.PagesPerSec);

            Console.WriteLine("Network:");
            for (int i = 0; i < a.NetworkInterface.Names.Count; i++)
            {
                Console.WriteLine("\t{0}:\tRecv {1}/s\tSent {2}/s", a.NetworkInterface.Names[i], a.NetworkInterface.GetBytesReceivedPerSec(i).Bytes().Humanize("0.#"), a.NetworkInterface.GetBytesSentPerSec(i).Bytes().Humanize("0.#"));
            }

            Console.WriteLine("Disk:");
            for (int i = 0; i < a.PhysicalDisk.Names.Count; i++)
            {
                Console.WriteLine("\t{0}\t{1}/s\tAvg queue: {2:N2}", a.PhysicalDisk.Names[i], a.PhysicalDisk.GetBytesPerSec(i).Bytes().Humanize("0.#"), a.PhysicalDisk.GetAvgQueueLength(i));
            }
        }
    }
}
