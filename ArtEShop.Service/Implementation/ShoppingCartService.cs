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
        private readonly IRepository<Order> _orderRepository;
        private readonly IShoppingCartItemService _shoppingCartItemService;
        private readonly IEmailService _emailService;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<Order> orderRepository, IShoppingCartItemService shoppingCartItemService, IEmailService emailService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
            _shoppingCartItemService = shoppingCartItemService;
            _emailService = emailService;
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

            var shoppingCartItem = _shoppingCartItemService.GetAllByShoppingCartIdAndArtPieceId(shoppingCartId,artPieceId);

            if (shoppingCartItem == null)
            {
                throw new Exception("Product in shopping cart not found");
            }

            _shoppingCartItemService.Delete(shoppingCartItem);
        }

        public async Task<bool> PayOrder(PaymentDTO paymentDTO, Order order)
        {
            if (paymentDTO == null)
            {
                throw new Exception("Payment details are missing");
            }

            if (order == null)
            {
                throw new Exception("Order is null");
            }

            var emailMessage = new EmailMessage();
            emailMessage.MailTo = paymentDTO.Email;

            emailMessage.Subject = "Purchase successful";

            List<ShoppingCartItem> purchasedItems = _shoppingCartItemService.GetAllByOrderId(order.Id);

            if (purchasedItems.IsNullOrEmpty())
            {
                throw new Exception("No items found for this order");
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in purchasedItems)
            {
                sb.Append($"You have successfully purchased {item.ArtPiece.Name} for " +
                $"{item.ArtPiece.Price * item.Quantity} ДЕН. for a total amount of {item.Quantity}\n");

                item.ShoppingCart = null;
                item.Order = order;
                _shoppingCartItemService.Update(item);
            }
            sb.Append($"Total price: {order.TotalPrice}");
            emailMessage.Content = sb.ToString();

            _orderRepository.Insert(order);
            await _emailService.SendEmailAsync(emailMessage);
            return true;
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
