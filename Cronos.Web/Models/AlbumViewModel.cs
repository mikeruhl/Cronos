using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronos.Web.Models
{
    public class AlbumViewModel
    {
        public string AlbumId { get; set; }

        public string ImgUrl { get; set; }
        public string AlbumTitle { get; set; }
        public string ReleaseDate { get; set; }
        public bool IsChecked { get; set; }
        

    }
}
