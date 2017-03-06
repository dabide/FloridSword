namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal class Init : ShoreWallFile
    {
        public string Script { get; set; }

        public override string ToString()
        {
            return Script;
        }
    }
}
