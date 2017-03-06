using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal class ShoreWallConfiguration
    {
        private const string ConfigFolder = "/etc/shorewall";

        public ShoreWallConfiguration()
        {
            Init = new Init();
            Interfaces = new Interfaces();
            Zones = new Zones();
            Policy = new Policy();
            Rules = new Rules();
            Masq = new Masq();
        }

        public Init Init { get; set; }
        public Interfaces Interfaces { get; set; }
        public Zones Zones { get; set; }
        public Policy Policy { get; set; }
        public Rules Rules { get; set; }
        public Masq Masq { get; set; }

        public void Apply()
        {
            Write();

            // TODO: Enable when sure that it'll work
            //ReloadShoreWall();
        }

        private static void ReloadShoreWall()
        {
            var process = new Process
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

                var reader = process.StandardOutput;
                var output = reader.ReadToEnd();

                process.WaitForExit();
            }

            if (process.ExitCode != 0)
                throw new Exception("Couldn't reload ShoreWall");
        }

        private void Write()
        {
            var shoreWallConfPath = $"{ConfigFolder}/shorewall.conf";
            var shoreWallConf = File.ReadAllText(shoreWallConfPath);
            shoreWallConf = Regex.Replace(shoreWallConf, @"IP_FORWARDING=.*$", "IP_FORWARDING=Yes",
                RegexOptions.Multiline);
            shoreWallConf = Regex.Replace(shoreWallConf, @"SAVE_IPSETS=.*$", "SAVE_IPSETS=Yes", RegexOptions.Multiline);
            File.WriteAllText(shoreWallConfPath, shoreWallConf);

            Init.Write(ConfigFolder);
            Interfaces.Write(ConfigFolder);
            Zones.Write(ConfigFolder);
            Policy.Write(ConfigFolder);
            Rules.Write(ConfigFolder);
            Masq.Write(ConfigFolder);
        }
    }
}