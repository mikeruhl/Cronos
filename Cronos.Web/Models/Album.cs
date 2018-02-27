using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Cronos.Web.Models
{
    public class Album
    {
        public Album()
        {
            Tracks = new List<Track>();
        }
        public bool Checked { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public string ReleaseDate { get; set; }

        public int Popularity { get; set; }

        public string ImgUrl { get; set; }
        public string Id { get; set; }

        [IgnoreMap]
        public List<Track> Tracks { get; private set; }
    }
}
