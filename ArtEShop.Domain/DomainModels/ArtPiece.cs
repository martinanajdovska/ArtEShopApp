using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Domain.DomainModels
{
    public class ArtPiece : BaseEntity
    {
        public string? Name { get; set; }
        public int? Price { get; set; }
        public string? Image { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
