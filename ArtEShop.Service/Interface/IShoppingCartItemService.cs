using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Interface
{
    public interface IShoppingCartItemService
    {
        List<ShoppingCartItem> GetAllByShoppingCartId(Guid shoppingCartId);
        List<ShoppingCartItem> GetAllByOrderId(Guid orderId);
        ShoppingCartItem? GetById(Guid id);
        ShoppingCartItem Insert(ShoppingCartItem shoppingCartItem);
        ShoppingCartItem Update(ShoppingCartItem shoppingCartItem);
        ShoppingCartItem Delete(ShoppingCartItem shoppingCartItem);
        ShoppingCartItem GetAllByShoppingCartIdAndArtPieceId(Guid shoppingCartId, Guid artPieceId);
    }
}
