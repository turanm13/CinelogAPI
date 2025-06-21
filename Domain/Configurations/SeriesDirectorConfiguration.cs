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
    public class SeriesDirectorConfiguration : IEntityTypeConfiguration<SeriesDirector>
    {
        public void Configure(EntityTypeBuilder<SeriesDirector> builder)
        {
            builder.HasKey(sd => new { sd.SeriesId, sd.DirectorId });

            builder.HasOne(sd => sd.Series)
                   .WithMany(s => s.SeriesDirectors)
                   .HasForeignKey(sd => sd.SeriesId);

            builder.HasOne(sd => sd.Director)
                   .WithMany(d => d.SeriesDirectors)
                   .HasForeignKey(sd => sd.DirectorId);
        }
    }
}
