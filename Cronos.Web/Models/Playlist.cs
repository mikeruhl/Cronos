using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronos.Web.Models
{
    public class Playlist
    {
        public Playlist()
        {
            Tracks = new List<Track>();
        }
        public List<Track> Tracks { get; set; }
        public bool Public { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
