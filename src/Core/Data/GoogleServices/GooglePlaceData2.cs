using System.Collections.Generic;

namespace Core.Data.GoogleServices
{
    public class GooglePlaceData2
    {
        public IEnumerable<TextPlaceData> candidates { get; set; }
        public string status { get; set; }
    }

    public class TextPlaceData
    {
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string name { get; set; }
        public OpeningHours opening_hours { get; set; }
        public IEnumerable<Photo> photos { get; set; }
        public double rating { get; set; }
    }
    public class OpeningHours
    {
        public bool open_now { get; set; }
    }



}
