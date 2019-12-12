using System.Collections.Generic;

namespace Core.Data
{
    /// <summary>
    /// feature data use for mapbox
    /// </summary>
    public class FeatureCollection
    {
        public string Type { get; set; } = "FeatureCollection";
        public IEnumerable<FeaturePoint> Features { get; set; }
    }

    public class FeaturePoint
    {
        public string Type { get; set; } = "Feature";
        public Geometry Geometry { get; set; }
        public Property Properties { get; set; }
    }
    public class Geometry
    {
        public Geometry(double longitude, double latitude)
        {
            this.Coordinates = new[] { longitude, latitude };
        }
        public string Type { get; set; } = "Point";
        public double[] Coordinates { get; set; }
    }
    public class Property
    {
        public string Name { get; set; }
    }
}
