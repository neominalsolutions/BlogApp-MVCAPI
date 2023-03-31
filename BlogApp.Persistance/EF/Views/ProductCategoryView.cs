using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Persistance.EF.Views
{
  // store procedure CategoryName, ProductName alanları ile eşleşmek zorundadır.
  public class ProductCategoryView
  {
    public string CategoryName { get; set; }
    public string ProductName { get; set; }

  }
}
