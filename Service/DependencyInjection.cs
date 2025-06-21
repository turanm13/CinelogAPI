using Microsoft.Extensions.DependencyInjection;
using Service.Services.Interfaces;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmailSender, SmtpEmailSender>();
            services.AddScoped<IActorService, ActorService>();
            services.AddScoped<IDirectorService, DirectorService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<ISeriesService, SeriesService>();
            services.AddScoped<IEpisodeService, EpisodeService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IWatchlistService, WatchlistService>();
            return services;
        }
    }
}
