using System;
using System.Collections.Generic;
using System.Text;

namespace FloridSword.SystemService.Configuration.Configurators.DnsMasq
{
    internal class DnsMasqConfiguration : ConfigFile
    {
        protected override string GetFileName() => "20-florid_sword";
        public List<DhcpRange> DhcpRanges { get; set; } = new List<DhcpRange>();
        public List<string> Interfaces { get; set; } = new List<string>();

        public override string ToString()
        {
            StringBuilder configBuilder = new StringBuilder();

            foreach (string networkInterface in Interfaces)
            {
                configBuilder.AppendLine($"interface={networkInterface}");
            }

            foreach (DhcpRange dhcpRange in DhcpRanges)
            {
                configBuilder.AppendLine(dhcpRange.ToString());
            }

            return configBuilder.ToString();
        }

        public void AddInterface(string networkInterface)
        {
            Interfaces.Add(networkInterface);
        }

        public void AddDhcpRange(string networkInterface, string startAddress, string endAddress, TimeSpan leaseTime)
        {
            DhcpRanges.Add(new DhcpRange
            {
                Interface = networkInterface,
                StartAddress = startAddress,
                EndAddress = endAddress,
                LeaseTime = leaseTime
            });
        }
    }

    internal class DhcpRange
    {
        public string Interface { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public TimeSpan LeaseTime { get; set; }

        public override string ToString() => $"dhcp-range={Interface},{StartAddress},{EndAddress},{GetLeaseTimeText(LeaseTime)}";

        private string GetLeaseTimeText(TimeSpan leaseTime)
        {
            if (leaseTime.Seconds != 0)
            {
                return $"{leaseTime.TotalSeconds:0}s";
            }

            if (leaseTime.Minutes != 0)
            {
                return $"{leaseTime.TotalMinutes:0}m";
            }

            return $"{leaseTime.TotalHours:0}h";
        }
    }
}
