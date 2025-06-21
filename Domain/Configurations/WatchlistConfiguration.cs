using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configurations
{
    public class WatchlistConfiguration : IEntityTypeConfiguration<Watchlist>
    {
        public void Configure(EntityTypeBuilder<Watchlist> builder)
        {
            builder.HasOne(w => w.User)
                   .WithMany()
                   .HasForeignKey(w => w.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Movie)
                   .WithMany(m => m.Watchlists)
                   .HasForeignKey(w => w.MovieId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Series)
                   .WithMany(s => s.Watchlists)
                   .HasForeignKey(w => w.SeriesId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasCheckConstraint("CK_Watchlist_MovieOrSeries",
                @"(MovieId IS NOT NULL AND SeriesId IS NULL) 
                OR (MovieId IS NULL AND SeriesId IS NOT NULL)");
        }
    }
}
