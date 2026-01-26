using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using ArtEShop.Repository.Interface;
using ArtEShop.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IShoppingCartItemService _shoppingCartItemService;

        public OrderService(IRepository<Order> orderRepository, IShoppingCartService shoppingCartService, IShoppingCartItemService shoppingCartItemService)
        {
            _orderRepository = orderRepository;
            _shoppingCartService = shoppingCartService;
            _shoppingCartItemService = shoppingCartItemService;
        }

        public List<Order> GetAll()
        {
            return _orderRepository.GetAll(selector: x => x).ToList();
        }

        public Order? GetById(Guid id)
        {
            return _orderRepository.Get(selector: x => x,
                                          predicate: x => x.Id.Equals(id));
        }

        public Order Insert(Order order)
        {
            order.Id = Guid.NewGuid();
            return _orderRepository.Insert(order);
        }

        public Order CreateOrder(Guid? artPieceId, Guid userId)
        {
            var shoppingCart = _shoppingCartService.GetByUserId(userId);
            if (shoppingCart == null)
            {
                throw new Exception("Shopping cart not found");
            }

            Order order = new Order();
            order.OwnerId = userId;

            if (artPieceId != null)
            {
                var shoppingCartItem = _shoppingCartItemService.GetByShoppingCartIdAndArtPieceId(shoppingCart.Id, artPieceId.Value);

                if (shoppingCartItem == null)
                {
                    throw new Exception("Item not found");
                }
                order.TotalPrice = (int)(shoppingCartItem.ArtPiece.Price * shoppingCartItem.Quantity);
                shoppingCartItem.Order = order;
                _shoppingCartItemService.Update(shoppingCartItem);
            }
            else
            {
                List<ShoppingCartItem> shoppingCartItems = _shoppingCartItemService.GetAllByShoppingCartId(shoppingCart.Id);

                order.TotalPrice = (int)shoppingCartItems.Sum(x => x.Quantity * x.ArtPiece.Price);
                foreach (var item in shoppingCartItems)
                {
                    item.Order = order;
                    _shoppingCartItemService.Update(item);
                }
            }
            return order;
        }
    }
}
