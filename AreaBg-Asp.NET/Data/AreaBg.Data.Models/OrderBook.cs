using System;
using System.Collections.Generic;
using System.Text;

namespace AreaBg.Data.Models
{
    public class OrderBook
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public decimal PriceWithDisc { get; set; }
    }
}
