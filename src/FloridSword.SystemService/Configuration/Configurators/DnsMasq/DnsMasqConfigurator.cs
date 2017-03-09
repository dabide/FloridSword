using FloridSword.SystemService.Tools;
using System;

namespace FloridSword.SystemService.Configuration.Configurators.DnsMasq
{
    internal class DnsMasqConfigurator : IDnsMasqConfigurator
    {
        private readonly IProcessTool _processTool;

        public DnsMasqConfigurator(IProcessTool processTool)
        {
            _processTool = processTool;
        }

        public void Apply(DnsMasqConfiguration configuration)
        {
            configuration.Write("/etc/dnsmasq.d");

            var processResult = _processTool.Execute("/usr/sbin/service", "dnsmasq restart");
            if (processResult.ExitCode != 0)
            {
                throw new Exception("Couldn't restart dnsmasq");
            }
        }
    }
}
