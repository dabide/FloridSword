namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal abstract class ShoreWallFile : ConfigFile
    {
        protected override string GetFileName()
        {
            return GetType().Name.ToLowerInvariant();
        }
    }
}
