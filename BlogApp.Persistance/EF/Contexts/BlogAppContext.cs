using BlogApp.Domain.Entities;
using BlogApp.Persistance.EF.Configurations;
using BlogApp.Persistance.EF.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Persistance.EF.Contexts
{
    public class BlogAppContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public BlogAppContext()
        {

        }

        // bu ops değeri sayesinde farklı veri kaynaklarına bağlanabiliyor.
        // addDbContext(Config.useNpgl())
        public BlogAppContext(DbContextOptions<BlogAppContext> opt, IConfiguration configuration) : base(opt)
        {
          Configuration = configuration;
        }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("BlogContextConn"));

        base.OnConfiguring(optionsBuilder);
      }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // view configürasyonu
      modelBuilder.Entity<ProductCategoryView>().ToView("ProductCategoryView");
      modelBuilder.Entity<ProductCategoryView>().HasNoKey();


      modelBuilder.ApplyConfiguration(new PostConfiguration());

      // nesnelerin veri tabanı üzerindeki konfigürasyon ayarları için kullanıyoruz.
      base.OnModelCreating(modelBuilder);
    }

    // EF için Eğer Commentslere arayüzden direkt olarak bğlanmak istemiyorsak DbSet yazmak zorunda değilidi.
    public DbSet<Post> Posts { get; set; }

    // Program tarafından ulaşıcalack nesneleri sonradan yazdığımızda ise bir migration almamız lazım
    public DbSet<Category> Categories { get; set; }

    public DbSet<Tag> Tags { get; set; }

    // View de olsa DbSet ile tanımlarız.
    public DbSet<ProductCategoryView> PcView { get; set; }



        //public DbSet<Comment> Comments { get; set; }







    }
}
