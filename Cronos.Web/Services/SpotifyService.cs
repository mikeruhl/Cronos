using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cronos.Web.Models;
using FluentSpotifyApi;
using FluentSpotifyApi.Builder.Artists;
using FluentSpotifyApi.Builder.User.Playlists;
using FluentSpotifyApi.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Cronos.Web.Services
{
    public class SpotifyService
    {
        private IFluentSpotifyClient _fluentSpotifyClient;
        private ILogger<SpotifyService> _logger;
        private IMapper _mapper;
        private IMemoryCache _memoryCache;

        public SpotifyService(IFluentSpotifyClient fluentSpotifyClient, 
            ILogger<SpotifyService> logger, 
            IMapper mapper,
            IMemoryCache memoryCache)
        {
            _fluentSpotifyClient = fluentSpotifyClient;
            _logger = logger;
            _mapper = mapper;
            _memoryCache = memoryCache;
             
        }

        public async Task<FullArtist> GetArtistById(string id)
        {
            if (_memoryCache.TryGetValue(id, out FullArtist artist))
            {
                return await Task.Run(()=> artist);
            }
            return await _fluentSpotifyClient.Artist(id).GetAsync();
        }

        public async Task<string> GetUserId()
        {
            try
            {
                return (await _fluentSpotifyClient.Me.GetAsync()).Id;
            }
            catch (Exception e)
            {
                _logger.LogError(LoggingEvents.GetUserId, e, "Couldn't get userid from Spotify Service", null);
            }

            return null;

        }
        public async Task<IEnumerable<FullArtist>> GetRecentlyPlayed()
        {

            var mystuff = await _fluentSpotifyClient.Me.Personalization.TopArtists.GetAsync();

            return mystuff
                .Items;

        }

        private void CacheArtists(IEnumerable<FullArtist> artists)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            foreach (var artist in artists)
                _memoryCache.Set(artist.Id, artist, cacheEntryOptions);
        }

        public async Task<FullArtist[]> SearchArtistsAsync(string searchTerm, int page = 0)
        {
            return (await _fluentSpotifyClient.Search.Artists.Matching(t => t.Artist == searchTerm).GetAsync(offset: page * 20, market: "from_token"))
                .Page.Items;
        }

        public async Task<FullAlbum[]> GetAlbumsByArtistAsync(string artistId, int page = 0)
        {
            var simpleAlbums = (await _fluentSpotifyClient.Artist(artistId).Albums
                    .GetAsync(new[] { AlbumType.Album }, limit: 50, offset: page * 50))
                .Items.ToArray();

            var returnAlbums = new ConcurrentBag<FullAlbum>();

            var tasks = new List<Task>();

            for (var index = 0; index < simpleAlbums.Length; index += 20)
            {
                var albumIndex = index;
                var fetch = Task.Run(async () =>
                {
                    var fetchAlbums = simpleAlbums.Where((k, i) => i >= albumIndex && i < albumIndex + 19)
                        .Select(t => t.Id);
                    var fullAlbumsPartial = (await _fluentSpotifyClient.Albums(fetchAlbums).GetAsync("from_token")).Items;
                    foreach (var album in fullAlbumsPartial)
                    {
                        returnAlbums.Add(album);
                    }
                });
                tasks.Add(fetch);
            }

            Task.WaitAll(tasks.ToArray());

            return returnAlbums.Distinct(new AlbumEqualityComparer()).ToArray();
        }

        public async Task<FullPlaylist> CreatePlaylistAsync(Playlist playlist)
        {
            var playlistDto = _mapper.Map<CreatePlaylistDto>(playlist);

            var newPlaylist = await _fluentSpotifyClient.Me.Playlists.CreateAsync(playlistDto);

            var trackArray = playlist.Tracks.ToArray();

            for (var i = 0; i < trackArray.Length; i += 100)
            {
                var bound = i;
                var addedTracks = await _fluentSpotifyClient.Me.Playlist(newPlaylist.Id).Tracks((playlist.Tracks.Where((t,k)=>k >= bound && k <= bound + 99).Select(t => t.Id))).AddAsync();
            }

            

            var fullPlaylist = await _fluentSpotifyClient.Me.Playlist(newPlaylist.Id).GetAsync();
            return fullPlaylist;

        }
    }

    public class AlbumEqualityComparer : IEqualityComparer<FullAlbum>
    {
        public bool Equals(FullAlbum x, FullAlbum y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(FullAlbum obj)
        {
            return obj.Name.GetHashCode() * obj.Tracks.Items.Length;
        }
    }
}
