using System.IO;

namespace FloridSword.SystemService.Configuration.Configurators
{
    internal abstract class ConfigFile
    {
        public void Write(string configFolder)
        {
            File.WriteAllText(Path.Combine(configFolder, GetFileName()), ToString());
        }

        protected abstract string GetFileName();
    }
}
