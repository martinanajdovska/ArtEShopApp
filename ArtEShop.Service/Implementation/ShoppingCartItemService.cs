using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using ArtEShop.Repository.Interface;
using ArtEShop.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Implementation
{
    public class ShoppingCartItemService : IShoppingCartItemService
    {
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;

        public ShoppingCartItemService(IRepository<ShoppingCartItem> shoppingCartItemRepository)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
        }

        public ShoppingCartItem Delete(ShoppingCartItem shoppingCartItem)
        {
            return _shoppingCartItemRepository.Delete(shoppingCartItem);
        }

        public List<ShoppingCartItem> GetAllByOrderId(Guid orderId)
        {
            return _shoppingCartItemRepository.GetAll(selector: x => x,
                            predicate: x => x.Order.Id.Equals(orderId),
                            include: x => x.Include(z => z.ArtPiece).Include(z => z.ShoppingCart).Include(z => z.Order)).ToList();
        }

        public List<ShoppingCartItem> GetAllByShoppingCartId(Guid shoppingCartId)
        {
            return _shoppingCartItemRepository.GetAll(selector: x => x,
                predicate: x => x.ShoppingCart.Id.Equals(shoppingCartId),
                include: x => x.Include(z => z.ArtPiece).Include(z => z.ShoppingCart).Include(z => z.Order)).ToList();
        }

        public ShoppingCartItem? GetById(Guid id)
        {
            return _shoppingCartItemRepository.Get(selector: x => x,
                                                      predicate: x => x.Id.Equals(id),
                                                      include: x => x.Include(z => z.ArtPiece).Include(z => z.ShoppingCart).Include(z => z.Order));
        }

        public ShoppingCartItem GetAllByShoppingCartIdAndArtPieceId(Guid shoppingCartId, Guid artPieceId)
        {
            return _shoppingCartItemRepository.Get(selector: x => x,
                                                      predicate: x => x.ShoppingCart.Id.Equals(shoppingCartId) && x.ArtPiece.Id.Equals(artPieceId),
                                                      include: x=>x.Include(z=>z.ArtPiece).Include(z=>z.ShoppingCart).Include(z=>z.Order));
        }

        public ShoppingCartItem Update(ShoppingCartItem shoppingCartItem)
        {
            return _shoppingCartItemRepository.Update(shoppingCartItem);
        }

        public ShoppingCartItem Insert(ShoppingCartItem shoppingCartItem)
        {
            return _shoppingCartItemRepository.Insert(shoppingCartItem);
        }
    }
}
