using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.SeedWorks
{

  // Code Defensing ile sadece IEntity Interface impement olan sınıf ile çalış
  public interface IRepository<T> where T: IEntity
  {
    /// <summary>
    /// Yeni bir entity db eklemek için
    /// </summary>
    /// <param name="Entity"></param>
    /// <returns></returns>
    Task AddAsync(T Entity);

    /// <summary>
    /// Tek bir entity bulmak için
    /// </summary>
    /// <param name="lamda"></param>
    /// <returns></returns>
    Task<T> FindAsync(Expression<Func<T, bool>> lamda);

    /// <summary>
    /// Db Entityleri listelemek için tüm liste
    /// </summary>
    /// <returns></returns>
    Task<List<T>> ListAsync();

    /// <summary>
    /// DbEntityleri bir kritere göre filterelemek için
    /// </summary>
    /// <param name="lamda"></param>
    /// <returns></returns>
    Task<List<T>> WhereAsync(Expression<Func<T, bool>> lamda);
  }
}
