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
    public class SeriesActorConfiguration : IEntityTypeConfiguration<SeriesActor>
    {
        public void Configure(EntityTypeBuilder<SeriesActor> builder)
        {
            builder.HasKey(sa => new { sa.SeriesId, sa.ActorId });

            builder.HasOne(sa => sa.Series)
                   .WithMany(s => s.SeriesActors)
                   .HasForeignKey(sa => sa.SeriesId);

            builder.HasOne(sa => sa.Actor)
                   .WithMany(a => a.SeriesActors)
                   .HasForeignKey(sa => sa.ActorId);

            builder.Property(sa => sa.CharacterName)
                   .HasMaxLength(100)
                   .IsRequired();
        }
    }
}
