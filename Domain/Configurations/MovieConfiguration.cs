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
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.Property(m => m.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.Description)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(m => m.ReleaseDate)
                   .IsRequired();

            builder.Property(m => m.Duration)
                   .IsRequired();


            builder.HasMany(m => m.MovieDirectors)
                   .WithOne(md => md.Movie)
                   .HasForeignKey(md => md.MovieId);

            builder.HasMany(m => m.MovieGenres)
                   .WithOne(mg => mg.Movie)
                   .HasForeignKey(mg => mg.MovieId);

            builder.HasMany(m => m.MovieActors)
                   .WithOne(ma => ma.Movie)
                   .HasForeignKey(ma => ma.MovieId);

            builder.HasMany(m => m.Favorites)
                   .WithOne(f => f.Movie)
                   .HasForeignKey(f => f.MovieId);

            builder.HasMany(m => m.Ratings)
                   .WithOne(r => r.Movie)
                   .HasForeignKey(r => r.MovieId);

            builder.HasMany(m => m.Comments)
                   .WithOne(c => c.Movie)
                   .HasForeignKey(c => c.MovieId);
        }
    }
}
