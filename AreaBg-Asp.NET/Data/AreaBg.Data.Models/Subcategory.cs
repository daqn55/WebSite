using System;
using System.Collections.Generic;

namespace AreaBg.Data.Models
{
    public class Subcategory
    {
        public Subcategory()
        {
            this.Books = new HashSet<Book>();
        }

        public int SubcategoryId { get; set; }

        public string Title { get; set; }

        public int OrderNumber { get; set; }

        public Category Category { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
