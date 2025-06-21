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
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(m => m.BirthDate)
                .IsRequired();
            builder.Property(m => m.Bio)
                .HasMaxLength(1000);

            builder.HasMany(a => a.MovieActors)
                   .WithOne(ma => ma.Actor)
                   .HasForeignKey(ma => ma.ActorId);

            builder.HasMany(a => a.SeriesActors)
                   .WithOne(sa => sa.Actor)
                   .HasForeignKey(sa => sa.ActorId);
        }
    }
}
