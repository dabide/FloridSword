using System;
using System.IO;
using System.Text.RegularExpressions;
using FloridSword.SystemService.Tools;

namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal class ShoreWallConfigurator : IShoreWallConfigurator
    {
        private readonly IProcessTool _processTool;
        private const string ConfigFolder = "/etc/shorewall";

        public ShoreWallConfigurator(IProcessTool processTool)
        {
            _processTool = processTool;
        }

        public void Apply(ShoreWallConfiguration configuration)
        {
            Write(configuration);

            ReloadShoreWall();
        }

        private void ReloadShoreWall()
        {
            ProcessResult processResult = _processTool.Execute("/sbin/shorewall", "refresh");

            if (processResult.ExitCode != 0)
            {
                throw new Exception("Couldn't reload ShoreWall");
            }
        }

        private void Write(ShoreWallConfiguration configuration)
        {
            SetConfigDefaults();
            EnableStartup();

            configuration.Init.Write(ConfigFolder);
            configuration.Interfaces.Write(ConfigFolder);
            configuration.Zones.Write(ConfigFolder);
            configuration.Policy.Write(ConfigFolder);
            configuration.Rules.Write(ConfigFolder);
            configuration.Masq.Write(ConfigFolder);
        }

        private static void SetConfigDefaults()
        {
            var shoreWallConfPath = $"{ConfigFolder}/shorewall.conf";
            var shoreWallConf = File.ReadAllText(shoreWallConfPath);
            shoreWallConf = Regex.Replace(shoreWallConf, @"IP_FORWARDING=.*$", "IP_FORWARDING=Yes",
                RegexOptions.Multiline);
            shoreWallConf = Regex.Replace(shoreWallConf, @"SAVE_IPSETS=.*$", "SAVE_IPSETS=Yes", RegexOptions.Multiline);
            File.WriteAllText(shoreWallConfPath, shoreWallConf);
        }

        private static void EnableStartup()
        {
            var shorewalDefaultFilePath = "/etc/default/shorewall";
            var shorewallDefault = File.ReadAllText(shorewalDefaultFilePath);
            Regex startupRegex = new Regex(@"^startup=0", RegexOptions.Multiline);
            if (!startupRegex.IsMatch(shorewallDefault)) return;

            startupRegex.Replace(shorewallDefault, "startup=1");
            File.WriteAllText(shorewalDefaultFilePath, shorewallDefault);
        }
    }
}
