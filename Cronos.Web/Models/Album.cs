using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronos.Web.Models
{
    public class Album
    {
        public bool Checked { get; set; }
        public string AlbumName { get; set; }

        public string AlbumDate { get; set; }

        public int Popularity { get; set; }

        public string ImgUrl { get; set; }

        public Dictionary<int, Track> Tracks { get; set; }
    }
}
