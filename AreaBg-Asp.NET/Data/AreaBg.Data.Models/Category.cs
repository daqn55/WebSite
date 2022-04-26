using System;
using System.Collections.Generic;

namespace AreaBg.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Subcategories = new HashSet<Subcategory>();
        }

        public int CategoryId { get; set; }

        public string Title { get; set; }

        public int OrderNumber { get; set; }

        public virtual ICollection<Subcategory> Subcategories { get; set; }
    }
}
