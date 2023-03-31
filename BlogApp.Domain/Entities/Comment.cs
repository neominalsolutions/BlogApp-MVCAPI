using BlogApp.Domain.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Entities
{
    public class Comment : BaseEntity<string>
    {

        public string Text { get; set; }
        public string By { get; set; }
        public DateTime? IssuedAt { get; set; }


      // Navigation Property, Comment Hangi Posta ait
      public Post Post { get; set; }

       // Fk alanı
       public string PostId { get; set; }



    public Comment(string by, string text)
        {
            Id = Guid.NewGuid().ToString();
            IssuedAt = DateTime.Now;
        }

    }
}
