using ArtEShop.Domain.DomainModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Domain.DTO
{
    public class RequestDTO
    {
        public Guid RequestId { get; set; }
        public string? Description { get; set; }
        public string? ReferenceImage { get; set; }
        public int? Price { get; set; }
        public string? ArtistNotes { get; set; }
        public string? Subject { get; set; }
        public bool? IsAnswered { get; set; }

    }
}
