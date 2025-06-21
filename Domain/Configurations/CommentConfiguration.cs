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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(c => c.UserId)
                  .IsRequired();

            builder.Property(c => c.Content)
                   .IsRequired()
                   .HasMaxLength(2000); 

            builder.HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Movie)
                   .WithMany(m => m.Comments)
                   .HasForeignKey(c => c.MovieId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(c => c.Series)
                   .WithMany(s => s.Comments)
                   .HasForeignKey(c => c.SeriesId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
