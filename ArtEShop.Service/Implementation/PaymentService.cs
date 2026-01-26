using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using ArtEShop.Domain.Email;
using ArtEShop.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IShoppingCartItemService _shoppingCartItemService;
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;

        public PaymentService(IShoppingCartItemService shoppingCartItemService, IOrderService orderService, IEmailService emailService)
        {
            _shoppingCartItemService = shoppingCartItemService;
            _orderService = orderService;
            _emailService = emailService;
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

            _orderService.Insert(order);
            await _emailService.SendEmailAsync(emailMessage);
            return true;
        }
    }
}
