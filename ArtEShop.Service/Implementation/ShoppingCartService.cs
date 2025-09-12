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
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IEmailService _emailService;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<ShoppingCartItem> shoppingCartItemRepository, IRepository<Order> orderRepository, IEmailService emailService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _orderRepository = orderRepository;
            _emailService = emailService;
        }

        public void DeleteItemFromShoppingCart(Guid artPieceId, Guid shoppingCartId)
        {
            var shoppingCartItem = _shoppingCartItemRepository.Get(selector: x => x,
                                                                             predicate: x => x.ShoppingCart.Id.Equals(shoppingCartId) && x.ArtPiece.Id.Equals(artPieceId));

            if (shoppingCartItem == null)
            {
                throw new Exception("Product in shopping cart not found");
            }

            _shoppingCartItemRepository.Delete(shoppingCartItem);
        }

        public async Task<bool> PayOrder(PaymentDTO paymentDTO, Order order)
        {
            var emailMessage = new EmailMessage();
            emailMessage.MailTo = paymentDTO.Email;

            emailMessage.Subject = "Purchase successful";

            StringBuilder sb = new StringBuilder();
            foreach (var item in order.PurchasedItems)
            {
                sb.Append($"You have successfully purchased {item.ArtPiece.Name} for " +
                $"{item.ArtPiece.Price * item.Quantity} ДЕН. for a total amount of {item.Quantity}\n");

                item.ShoppingCart = null;
                item.Order = order;
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

            List<ShoppingCartItemDTO> shoppingCartItemsDTO = _shoppingCartItemRepository.GetAll(selector: x => new ShoppingCartItemDTO
            {
                ArtPiece = x.ArtPiece,
                Quantity = x.Quantity,
                TotalPrice = x.Quantity * x.ArtPiece.Price
            },
                predicate: x => x.ShoppingCart.Id.Equals(shoppingCart.Id),
                include: x => x.Include(z => z.ArtPiece)).ToList();

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
