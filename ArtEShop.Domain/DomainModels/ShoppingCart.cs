using ArtEShop.Domain.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public ApplicationUser? Owner { get; set; }
        public string? OwnerId { get; set; }
    }
}
