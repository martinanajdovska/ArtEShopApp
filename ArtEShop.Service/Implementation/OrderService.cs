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
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;
        private readonly IShoppingCartService _shoppingCartService;

        public OrderService(IRepository<Order> orderRepository, IRepository<ShoppingCartItem> shoppingCartItemRepository, IShoppingCartService shoppingCartService)
        {
            _orderRepository = orderRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _shoppingCartService = shoppingCartService;
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
            order.PurchasedItems = new List<ShoppingCartItem>();
            order.OwnerId = userId;

            if (artPieceId != null)
            {
                var shoppingCartItem = _shoppingCartItemRepository.Get(selector: x => x,
                                                                             predicate: x => x.ShoppingCart.Id.Equals(shoppingCart.Id) && x.ArtPiece.Id.Equals(artPieceId),
                                                                             include: x => x.Include(z => z.ArtPiece));

                if (shoppingCartItem == null)
                {
                    throw new Exception("Item not found");
                }
                order.PurchasedItems.Add(shoppingCartItem);
                order.TotalPrice = (int)(shoppingCartItem.ArtPiece.Price * shoppingCartItem.Quantity);
            }
            else
            {
                List<ShoppingCartItem> shoppingCartItems = _shoppingCartItemRepository.GetAll(selector: x => x,
                    predicate: x => x.ShoppingCart.Id.Equals(shoppingCart.Id),
                    include: x => x.Include(z => z.ArtPiece)).ToList();

                order.PurchasedItems = shoppingCartItems;
                order.TotalPrice = (int)shoppingCartItems.Sum(x => x.Quantity * x.ArtPiece.Price);
            }
            return order;
        }
    }
}
