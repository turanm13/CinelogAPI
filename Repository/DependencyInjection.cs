using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories.Interface;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public static class DependencyInjection
    {
        
        public static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();
            services.AddScoped<IEpisodeRepository, EpisodeRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<ISeriesRepository, SeriesRepository>();
            services.AddScoped<IWatchlistRepository, WatchlistRepository>();
            return services;
        }
    }
}
