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
    public class DirectorConfiguration : IEntityTypeConfiguration<Director>
    {
        public void Configure(EntityTypeBuilder<Director> builder)
        {
            builder.Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(m => m.BirthDate)
                .IsRequired();
            builder.Property(m => m.Bio)
                .HasMaxLength(1000);

            builder.HasMany(a => a.MovieDirectors)
                   .WithOne(ma => ma.Director)
                   .HasForeignKey(ma => ma.DirectorId);

            builder.HasMany(a => a.SeriesDirectors)
                   .WithOne(sa => sa.Director)
                   .HasForeignKey(sa => sa.DirectorId);
        }
    }
}
