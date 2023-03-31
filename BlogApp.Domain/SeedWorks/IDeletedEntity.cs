using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.SeedWorks
{
  public interface IDeletedEntity
  {
    public string DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }

  }
}
