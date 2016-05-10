using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashSharp;
using DashSharp.Exceptions;
using DashSharp.Models;

namespace DashExample
{
    class Program
    {
      
        private static void Main(string[] args)
        {
            Console.Title = "DashSharp - Amazon Dash Button";
            Console.WriteLine("Dash Buttons have two MAC addresses, their wakeup and their pair. The last one you receive is the pair address. ");
            var network = new DashNetwork();
            network.ListenerStarted += network_ListenerStarted;
            network.DashButtonProbed += network_DashProbed;
            try
            {
                network.StartListening();
            }
            catch (PcapMissingException)
            {
                Console.WriteLine("No Pcap is missing, please install it.");
            }
            Console.Read();
        }

        private static void network_DashProbed(object sender, EventArgs e)
        {
            var probe = (DashResponse)e;
            Console.WriteLine("Amazon Dash Connected: " + probe.DashMac + " seen on " + probe.Device);
         
        }

        private static void network_ListenerStarted(object sender, EventArgs e)
        {
            Console.WriteLine(((DashListenerResponse)e).Message);
        }
    }
}