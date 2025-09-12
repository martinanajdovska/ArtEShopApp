using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Domain.DomainModels
{
    public class ShoppingCartItem : BaseEntity
    {
        public ArtPiece? ArtPiece { get; set; }
        public int? Quantity { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        public Order? Order { get; set; }

    }
}
