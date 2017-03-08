using System;
using System.IO;
using FloridSword.SystemService.Configuration.Configurators.DnsMasq;
using FloridSword.SystemService.Configuration.Configurators.ShoreWall;
using ServiceStack;
using ServiceStack.Text;

namespace FloridSword.SystemService.Configuration
{
    internal class SystemConfigurator : ISystemConfigurator
    {
        private readonly IShoreWallConfigurator _shoreWallConfigurator;
        private readonly IDnsMasqConfigurator _dnsMasqConfigurator;
        private const string ConfigPath = "/etc/floridsword.conf";

        public SystemConfigurator(IShoreWallConfigurator shoreWallConfigurator, IDnsMasqConfigurator dnsMasqConfigurator)
        {
            _shoreWallConfigurator = shoreWallConfigurator;
            _dnsMasqConfigurator = dnsMasqConfigurator;
        }

        public void Save(Settings settings)
        {
            var settingsJson = settings.ToJson().IndentJson();
            File.WriteAllText(ConfigPath, settingsJson);
        }

        public void Load()
        {
            if (!File.Exists(ConfigPath))
            {
                Settings settings = CreateInitialConfiguration();
                Save(settings);
            }

            var settingsJson = File.ReadAllText(ConfigPath);
            Settings.Instance = settingsJson.FromJson<Settings>();

            ApplySettings(Settings.Instance);
        }

        private Settings CreateInitialConfiguration()
        {
            Settings settings = new Settings();

            ShoreWallConfiguration shoreWallConfiguration = settings.ShoreWallConfiguration;
            shoreWallConfiguration.Zones.Add("net");
            shoreWallConfiguration.Zones.Add("lan");
            shoreWallConfiguration.Zones.Add("guest");
            shoreWallConfiguration.Interfaces.Add("net", "eth0", true);
            shoreWallConfiguration.Interfaces.Add("lan", "eth1", false);
            shoreWallConfiguration.Policy.Add("lan", "net", PolicyType.Accept);
            shoreWallConfiguration.Policy.Add("$FW", "all", PolicyType.Accept);
            shoreWallConfiguration.Policy.Add("net", "all", PolicyType.Drop);
            shoreWallConfiguration.Rules.Add(Section.New, "SSH(ACCEPT)", "lan", "$FW");
            shoreWallConfiguration.Rules.Add(Section.New, "DNS(ACCEPT)", "lan", "$FW");
            shoreWallConfiguration.Masq.Add("eth0", "10.0.0.0/8,169.254.0.0/16,172.16.0.0/12,192.168.0.0/16");

            settings.DnsMasqConfiguration.AddInterface("eth1");
            settings.DnsMasqConfiguration.AddDhcpRange("eth1", "192.168.1.100", "192.168.1.199", TimeSpan.FromHours(12));

            return settings;
        }

        private void ApplySettings(Settings settings)
        {
            _shoreWallConfigurator.Apply(settings.ShoreWallConfiguration);
            _dnsMasqConfigurator.Apply(settings.DnsMasqConfiguration);            
        }
    }

    internal interface ISystemConfigurator
    {
        void Save(Settings settings);
        void Load();
    }
}
