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
    public class SeriesConfiguration : IEntityTypeConfiguration<Series>
    {
        public void Configure(EntityTypeBuilder<Series> builder)
        {
            builder.Property(s => s.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(s => s.Description)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(s => s.PosterUrl)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(s => s.ReleaseDate)
                   .IsRequired()
                   .HasColumnType("date");

            builder.HasMany(s => s.Episodes)
                   .WithOne(e => e.Series)
                   .HasForeignKey(e => e.SeriesId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.HasMany(s => s.Favorites)
                   .WithOne(f => f.Series)
                   .HasForeignKey(f => f.SeriesId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Ratings)
                   .WithOne(r => r.Series)
                   .HasForeignKey(r => r.SeriesId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Comments)
                   .WithOne(c => c.Series)
                   .HasForeignKey(c => c.SeriesId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
