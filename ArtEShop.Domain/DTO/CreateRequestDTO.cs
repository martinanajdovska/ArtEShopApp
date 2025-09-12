using ArtEShop.Domain.IdentityModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Domain.DTO
{
    public class CreateRequestDTO
    {
        public string? Description { get; set; }
        public IFormFile? ReferenceImageFile { get; set; }
        public string? Subject { get; set; }
        public string? Email { get; set; }
    }
}
