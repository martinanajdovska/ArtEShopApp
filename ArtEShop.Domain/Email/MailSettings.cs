using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Domain.Email
{
    public class MailSettings
    {
        public string? SendersName { get; set; }
        public string? SmtpUserName { get; set; }
        public string? SmtpPassword { get; set; }
    }
}
