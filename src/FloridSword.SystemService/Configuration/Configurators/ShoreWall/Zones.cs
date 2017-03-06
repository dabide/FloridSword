using System.Collections.Generic;
using System.Text;

namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal class Zones : ShoreWallFile
    {
        public List<ZoneEntry> ZoneEntries { get; set; } = new List<ZoneEntry>();

        public void Add(string name)
        {
            ZoneEntries.Add(new ZoneEntry
            {
                Zone = name,
                Type = "ipv4"
            });
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("#ZONE\tTYPE\tOPTIONS\tIN OPTIONS\tOUT OPTIONS");
            stringBuilder.AppendLine("fw\tfirewall");

            foreach (ZoneEntry zone in ZoneEntries)
            {
                stringBuilder.AppendLine(zone.ToString());
            }
        
            return stringBuilder.ToString();
        }
    }

    internal class ZoneEntry
    {
        public string Zone { get; set; }
        public string Type { get; set; }
        public string Options { get; set; }
        public string InOptions { get; set; }
        public string OutOptions { get; set; }

        public override string ToString()
        {
            return $"{Zone}\t{Type}\t{Options}\t{InOptions}\t{OutOptions}";
        }
    }
}