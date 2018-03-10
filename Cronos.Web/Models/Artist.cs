using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentSpotifyApi.Model;

namespace Cronos.Web.Models
{
    public class Artist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }

        public bool IsSelectedArtist { get; set; }

    }
}
