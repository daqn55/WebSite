using System;
using System.Collections.Generic;
using System.Text;

namespace AreaBg.Data.Models
{
    public class BookReview
    {
        public int Id { get; set; }

        public DateTime date { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public int Stars { get; set; }

        public bool IsApproved { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}
