namespace Core.Data
{
    /// <summary>
    /// main data: scenery from excel file
    /// </summary>
    public class Scenery
    {
        public string Name { get; set; }
        public Coordinates Coordinates { get; set; }
    }

    public class Coordinates
    {
        public double Longitude { get; set; }
        public double Latidue { get; set; }
    }
}
