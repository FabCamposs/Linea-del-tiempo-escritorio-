using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tarea1_3erParcial
{
    class Class2
    {
        public class Cancion
        {
            
            public clsTrack Tracks { get; set; }
        }

        public class clsTrack
        {
            [JsonProperty("track")]
            public List<TrackData> tracklist { get; set; }
        }

        public class TrackData
        {
            public string name { get; set; }
            public string url { get; set; }
        }
    }
}
