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
    public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.Property(e => e.SeriesId)
                   .IsRequired();
            builder.Property(e => e.SeasonNumber)
                   .IsRequired();
            builder.Property(e => e.EpisodeNumber)
                   .IsRequired();
            builder.Property(e => e.Title)
                   .IsRequired()
                   .HasMaxLength(200);
            builder.Property(e => e.Duration)
                   .HasColumnType("time");
            builder.Property(e => e.ReleaseDate)
                   .HasColumnType("date");
            
            builder.HasOne(e => e.Series)
                   .WithMany(s => s.Episodes)
                   .HasForeignKey(e => e.SeriesId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
