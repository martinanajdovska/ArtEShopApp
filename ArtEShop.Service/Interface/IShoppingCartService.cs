using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCart? GetByUserId(Guid userId);
        ShoppingCartDTO GetByUserIdWithIncludedProducts(Guid userId);
        void DeleteItemFromShoppingCart(Guid artPieceId, Guid shoppingCartId);
        ShoppingCart Insert();
    }
}
