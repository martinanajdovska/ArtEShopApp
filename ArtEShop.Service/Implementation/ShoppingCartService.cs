using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using ArtEShop.Domain.Email;
using ArtEShop.Domain.IdentityModels;
using ArtEShop.Repository.Interface;
using ArtEShop.Service.Interface;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IShoppingCartItemService _shoppingCartItemService;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IShoppingCartItemService shoppingCartItemService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartItemService = shoppingCartItemService;
        }

        public void DeleteItemFromShoppingCart(Guid artPieceId, Guid shoppingCartId)
        {
            if (artPieceId == Guid.Empty || shoppingCartId == Guid.Empty)
            {
                throw new Exception("Invalid art piece id");
            }

            if (shoppingCartId == Guid.Empty)
            {
                throw new Exception("Invalid shopping cart id");
            }

            var shoppingCartItem = _shoppingCartItemService.GetByShoppingCartIdAndArtPieceId(shoppingCartId,artPieceId);

            if (shoppingCartItem == null)
            {
                throw new Exception("Product in shopping cart not found");
            }

            _shoppingCartItemService.Delete(shoppingCartItem);
        }

        public ShoppingCartDTO GetByUserIdWithIncludedProducts(Guid userId)
        {
            var shoppingCart = GetByUserId(userId);
            if (shoppingCart == null)
            {
                throw new Exception("Shopping cart not found");
            }

            List<ShoppingCartItem> shoppingCartItems = _shoppingCartItemService.GetAllByShoppingCartId(shoppingCart.Id);
            List<ShoppingCartItemDTO> shoppingCartItemsDTO = new List<ShoppingCartItemDTO>();

            foreach (var item in shoppingCartItems)
            {
                shoppingCartItemsDTO.Add(new ShoppingCartItemDTO
                {
                    ArtPiece = item.ArtPiece,
                    Quantity = item.Quantity,
                    TotalPrice = item.Quantity * item.ArtPiece.Price
                });
            }

            ShoppingCartDTO shoppingCartDTO = new ShoppingCartDTO();

            if (shoppingCartItemsDTO.IsNullOrEmpty())
            {
                shoppingCartDTO.TotalPrice = 0;
            }
            else
            {
                shoppingCartDTO.TotalPrice = shoppingCartItemsDTO.Sum(x => x.TotalPrice);
            }

            shoppingCartDTO.ShoppingCartItemsDTO = shoppingCartItemsDTO;

            return shoppingCartDTO;
        }

        public ShoppingCart? GetByUserId(Guid userId)
        {
            return _shoppingCartRepository.Get(selector: x => x, predicate: x => x.OwnerId.Equals(userId.ToString()));
        }



        public ShoppingCart Insert()
        {
            return _shoppingCartRepository.Insert(new ShoppingCart());
        }
    }
}
