using System.Collections.Generic;
using System.Text;

namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal class Masq : ShoreWallFile
    {
        public List<MasqEntry> MasqEntries { get; } = new List<MasqEntry>();

        public void Add(string destInterface, string sourceInterface)
        {
            MasqEntries.Add(new MasqEntry
            {
                InterfaceDest = destInterface,
                Source = sourceInterface
            });
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(
                "#INTERFACE:DEST         SOURCE          ADDRESS         PROTO   PORT(S) IPSEC   MARK    USER/   SWITCH  ORIGINAL");
            stringBuilder.AppendLine(
                "#                                                                                       GROUP           DEST");

            foreach (var zone in MasqEntries)
                stringBuilder.AppendLine(zone.ToString());

            return stringBuilder.ToString();
        }
    }

    internal class MasqEntry
    {
        public string InterfaceDest { get; set; }
        public string Source { get; set; }
        public string Address { get; set; }
        public string Proto { get; set; }
        public string Ports { get; set; }
        public string IpSec { get; set; }
        public string Mark { get; set; }
        public string UserOrGroup { get; set; }
        public string Switch { get; set; }
        public string OriginalDest { get; set; }

        public override string ToString()
        {
            return $"{InterfaceDest}\t{Source}\t{Address}\t{Proto}\t{Ports}";
        }
    }
}
