


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
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            
            builder.Property(r => r.UserId)
                   .IsRequired();

            
            builder.Property(r => r.Score)
                   .IsRequired();

            builder.HasOne(r => r.User)
                   .WithMany()
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(r => r.Movie)
                   .WithMany(m => m.Ratings)
                   .HasForeignKey(r => r.MovieId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(r => r.Series)
                   .WithMany(s => s.Ratings)
                   .HasForeignKey(r => r.SeriesId)
                   .OnDelete(DeleteBehavior.Cascade); 


            builder.HasIndex(r => new { r.UserId, r.MovieId })
                   .IsUnique()
                   .HasFilter("[MovieId] IS NOT NULL");

            builder.HasIndex(r => new { r.UserId, r.SeriesId })
                   .IsUnique()
                   .HasFilter("[SeriesId] IS NOT NULL");
        }
    }
}
