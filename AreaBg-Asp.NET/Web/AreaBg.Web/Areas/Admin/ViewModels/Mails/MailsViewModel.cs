using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.ViewModels.Mails
{
    public class MailsViewModel
    {
        public int id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public bool Archived { get; set; }

        public string Date { get; set; }
    }
}
