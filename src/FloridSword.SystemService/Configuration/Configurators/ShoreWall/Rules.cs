using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal class Rules : ShoreWallFile
    {
        public List<RuleEntry> RuleEntries { get; set; } = new List<RuleEntry>();

        public void Add(Section section, string action, string source, string dest, string protocol)
        {
            RuleEntries.Add(new RuleEntry
            {
                Section = section,
                Action = action,
                Source = source,
                Proto = protocol
            });    
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("#ACTION\tSOURCE\tDEST\tPROTO\tDEST_PORT\tSOURCE_PORT(S)\tORIGINAL_DEST\tRATE_LIMIT\tUSER/GROUP\tMARK\tCONNLIMIT\tTIME\tHEADERS\tSWITCH\tHELPER");
        
            foreach (var ruleEntryGroup in RuleEntries.GroupBy(e => e.Section))
            {
                stringBuilder.AppendLine($"?SECTION {ruleEntryGroup.Key.ToString().ToUpperInvariant()}");
                foreach (RuleEntry entry in ruleEntryGroup)
                {
                    stringBuilder.AppendLine(entry.ToString());
                }
            }
        
            return stringBuilder.ToString();
        }
    }

    internal class RuleEntry
    {
        public Section Section { get; set; }
        public string Action { get; set; }
        public string Source { get; set; }
        public string Dest { get; set; }
        public string Proto { get; set; }
        public string DestPort { get; set; }
        public string SourcePorts { get; set; }
        public string OriginalDest { get; set; }
        public string RateLimit { get; set; }
        public string UserOrGroup { get; set; }
        public string Mark { get; set; }
        public string ConnLimit { get; set; }
        public string Time { get; set; }
        public string Headers { get; set; }
        public string Switch { get; set; }
        public string Helper { get; set; }

        public override string ToString()
        {
            return $"{Action}\t{Source}\t{Dest}\t{Proto}\t{DestPort}\t{SourcePorts}\t{OriginalDest}\t{RateLimit}\t{UserOrGroup}\t{Mark}\t{ConnLimit}\t{Time}\t{Headers}\t{Switch}\t{Helper}";
        }
    }

    internal enum Section
    {
        All,
        Established,
        Related,
        Invalid,
        Untracked,
        New
    }
}