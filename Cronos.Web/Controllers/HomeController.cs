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

namespace Cronos.Web.Controllers
{
    public class HomeController : CronosBaseController
    {
        private IMapper _mapper;
        private MockSpotifyService _spotifyService;

        public HomeController(IMapper mapper, MockSpotifyService spotifyService)
        {
            _mapper = mapper;
            _spotifyService = spotifyService;
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult SelectArtist()
        {
            CronosState.CurrentState = UserState.SearchArtist;

            if(CronosState.SelectedArtist != null)
                CronosState.CurrentState = UserState.SelectedArtist;

            var vm = _mapper.Map<SelectArtistViewModel>(CronosState);

            return View(vm);
        }

        [HttpPost]
        public IActionResult SelectArtist(SelectArtistViewModel vm)
        {
            CronosState.CurrentState = UserState.SearchArtist;
            
            if (!string.IsNullOrEmpty(vm.SearchTerm))
            {
                CronosState.SearchTerm = vm.SearchTerm;
                var results = _spotifyService.SearchForArtist(vm.SearchTerm);
                CronosState.ArtistResults = _mapper.Map<IEnumerable<Artist>>(results);
                vm.ArtistListMessage = "Select an artist from the search results";
            }
            
            
            vm = _mapper.Map<SelectArtistViewModel>(CronosState);
            return View(vm);
        }

        public IActionResult SelectAlbum()
        {
            if (CronosState.SelectedArtist != null)
                CronosState.CurrentState = UserState.SelectAlbums;

            if (CronosState.HighestState < UserState.SelectedArtist)
                RedirectToAction("SelectArtist");

            if (CronosState.AlbumResults == null)
                RedirectToAction("SelectAlbum", CronosState.SelectedArtist);

            var vm = _mapper.Map<SelectAlbumsViewModel>(CronosState);

            return View(vm);
        }

        public IActionResult SelectAlbumByArtist(string artistId)
        {

            CronosState.SelectedArtist = artistId;

            var albums = _spotifyService.SearchAlbums(artistId);
            CronosState.AlbumResults = _mapper.Map<IEnumerable<Album>>(albums);

            var vm = _mapper.Map<SelectAlbumsViewModel>(CronosState);

            return View("SelectAlbum", vm);
        }

        [HttpPost]
        public IActionResult SelectAlbum(SelectAlbumsViewModel viewModel)
        {
            var selectedAlbums = viewModel.AlbumResults.Where(a => a.Checked);

            if (selectedAlbums.Any())
            {
                CronosState.AlbumResults = selectedAlbums;
                var vm = new CreatePlaylistViewModel();
                vm.SelectedAlbums = selectedAlbums;

                return View("CreatePlaylist", vm);

            }

            return View(_mapper.Map<SelectAlbumsViewModel>(CronosState));
        }

        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
