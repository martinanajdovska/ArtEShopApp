using ArtEShop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Domain.DTO
{
    public class AddToCartDTO
    {
        public ArtPiece? ArtPiece { get; set; }
        public int? Quantity { get; set; }
    }
}
