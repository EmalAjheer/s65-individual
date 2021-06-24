using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tweet_service.Models.config
{
    public class TweetConfig : IEntityTypeConfiguration<Tweet>
    {
        public void Configure(EntityTypeBuilder<Tweet> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Date_Created)
                .HasColumnType("TIMESTAMP(0)")
                .IsRequired();

            builder.Property(prop => prop.UserId)
                .IsRequired();

            builder.Property(prop => prop.Date_Updated);
            builder.Property(prop => prop.HeartCount);
            builder.Property(prop => prop.Description).IsRequired().HasMaxLength(140);

        }
    }
}
