using BlogApp.Domain.Entities;
using BlogApp.Domain.Repositories;
using BlogApp.Domain.SeedWorks;
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
  public class PostRepository : IPostRepository
  {

    // Veri tabanı balantısı ve veri tabanındaki Entity Stateleri yönettiğimiz kısım. DB bazlı EF Altyapı hizmeti


    private readonly BlogAppContext _blogAppContext;


    public PostRepository(BlogAppContext blogAppContext)
    {
      _blogAppContext = blogAppContext;
    }

    public async Task AddAsync(Post Entity)
    {

      await _blogAppContext.Posts.AddAsync(Entity);
      await _blogAppContext.SaveChangesAsync();
     
       
    }


    public async Task<Post> FindAsync(Expression<Func<Post, bool>> lamda)
    {// Post ile birlikte Tags,Comments,Category çekilmeli
      return await _blogAppContext.Posts
        .Include(x=> x.Category)
        .Include(x=> x.Tags)
        .Include(x=> x.Commments)
        .AsNoTracking()
        .FirstOrDefaultAsync(lamda);
    }

    /// <summary>
    /// Bütün gönderileri listeler
    /// </summary>
    /// <returns></returns>
    public async Task<List<Post>> ListAsync()
    {
      // Post ile birlikte Tags,Comments,Category çekilmeli
      // Include ThenInclude yapmamız gerekecek.
      return await _blogAppContext.Posts
        .Include(x=> x.Category)
        .Include(x=> x.Tags)
        .Include(x=> x.Commments)
        .AsNoTracking()
        .ToListAsync();
    }

    /// <summary>
    /// Kritere göre sorgulama
    /// </summary>
    /// <param name="lamda"></param>
    /// <returns></returns>
    public async Task<List<Post>> WhereAsync(Expression<Func<Post, bool>> lamda)
    {// Post ile birlikte Tags,Comments,Category çekilmeli
      return await _blogAppContext.Posts.Include(x=> x.Category).Include(x=> x.Tags).Include(x=> x.Commments).AsNoTracking().Where(lamda).ToListAsync();
    }
  }
}
