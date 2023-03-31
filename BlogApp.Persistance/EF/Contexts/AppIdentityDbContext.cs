using BlogApp.Persistance.EF.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Persistance.EF.Contexts
{
  // kendi ApplicationUser ve kendi ApplicationRole tabloları ile IdentityDbContext çalışacak
  public class AppIdentityDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, string>
  {

    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      // Normalde tablolar AspNet ön takısı ile gelir.
      builder.Entity<ApplicationUser>().ToTable("Users");
      builder.Entity<ApplicationRole>().ToTable("Roles");
      builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
      builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
      builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
      builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
      builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");


    }


  }
}
