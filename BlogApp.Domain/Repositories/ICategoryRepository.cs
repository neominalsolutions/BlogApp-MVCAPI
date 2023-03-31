using BlogApp.Domain.Entities;
using BlogApp.Domain.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Repositories
{
  public interface ICategoryRepository:IRepository<Category>
  {
  }
}
