using System;
using System.Collections.Generic;
using System.Text;

namespace AreaBg.Data.Models
{
    public class SiteEmail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string UserId { get; set; }

        public MyIdentityUser User { get; set; }

        public bool Archived { get; set; }

        public DateTime Date { get; set; }
    }
}
