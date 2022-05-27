using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WwiseTools.Models.Profiler
{
    public class ProfilerRemoteInfo
    {
        public string Name { get; set; }
        public string Platform { get; set; }

        public string Host { get; set; }

        public string AppName { get; set; }

        public int CommandPort { get; set; }

        public int NotificationPort { get; set; }

        public override string ToString()
        {
            return $"{Name}-{AppName}-{Platform}@{Host}|{CommandPort}:{NotificationPort}";
        }
    }
}
