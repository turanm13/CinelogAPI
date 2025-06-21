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
    public class MovieDirectorConfiguration : IEntityTypeConfiguration<MovieDirector>
    {
        public void Configure(EntityTypeBuilder<MovieDirector> builder)
        {
            builder.HasKey(md => new { md.MovieId, md.DirectorId });

            builder.HasOne(md => md.Movie)
                   .WithMany(m => m.MovieDirectors)
                   .HasForeignKey(md => md.MovieId);

            builder.HasOne(md => md.Director)
                   .WithMany(d => d.MovieDirectors)
                   .HasForeignKey(md => md.DirectorId);
        }
    }
}
