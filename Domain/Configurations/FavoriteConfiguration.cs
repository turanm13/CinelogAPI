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
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.Property(f => f.UserId)
                   .IsRequired();

            // İstifadəçi ilə əlaqə (1 istifadəçinin bir çox favoriti ola bilər)
            builder.HasOne(f => f.User)
                   .WithMany(u => u.Favorites)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Film ilə əlaqə (optional)
            builder.HasOne(f => f.Movie)
                   .WithMany(m => m.Favorites)
                   .HasForeignKey(f => f.MovieId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Serial ilə əlaqə (optional)
            builder.HasOne(f => f.Series)
                   .WithMany(s => s.Favorites)
                   .HasForeignKey(f => f.SeriesId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
