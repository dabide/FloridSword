using System.Collections.Generic;
using System.Text;

namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal class Policy : ShoreWallFile
    {
        public List<PolicyEntry> PolicyEntries { get; set; } = new List<PolicyEntry>();

        public void Add(string source, string dest, PolicyType policy, LogLevel? logLevel = null)
        {
            PolicyEntries.Add(new PolicyEntry
            {
                Source = source,
                Dest = dest,
                Policy = policy,
                LogLevel = logLevel
            });
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("#SOURCE\tDEST\tPOLICY\tLOG LEVEL\tLIMIT:BURST");
        
            foreach (PolicyEntry policyEntry in PolicyEntries)
            {
                stringBuilder.AppendLine(policyEntry.ToString());
            }
            stringBuilder.AppendLine("# THE FOLLOWING POLICY MUST BE LAST");
            stringBuilder.AppendLine(
                new PolicyEntry {Source = "all", Dest = "all", Policy = PolicyType.Reject, LogLevel = LogLevel.Info}.ToString());
            
            return stringBuilder.ToString();
        }
    }

    internal class PolicyEntry
    {
        public string Source { get; set; }
        public string Dest { get; set; }
        public PolicyType Policy { get; set; }
        public LogLevel? LogLevel { get; set; }
        public string LimitBurst { get; set; }

        public override string ToString()
        {
            return $"{Source}\t{Dest}\t{Policy.ToString().ToUpperInvariant()}\t{LogLevel.ToString().ToLowerInvariant()}\t{LimitBurst}";
        }
    }
}
