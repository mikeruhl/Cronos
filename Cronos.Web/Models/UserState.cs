using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cronos.Web.Models
{
    public enum UserState
    {
        StartingOut = 0,

        SearchArtist = 2,
        SelectedArtist = 4,

        SelectAlbums = 8,
        SelectedAlbums = 64,

        GeneratePlaylist = 128,
        GeneratedPlaylist = 16384
    }
}
