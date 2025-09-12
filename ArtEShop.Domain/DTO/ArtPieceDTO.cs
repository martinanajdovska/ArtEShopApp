using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ArtEShop.Domain.DTO
{
    public class ArtPieceDTO
    {

        public string? Name { get; set; }
        public int? Price { get; set; }
        public IFormFile? ImageFile { get; set; }

    }
}
