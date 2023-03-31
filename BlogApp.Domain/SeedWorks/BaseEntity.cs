using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.SeedWorks
{
  public abstract class BaseEntity<TKey>: IEntity
  {
    // PK alan yoksa entity olamaz.
    // CreateAt
    // UpdatedAt
    // DeletedAt
    // CreatedBy
    // UpdatedBy
    // DeletedBy
    // IsActive
    // IsDeleted
    public TKey Id { get; set; }
  }
}
