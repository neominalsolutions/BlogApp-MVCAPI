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
  public class TagRepository : ITagRepository
  {

    private readonly BlogAppContext _blogAppContext;


    public TagRepository(BlogAppContext blogAppContext)
    {
      _blogAppContext = blogAppContext;
    }

    public async Task AddAsync(Tag Entity)
    {
      await _blogAppContext.Tags.AddAsync(Entity);
      await _blogAppContext.SaveChangesAsync();
    }

    public Task<Tag> FindAsync(Expression<Func<Tag, bool>> lamda)
    {
      return _blogAppContext.Tags.AsNoTracking().FirstOrDefaultAsync(lamda);
    }

    public Task<List<Tag>> ListAsync()
    {
      return _blogAppContext.Tags.AsNoTracking().ToListAsync();
    }

    public Task<List<Tag>> WhereAsync(Expression<Func<Tag, bool>> lamda)
    {
      throw new NotImplementedException();
    }
  }
}
