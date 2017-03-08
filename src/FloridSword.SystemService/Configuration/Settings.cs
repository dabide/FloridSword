using FloridSword.SystemService.Configuration.Configurators.DnsMasq;
using FloridSword.SystemService.Configuration.Configurators.ShoreWall;

namespace FloridSword.SystemService.Configuration
{
    internal class Settings
    {
        public ShoreWallConfiguration ShoreWallConfiguration { get; set; } = new ShoreWallConfiguration();
        public DnsMasqConfiguration DnsMasqConfiguration { get; set; } = new DnsMasqConfiguration();

        public static Settings Instance { get; set; } = new Settings();
    }
}
