using AutoMapper;
using Domain.Entities;
using Domain.Entities.Join;
using Service.DTOs.Actor;
using Service.DTOs.ActorCharacter;
using Service.DTOs.Comment;
using Service.DTOs.Director;
using Service.DTOs.Episode;
using Service.DTOs.Favorite;
using Service.DTOs.Genre;
using Service.DTOs.Movie;
using Service.DTOs.Rating;
using Service.DTOs.Series;
using Service.DTOs.Watchlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            //Actor Mapping
            CreateMap<Actor, ActorDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Movies, opt => opt.MapFrom(src => src.MovieActors.Select(m=>m.Movie)))
                .ForMember(dest => dest.Series, opt => opt.MapFrom(src => src.SeriesActors.Select(m=>m.Series)));


            CreateMap<ActorCreateDto, Actor>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            //Director Mapping
            CreateMap<Director, DirectorDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToString("yyyy-MM-dd")))
                //.ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.PhotoUrl))
                .ForMember(dest => dest.Movies, opt => opt.MapFrom(src => src.MovieDirectors.Select(m => m.Movie)))
                .ForMember(dest => dest.Serieses, opt => opt.MapFrom(src => src.SeriesDirectors.Select(m => m.Series)));
            CreateMap<Director, DirectorShortDto>();
            CreateMap<DirectorCreateDto, Director>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.Ignore());


            //Genre Mapping
            CreateMap<Genre, GenreDto>()
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("yyyy-MM-dd HH:mm")))
               .ForMember(dest => dest.Movies, opt => opt.MapFrom(src => src.MovieGenres.Select(m => m.Movie)))
                .ForMember(dest => dest.Series, opt => opt.MapFrom(src => src.SeriesGenres.Select(m => m.Series)));
            CreateMap<Genre, GenreShortDto>();

            CreateMap<GenreCreateDto, Genre>();

            //Movie Mapping
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.ToString(@"hh\:mm\:ss")))
                .ForMember(dest => dest.Directors, opt => opt.MapFrom(src => src.MovieDirectors.Select(md => md.Director)))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.MovieGenres.Select(mg => mg.Genre)))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Ratings != null && src.Ratings.Any()  ? Math.Round(src.Ratings.Average(r => r.Score), 1): 0))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.MovieActors.Select(ma => new ActorCharacterDto { ActorName= ma.Actor.FullName , ImageUrl = ma.Actor.ImageUrl , CharacterName = ma.CharacterName})));
            CreateMap<Movie, MovieShortDto>();
            CreateMap<MovieCreateDto, Movie>()
                .ForMember(dest => dest.PosterUrl, opt => opt.Ignore())
                .ForMember(dest => dest.MovieDirectors, opt => opt.MapFrom(src => src.DirectorsId.Select(id => new MovieDirector { DirectorId = id})))
                .ForMember(dest => dest.MovieActors, opt => opt.MapFrom(src => src.Actors.Select(a => new MovieActor { ActorId = a.ActorId, CharacterName = a.CharacterName })))
                .ForMember(dest => dest.MovieGenres, opt => opt.MapFrom(src => src.GenresId.Select(id => new MovieGenre { GenreId = id })));

            //Series Mapping
            CreateMap<Series, SeriesDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("dd.MM.yyyy HH:mm")))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate.ToString("dd.MM.yyyy")))
                .ForMember(dest => dest.Directors, opt => opt.MapFrom(src => src.SeriesDirectors.Select(md => md.Director)))
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.SeriesActors.Select(ma => new ActorCharacterDto { ActorName = ma.Actor.FullName, ImageUrl = ma.Actor.ImageUrl, CharacterName = ma.CharacterName })))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Ratings != null && src.Ratings.Any() ? Math.Round(src.Ratings.Average(r => r.Score), 1) : 0))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.SeriesGenres.Select(mg => mg.Genre)));
            CreateMap<Series, SeriesShortDto>();
            CreateMap<SeriesCreateDto, Series>()
                .ForMember(dest => dest.PosterUrl, opt => opt.Ignore())
                .ForMember(dest => dest.SeriesDirectors, opt => opt.MapFrom(src => src.DirectorsId.Select(id => new SeriesDirector { DirectorId = id })))
                .ForMember(dest => dest.SeriesActors, opt => opt.MapFrom(src => src.Actors.Select(a => new SeriesActor { ActorId = a.ActorId, CharacterName = a.CharacterName })))
                .ForMember(dest => dest.SeriesGenres, opt => opt.MapFrom(src => src.GenresId.Select(id => new SeriesGenre { GenreId = id })));

            //Episode Mapping
            CreateMap<Episode, EpisodeDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("dd.MM.yyyy HH:mm")))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate.ToString("dd.MM.yyyy")))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.ToString(@"hh\:mm\:ss")))
                .ForMember(dest => dest.SeriesName, opt => opt.MapFrom(src => src.Series.Title));

            CreateMap<EpisodeCreateDto, Episode>();

            //Comment Mapping
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("dd.MM.yyyy HH:mm")))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<Comment, CommentShortDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<CommentCreateDto, Comment>();

            //Favorite Mapping
            CreateMap<Favorite, FavoriteDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("dd.MM.yyyy HH:mm")))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<FavoriteCreateDto, Favorite>();

            //Raiting Mapping
            CreateMap<Rating, RatingDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("dd.MM.yyyy HH:mm")))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<RatingCreateDto, Rating>();

            //Watchlist Mapping
            CreateMap<Watchlist, WatchlistDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("dd.MM.yyyy HH:mm")))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<WatchlistCreateDto, Watchlist>();

        }
    }
}
