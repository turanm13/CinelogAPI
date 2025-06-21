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
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            
            builder.Property(g => g.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(g => g.MovieGenres)
                   .WithOne(mg => mg.Genre)
                   .HasForeignKey(mg => mg.GenreId);

            builder.HasMany(g => g.SeriesGenres)
                   .WithOne(sg => sg.Genre)
                   .HasForeignKey(sg => sg.GenreId);
        }
    }
}
