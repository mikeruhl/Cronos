using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Cronos.Web.Models;
using Cronos.Web.ViewModels;
using FluentSpotifyApi.Builder.User.Playlists;
using FluentSpotifyApi.Model;
using Newtonsoft.Json;

namespace Cronos.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FullArtist, Artist>()
                .ForMember(dest=>dest.IsSelectedArtist, opt=>opt.Ignore())
                .ForMember(dest => dest.ImgUrl,
                    m => m.MapFrom(src => src.Images.OrderByDescending(i => i.Width).FirstOrDefault().Url));

            CreateMap<FullAlbum, Album>()
                .ForMember(dest => dest.ImgUrl,
                    m => m.MapFrom(src => src.Images.OrderByDescending(i => i.Width).FirstOrDefault().Url))
                .AfterMap((src, dest, context) =>
                {
                    var trackIterator = 1;
                    foreach (var disc in src.Tracks.Items.GroupBy(t=>t.DiscNumber).OrderBy(d => d.Key))
                    {
                        foreach (var track in src.Tracks.Items
                            .Where(d => d.DiscNumber == disc.Key)
                            .OrderBy(t => t.TrackNumber))
                        {
                            var newTrack = Mapper.Map<SimpleTrack, Track>(track);
                            newTrack.TrackNumber = trackIterator;
                            dest.Tracks.Add(newTrack);
                            trackIterator++;
                        }
                    }

                    //copyrights
                    dest.ReleaseDate = src.ReleaseDate.Substring(0, 4);
                });
                //.ForMember(dest=> dest.Tracks,
                //    m=>m.MapFrom(src => src.Tracks.Items.ToDictionary(k=>((k.DiscNumber - 1) * k.TrackNumber) + k.TrackNumber, v=>new Track()
                //    {
                //        Duration = v.DurationMs,
                //        Id = v.Id,
                //        Name = v.Name
                //    })));

            CreateMap<UserProgress, SelectArtistViewModel>();
            CreateMap<UserProgress, SelectAlbumsViewModel>();
            CreateMap<UserProgress, ProgressMenuViewModel>();
            CreateMap<UserProgress, CreatePlaylistViewModel>()
                .ForMember(dest=>dest.SelectedAlbums,
                    m=>m.MapFrom(src=>src.AlbumResults.Where(a=>a.Checked)));
            CreateMap<SimpleTrack, Track>()
                .ForMember(dest=>dest.Duration,
                    m=>m.MapFrom(src=>src.DurationMs));

            CreateMap<Playlist, CreatePlaylistDto>();

            CreateMap<FullPlaylist, CompletedPlaylistViewModel>()
                .ForMember(dest => dest.ImgUrl,
                    m => m.MapFrom(src => src.Images.OrderByDescending(i => i.Width).FirstOrDefault().Url))
                .ForMember(dest => dest.ExtUrl,
                    m => m.MapFrom(src => src.ExternalUrls.FirstOrDefault(s=>s.Key == "spotify").Value))
                .AfterMap((f, c) =>
                {
                    var i = 1;
                    foreach (var t in f.Tracks.Items)
                    {
                        c.Tracks.Add($"{i} - {t.Track.Name}");
                        i++;
                    }
                });


        }
    }

   
}