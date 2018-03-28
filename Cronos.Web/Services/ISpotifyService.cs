using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronos.Web.Models;
using FluentSpotifyApi.Model;

namespace Cronos.Web.Services
{

    public class MockSpotifyService
    {
        public IEnumerable<FullArtist> SearchForArtist(string searchTerm)
        {
            for (var i = 0; i < 5; i++)
            {
                yield return new FullArtist
                {
                    Id = i.ToString(),
                    Name = string.Concat(searchTerm, $" {i}")
                };
            }
        }

        public IEnumerable<FullAlbum> SearchAlbums(string artist)
        {
            for (var i = 0; i < 5; i++)
            {
                yield return new FullAlbum()
                {
                    Id = i.ToString(),
                    Name = string.Concat(artist, $" sings #{i}")
                };
            }
        }
    }
}

namespace Cronos.Web.Services
{
    public interface ISpotifyService
    {
        Task<FullArtist> GetArtistById(string id);
        Task<IEnumerable<FullArtist>> GetRecentlyPlayed();
        Task<FullArtist[]> SearchArtistsAsync(string searchTerm, int page = 0);
        Task<FullAlbum[]> GetAlbumsByArtistAsync(string artistId, int page = 0);
        Task<FullPlaylist> CreatePlaylistAsync(Playlist playlist);
    }
}
