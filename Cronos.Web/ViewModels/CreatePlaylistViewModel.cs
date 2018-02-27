using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cronos.Web.Extensions;
using Cronos.Web.Models;

namespace Cronos.Web.ViewModels
{
    public class CreatePlaylistViewModel : FlowBaseViewModel
    {
        [DisplayName("Selected Albums")]
        public IEnumerable<Album> SelectedAlbums { get; set; }

        [DisplayName("Playlist Title")]
        [Required(ErrorMessage = "Playlist title is required")]
        public string PlaylistTitle { get; set; }

        public bool Public { get; set; }

        public string Description { get; set; }

    }
}
