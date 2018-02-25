using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Cronos.Web.Models;
using Cronos.Web.ViewModels;
using FluentSpotifyApi.Model;

namespace Cronos.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FullArtist, Artist>()
                .ForMember(dest => dest.ImgUrl,
                    m => m.MapFrom(src => src.Images.OrderByDescending(i => i.Width).FirstOrDefault().Url));

            CreateMap<FullAlbum, Album>()
                .ForMember(dest => dest.ImgUrl,
                    m => m.MapFrom(src => src.Images.OrderByDescending(i => i.Width).FirstOrDefault().Url))
                .ForMember(dest=> dest.Tracks,
                    m=>m.MapFrom(src => src.Tracks.Items.ToDictionary(k=>k.DiscNumber * k.TrackNumber + k.TrackNumber, v=>new Track()
                    {
                        Duration = v.DurationMs,
                        Id = v.Id,
                        Name = v.Name
                    })));

            CreateMap<UserProgress, SelectArtistViewModel>();

            CreateMap<UserProgress, ProgressMenuViewModel>();
        }
    }
}