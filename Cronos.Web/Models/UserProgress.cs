using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FluentSpotifyApi.Model;

namespace Cronos.Web.Models
{
    public class UserProgress
    {
        public UserProgress()
        {

        }

        public Guid Id { get; set; }

        private UserState _currentState;
        private Artist _selectedArtist;


        public UserState HighestState { get;
            private set; }

        //public string SpotifyUserId { get; set; }
        //public string SessionId { get; set; }
        public UserState CurrentState
        {
            get => _currentState;
            set

            {
                _currentState = value;
                if (HighestState < _currentState)
                {
                    HighestState = _currentState;
                }
            }
        }


        public Artist SelectedArtist
        {
            get => _selectedArtist;
            set
            {
                _selectedArtist = value;
                if (value != null)
                _selectedArtist.IsSelectedArtist = true;
            }
        }


        public string SearchTerm { get; set; }
        public IEnumerable<Artist> ArtistResults { get; set; }
        public IEnumerable<Album> AlbumResults { get; set; }

        public void Reset()
        {
            CurrentState = UserState.SelectArtist;
            HighestState = UserState.SelectArtist;
            ArtistResults = null;
            AlbumResults = null;
            SelectedArtist = null;
            SearchTerm = string.Empty;
        }
    }
}
