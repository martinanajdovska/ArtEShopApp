using ArtEShop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Interface
{
    public interface IOrderService
    {
        List<Order> GetAll();
        Order? GetById(Guid id);
        Order Insert(Order order);
        Order CreateOrder(Guid? artPieceId, Guid userId);
    }
}
