using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Cronos.Web
{
    public static class Constants
    {
        internal const string SessionKeyName = "Cronos.SessionId";
        internal const string CookieName = "Cronos.Cookie.Session";
        public const string AlbumErrorMessage = "AlbumError";
        public const string ArtistErrorMessage = "ArtistError";


    }

    internal static class LoggingEvents
    {
        internal const int GetUserId = 1000;
        internal const int GetArtist = 2000;
        internal const int GetAlbums = 3000;
        internal const int CreatePlaylist = 4000;
    }
}
