using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
