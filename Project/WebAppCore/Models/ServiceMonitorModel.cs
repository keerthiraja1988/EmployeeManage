using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Models
{
    public class ServiceMonitorViewModel
    {
        public string ServiceName { get; set; }
        public Int64 ServiceId { get; set; }

        public HashSet<string> ConnectionIds { get; set; }
    }
}