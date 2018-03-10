using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronos.Web.Models;

namespace Cronos.Web.ViewModels
{
    public class SelectArtistViewModel : FlowBaseViewModel
    {
        public IEnumerable<Artist> ArtistResults { get; set; }

        [Required]
        public string SearchTerm { get; set; }
        public string SearchedArtistId { get; set; }

        public string ArtistListMessage { get; set; }

        public Artist SelectedArtist { get; set; }
    }
}
