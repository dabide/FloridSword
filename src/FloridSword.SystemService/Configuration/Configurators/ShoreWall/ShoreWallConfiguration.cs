using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal class ShoreWallConfiguration
    {
        const string ConfigFolder = "/etc/shorewall";

        public Interfaces Interfaces { get; } = new Interfaces();
        public Zones Zones { get; } = new Zones();
        public Policy Policy { get; } = new Policy();
        public Rules Rules { get; } = new Rules();
        public Masq Masq { get; } = new Masq();

        public void Apply()
        {
            var shoreWallConfPath = $"{ConfigFolder}/shorewall.conf";
            var shoreWallConf = File.ReadAllText(shoreWallConfPath);
            shoreWallConf = Regex.Replace(shoreWallConf, @"IP_FORWARDING=.*$", "IP_FORWARDING=Yes", RegexOptions.Multiline);
            File.WriteAllText(shoreWallConfPath, shoreWallConf);

            Interfaces.Write(ConfigFolder);
            Zones.Write(ConfigFolder);
            Policy.Write(ConfigFolder);
            Rules.Write(ConfigFolder);
            Masq.Write(ConfigFolder);

            Process process = new Process
            {
                StartInfo =
                {
                    FileName = "/sbin/shorewall",
                    Arguments = "reload",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            using (process)
            {
                process.Start();

                StreamReader reader = process.StandardOutput;
                string output = reader.ReadToEnd();

                process.WaitForExit();
            }

            if (process.ExitCode != 0)
            {
                throw new Exception("Couldn't reload ShoreWall");
            }
        }
    }
}
