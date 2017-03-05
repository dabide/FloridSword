using System.Collections.Generic;
using System.Text;

namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal class Interfaces : ShoreWallFile
    {
        public List<InterfaceEntry> InterfaceEntries { get; } = new List<InterfaceEntry>();

        public void Add(string name, bool external)
        {
            InterfaceEntries.Add(new InterfaceEntry
            {
                Interface = name,
                Options = external ? "tcpflags,dhcp,nosmurfs,routefilter,logmartians,sourceroute=0" : "tcpflags,nosmurfs,routefilter,logmartians"
            });
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("###############################################################################");
            stringBuilder.AppendLine("? FORMAT 2");
            stringBuilder.AppendLine("###############################################################################");
            stringBuilder.AppendLine("#ZONE\tINTERFACE\tOPTIONS");
        
            foreach (InterfaceEntry interfaceEntry in InterfaceEntries)
            {
                stringBuilder.AppendLine(interfaceEntry.ToString());
            }
        
            return stringBuilder.ToString();
        }
    }

    internal class InterfaceEntry
    {
        public string Zone { get; set; }
        public string Interface { get; set; }
        public string Options { get; set; }

        public override string ToString()
        {
            return $"{Zone}\t{Interface}\t{Options}";
        }
    }
}
