using BlogApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Persistance.EF.Configurations
{
  public class PostConfiguration : IEntityTypeConfiguration<Post>
  {
    public void Configure(EntityTypeBuilder<Post> builder)
    {

      // Fluent API ile yazılan Config ayarları Default Constraint ayarlarını ezer yani buradaki ayarlar.

      builder.HasKey(x => x.Id); // PK
      builder.Property(x => x.Title).HasMaxLength(50);
      builder.HasIndex(x => x.Title).IsUnique(); // Unique Index tanımı
      builder.Property(x => x.Content).HasMaxLength(2000); // en fazla 2000 karakter olabilir


      // relations
      // Category Post Relation Ship
      builder.HasOne(x => x.Category).WithMany(x => x.Posts).HasForeignKey(x => x.CategoryId);

      // Comment Post ilişkisi
      builder.HasMany(x => x.Commments).WithOne(x => x.Post).HasForeignKey(x => x.PostId);

      // Tag ile Post çoka çok ilişkili
      builder.HasMany(x => x.Tags).WithMany(x => x.Posts);

    }
  }
}
