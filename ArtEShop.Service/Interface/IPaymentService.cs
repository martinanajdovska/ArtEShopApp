using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Interface
{
    public interface IPaymentService
    {
        Task<bool> PayOrder(PaymentDTO paymentDTO, Order order);
    }
}
