using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Cronos.Web.Models;
using Cronos.Web.Services;
using Cronos.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;

namespace Cronos.Web.Controllers
{
    [Authorize]
    public class HomeController : CronosBaseController
    {
        private readonly IMapper _mapper;
        private readonly ISpotifyService _spotifyService;

        public HomeController(IMapper mapper, ISpotifyService spotifyService, IMemoryCache memoryCache) 
            : base(memoryCache)
        {
            _mapper = mapper;
            _spotifyService = spotifyService;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {

            return View();
        }


        public async Task<IActionResult> SelectArtist()
        {
            CronosState.CurrentState = UserState.SelectArtist;

            var CurrentFeedback = "Search for an artist or select one of your recents from below";

            if (!ModelState.IsValid)
            {
                var oldVm = _mapper.Map<SelectArtistViewModel>(CronosState);
                return View(oldVm);
            }

            if (CronosState.AlbumResults == null || !CronosState.AlbumResults.Any() && string.IsNullOrEmpty(CronosState.SearchTerm))
            {
                var recentlyPlayed = await _spotifyService.GetRecentlyPlayed();
                CronosState.ArtistResults = _mapper.Map<IEnumerable<Artist>>(recentlyPlayed);
            }

            

            var vm = _mapper.Map<SelectArtistViewModel>(CronosState);
            vm.ArtistListMessage = CurrentFeedback;

            if (vm.SelectedArtist != null)
            {
                var selectedArtist = vm.ArtistResults.FirstOrDefault(a => a.Id == vm.SelectedArtist.Id);
                if (selectedArtist != null)
                {
                    vm.ArtistResults = vm.ArtistResults.OrderByDescending(a => a.IsSelectedArtist);
                    selectedArtist.IsSelectedArtist = true;
                }
                else
                {
                    var arrayOfArtists = vm.ArtistResults.ToArray();
                    var newListOfArtists = new List<Artist> {vm.SelectedArtist};
                    newListOfArtists.AddRange(arrayOfArtists);
                    vm.ArtistResults = newListOfArtists;
                }
            }


            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SelectArtist(SelectArtistViewModel vm)
        {
            CronosState.CurrentState = UserState.SelectArtist;

            if (!ModelState.IsValid)
            {
                var oldVm = _mapper.Map<SelectArtistViewModel>(CronosState);
                return View(oldVm);
            }

            if (!string.IsNullOrWhiteSpace(vm.SearchedArtistId))
            {
                var selectedArtist = await _spotifyService.GetArtistById(vm.SearchedArtistId);
                CronosState.SelectedArtist = _mapper.Map<Artist>(selectedArtist);

                return RedirectToAction("SelectAlbumByArtist", "Home", new {ArtistId = CronosState.SelectedArtist.Id });
            }

            if (!string.IsNullOrEmpty(vm.SearchTerm))
            {
                CronosState.SearchTerm = vm.SearchTerm;
                var results = await _spotifyService.SearchArtistsAsync(vm.SearchTerm);

                CronosState.ArtistResults = _mapper.Map<IEnumerable<Artist>>(results);
                vm.ArtistListMessage = "Select an artist from the search results";
            }


            vm = _mapper.Map<SelectArtistViewModel>(CronosState);
            return View(vm);
        }

        public IActionResult SelectAlbums()
        {
            if (CronosState.SelectedArtist == null || CronosState.HighestState < UserState.SelectAlbums)
            {
               return RedirectToAction("SelectArtist");
            }
            CronosState.CurrentState = UserState.SelectAlbums;

            if (CronosState.AlbumResults == null)
                return RedirectToAction("SelectAlbumByArtist", "Home", CronosState.SelectedArtist);

            var vm = _mapper.Map<SelectAlbumsViewModel>(CronosState);

            return View(vm);
        }

        public async Task<IActionResult> SuggestedArtists(string term)
        {
            var artistSearches = await _spotifyService.SearchArtistsAsync(term);
            var artistResults = _mapper.Map<IEnumerable<Artist>>(artistSearches).ToList();

            return Json(artistResults);

        }

        public async Task<IActionResult> SelectAlbumByArtist(string artistId)
        {
            if (string.IsNullOrWhiteSpace(artistId))
                RedirectToAction("SelectAlbums");

            var fullArtist = await _spotifyService.GetArtistById(artistId);

            CronosState.SelectedArtist = _mapper.Map<Artist>(fullArtist);

            var albums = await _spotifyService.GetAlbumsByArtistAsync(artistId);

            CronosState.AlbumResults = _mapper.Map<IEnumerable<Album>>(albums);
            CronosState.CurrentState = UserState.SelectAlbums;

            var vm = _mapper.Map<SelectAlbumsViewModel>(CronosState);

            return View("SelectAlbums", vm);
        }

        [HttpPost]
        public IActionResult SelectAlbumByArtist(IEnumerable<Album> albums)
        {

            if (CronosState?.AlbumResults == null)
                return RedirectToAction("SelectArtist");

            var selectedAlbums = albums.Where(a => a.Checked).ToArray();
            if (selectedAlbums.Length == 0)
            {
                TempData[Constants.AlbumErrorMessage] = true;

                var oldVm = _mapper.Map<SelectAlbumsViewModel>(CronosState);
                return View("SelectAlbums", oldVm);

            }
             
            if (selectedAlbums.Any())
            {
                CronosState.CurrentState = UserState.CreatePlaylist;
                foreach (var updatedAlbum in albums.ToArray())
                {
                    var albumMatch = CronosState.AlbumResults.FirstOrDefault(a => a.Id == updatedAlbum.Id);
                    if (albumMatch != null)
                        albumMatch.Checked = updatedAlbum.Checked;
                }


                var createPlaylist = _mapper.Map<CreatePlaylistViewModel>(CronosState);

                return View("CreatePlaylist", createPlaylist);
            }

            return View("SelectAlbums", _mapper.Map<SelectAlbumsViewModel>(CronosState));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(CreatePlaylistViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                var createPlaylist = _mapper.Map<CreatePlaylistViewModel>(CronosState);
                return View(createPlaylist);
            }

            if (CronosState?.AlbumResults == null)
                return RedirectToAction("SelectArtist");
            try
            {
                var playlist = new Playlist();
                foreach (var album in CronosState.AlbumResults.Where(a=>a.Checked))
                {
                    foreach (var t in album.Tracks)
                    {
                        playlist.Tracks.Add(t);
                    }
                }

                playlist.Name = viewModel.PlaylistTitle;
                playlist.Description = $"Playlist created by cronos.frenetik.io on {DateTime.Now:D} by none other than {User.Identity.Name}";
                playlist.Public = viewModel.Public;

                var returnPlaylist = await _spotifyService.CreatePlaylistAsync(playlist);

                var created = _mapper.Map<CompletedPlaylistViewModel>(returnPlaylist);

                CronosState.Reset();

                return View("CompletedPlaylist", created);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
                return View("Error", new ErrorViewModel(){RequestId = HttpContext.TraceIdentifier});
            }
        }

        public IActionResult CreatePlaylist()
        {


            if (CronosState.HighestState < UserState.SelectAlbums)
                return RedirectToAction("SelectArtist");
            if (!CronosState.AlbumResults.Any(a=>a.Checked))
                return RedirectToAction("SelectAlbums");

            CronosState.CurrentState = UserState.CreatePlaylist;

            if (CronosState.AlbumResults.Any(a => a.Checked))
            {
                var vm = _mapper.Map<CreatePlaylistViewModel>(CronosState);
                vm.SelectedAlbums = CronosState.AlbumResults.Where(a => a.Checked);
                
                return View("CreatePlaylist", vm);
            }

            var createPlaylist = _mapper.Map<CreatePlaylistViewModel>(CronosState);

            return View("CreatePlaylist", createPlaylist);
        }

        public IActionResult StartOver()
        {
            CronosState.Reset();
            return RedirectToAction("SelectArtist");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
