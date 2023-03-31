using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Persistance.EF.Identity
{
  // nesneyi extend ettik
  public class ApplicationUser: IdentityUser
  {
    public string? WebSite { get; set; }
  }
}
