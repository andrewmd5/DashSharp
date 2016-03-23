using System;
using System.Threading.Tasks;
using DashSharp.Exceptions;
using DashSharp.Models;
using PacketDotNet;
using SharpPcap;

namespace DashSharp
{
    public class DashNetwork
    {
        private ICaptureDevice _device;
        private const int ReadTimeoutMilliseconds = 1000;
        public event EventHandler ListenerStarted;
        public event EventHandler DashButtonProbed;


        public void StartListening()
        {
            // Retrieve the device list
            try
            {
                var devices = CaptureDeviceList.Instance;
                if (devices.Count < 1)
                {
                    throw new PcapMissingException("No interfaces found! Make sure WinPcap is installed.");
                }
                //default to the first device
                _device = devices[0];
                if (_device != null)
                {
                    _device.OnPacketArrival += device_OnPacketArrival;   
                    _device.Open(DeviceMode.Promiscuous, ReadTimeoutMilliseconds);

                    // tcpdump filter to capture only ARP Packets
                    var filter = "arp";
                    _device.Filter = filter;
                    AsyncCapture();
                }
            }
            catch (Exception)
            {
                throw new PcapMissingException("No interfaces found! Make sure WinPcap is installed.");
            }
        }

        private void AsyncCapture()
        {
            Action action = _device.Capture;
            action.BeginInvoke(ar => action.EndInvoke(ar), null);
            ListenerStarted?.Invoke(this, new DashListenerResponse { Started = true });
        }

        /// <summary>
        ///     Prints the time and length of each received packet
        /// </summary>
        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            //dash wakeup sends a packet this long
            if (e.Packet.Data.Length == 60)
            {
                var dashMac = e.Device.MacAddress.ToString();
                var dashName = e.Device.Name;
                var dashId = e.Device.MacAddress.GetHashCode();
                var dashData = e.Packet.Data;
                DashButtonProbed?.Invoke(this, new DashResponse { DashName = dashName, DashMac = dashMac, DashId = dashId, DashData = dashData });
            }
            
        }

      
    }
}