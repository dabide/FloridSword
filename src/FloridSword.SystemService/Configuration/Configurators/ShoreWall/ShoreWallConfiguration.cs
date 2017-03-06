namespace FloridSword.SystemService.Configuration.Configurators.ShoreWall
{
    internal class ShoreWallConfiguration
    {
        public Init Init { get; set; } = new Init();
        public Interfaces Interfaces { get; set; } = new Interfaces();
        public Zones Zones { get; set; } = new Zones();
        public Policy Policy { get; set; } = new Policy();
        public Rules Rules { get; set; } = new Rules();
        public Masq Masq { get; set; } = new Masq();
    }
}
