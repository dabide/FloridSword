using System.IO;
using FloridSword.SystemService.Configuration.Configurators.ShoreWall;
using ServiceStack;

namespace FloridSword.SystemService.Configuration
{
    internal class Settings
    {
        private const string ConfigPath = "/etc/floridsword.conf";

        public ShoreWallConfiguration ShoreWallConfiguration { get; set; } = new ShoreWallConfiguration();

        public static Settings Instance { get; private set; } = new Settings();

        public static void Save()
        {
            var settingsJson = Instance.ToJson();
            File.WriteAllText(ConfigPath, settingsJson);
        }

        public static void Load()
        {
            if (!File.Exists(ConfigPath))
            {
                CreateInitialConfiguration();
                Save();
            }

            var settingsJson = File.ReadAllText(ConfigPath);
            Instance = settingsJson.FromJson<Settings>();

            ApplySettings();
        }

        private static void CreateInitialConfiguration()
        {
            Instance = new Settings();

            ShoreWallConfiguration shoreWallConfiguration = Instance.ShoreWallConfiguration;
            shoreWallConfiguration.Zones.Add("net");
            shoreWallConfiguration.Zones.Add("lan");
            shoreWallConfiguration.Zones.Add("guest");
            shoreWallConfiguration.Interfaces.Add("net", "eth0", true);
            shoreWallConfiguration.Interfaces.Add("lan", "eth1", false);
            shoreWallConfiguration.Policy.Add("lan", "net", PolicyType.Accept);
            shoreWallConfiguration.Policy.Add("net", "all", PolicyType.Drop, LogLevel.Info);
            shoreWallConfiguration.Masq.Add("eth0", "10.0.0.0/8,169.254.0.0/16,172.16.0.0/12,192.168.0.0/16");
        }

        private static void ApplySettings()
        {
            Instance.ShoreWallConfiguration.Apply();
        }
    }
}
