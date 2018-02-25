using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronos.Web.Models;

namespace Cronos.Web.ViewModels
{
    public class CreatePlaylistViewModel
    {
        public IEnumerable<Album> SelectedAlbums { get; set; }

    }
}
