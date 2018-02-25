using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentSpotifyApi.Model;

namespace Cronos.Web.Models
{
    public class UserProgress
    {
        public UserProgress()
        {
            CurrentState = UserState.StartingOut;

        }

        private UserState _currentState;


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

        public string SelectedArtist { get; set; }


        public string SearchTerm { get; set; }
        public IEnumerable<Artist> ArtistResults { get; set; }
        public IEnumerable<Album> AlbumResults { get; set; }
    }
}
