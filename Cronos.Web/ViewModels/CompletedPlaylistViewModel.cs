using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Cronos.Web.ViewModels
{
    public class CompletedPlaylistViewModel : FlowBaseViewModel
    {
        public CompletedPlaylistViewModel()
        {
            Tracks = new List<string>();
        }
        public string Description { get; set; }
        [IgnoreMap]
        public List<string> Tracks { get; set; }

        public string Href { get; set; }
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public bool? Public { get; set; }

    }
}
