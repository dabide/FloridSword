using System.IO;

namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal abstract class ShoreWallFile
    {
        public void Write(string configFolder)
        {
            File.WriteAllText(Path.Combine(configFolder, GetType().Name.ToLowerInvariant()), ToString());
        }
    }
}