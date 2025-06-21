using Domain.Entities.Join;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configurations
{
    public class SeriesGenreConfiguration : IEntityTypeConfiguration<SeriesGenre>
    {
        public void Configure(EntityTypeBuilder<SeriesGenre> builder)
        {
            builder.HasKey(sg => new { sg.SeriesId, sg.GenreId });

            builder.HasOne(sg => sg.Series)
                   .WithMany(s => s.SeriesGenres)
                   .HasForeignKey(sg => sg.SeriesId);

            builder.HasOne(sg => sg.Genre)
                   .WithMany(g => g.SeriesGenres)
                   .HasForeignKey(sg => sg.GenreId);
        }
    }
}
