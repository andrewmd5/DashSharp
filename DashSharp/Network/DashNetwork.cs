#region

using System;
using DashSharp.Exceptions;
using DashSharp.Models;
using PacketDotNet;
using SharpPcap;

#endregion

namespace DashSharp
{
    public class DashNetwork
    {
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
                foreach (var device in devices)
                {
                    if (device == null) continue;
                    device.OnPacketArrival += delegate(object sender, CaptureEventArgs e)
                    {
                        var packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
                        var eth = (EthernetPacket) packet;
                        var dashMac = eth.SourceHwAddress.ToString();
                        if (dashMac.Equals(device.MacAddress.ToString()))
                        {
                            //ignore packets from our own device
                            return;
                        }
                        var dashId = dashMac.GetHashCode();

                        DashButtonProbed?.Invoke(this,
                            new DashResponse
                            {
                                DashMac = dashMac,
                                DashId = dashId,
                                Device = device.MacAddress.ToString()
                            });
                    };
                    device.Open(DeviceMode.Promiscuous, ReadTimeoutMilliseconds);
                    // tcpdump filter to capture only ARP Packets
                    device.Filter = "arp";
                    Action action = device.Capture;
                    action.BeginInvoke(ar => action.EndInvoke(ar), null);
                    ListenerStarted?.Invoke(this,
                        new DashListenerResponse
                        {
                            Started = true,
                            Message = $"Started listenr on {device.MacAddress}"
                        });
                }
            }
            catch (Exception)
            {
                throw new PcapMissingException("No interfaces found! Make sure WinPcap is installed.");
            }
        }
    }
}