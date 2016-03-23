using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DashSharp.Models
{
    public class DashResponse : EventArgs
    {
        public string DashName { get; set; }
        public string DashMac { get; set; }
        public int DashId { get; set; }
        public byte[] DashData { get; set; }
    }
}
