using BlogApp.Domain.Entities;
using BlogApp.Domain.Repositories;
using BlogApp.Persistance.EF.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infra.Persistance.EF.Repositories
{
  public class EFCategoryRepository : ICategoryRepository
  {


    private readonly BlogAppContext _blogAppContext;


    public EFCategoryRepository(BlogAppContext blogAppContext)
    {
      _blogAppContext = blogAppContext;
    }

    public async Task AddAsync(Category Entity)
    {
      await _blogAppContext.Categories.AddAsync(Entity);
      await _blogAppContext.SaveChangesAsync();
    }

    public async Task<Category> FindAsync(Expression<Func<Category, bool>> lamda)
    {
      return await _blogAppContext.Categories.AsNoTracking().FirstOrDefaultAsync(lamda);
    }

    public async Task<List<Category>> ListAsync()
    {
      return await _blogAppContext.Categories.AsNoTracking().ToListAsync();
    }

    public Task<List<Category>> WhereAsync(Expression<Func<Category, bool>> lamda)
    {
      throw new NotImplementedException();
    }
  }
}
