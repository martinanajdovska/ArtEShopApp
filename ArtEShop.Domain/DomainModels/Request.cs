using ArtEShop.Domain.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Domain.DomainModels
{
    public class Request : BaseEntity
    {
        public string? Description { get; set; }
        public string? ReferenceImage { get; set; }
        public int? Price { get; set; }
        public string? ArtistNotes { get; set; }
        public bool? IsAnswered { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
    }
}
