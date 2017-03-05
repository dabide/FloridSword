using System.Collections.Generic;
using System.Text;

namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal class Policy : ShoreWallFile
    {
        public List<PolicyEntry> PolicyEntries { get; } = new List<PolicyEntry>();

        public void Add(string source, string dest, PolicyType policy, LogLevel logLevel)
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
        
            return stringBuilder.ToString();
        }
    }

    internal class PolicyType
    {
        private readonly string _name;

        private PolicyType(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        public static PolicyType Drop { get; } = new PolicyType("DROP");
        public static PolicyType Reject { get; } = new PolicyType("REJECT");
        public static PolicyType Accept { get; } = new PolicyType("ACCEPT");
    }

    internal class PolicyEntry
    {
        public string Source { get; set; }
        public string Dest { get; set; }
        public PolicyType Policy { get; set; }
        public LogLevel LogLevel { get; set; }
        public string LimitBurst { get; set; }

        public override string ToString()
        {
            return $"{Source}\t{Dest}\t{Policy}\t{LogLevel.ToString().ToLowerInvariant()}\t{LimitBurst}";
        }
    }
}
