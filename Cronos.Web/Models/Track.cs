using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Cronos.Web.Models
{
    public class Track
    {
        [IgnoreMap]
        public int TrackNumber { get; set; }

        public string Id { get; set; }
        public int Duration { get; set; }
        public string Name { get; set; }
    }
}
