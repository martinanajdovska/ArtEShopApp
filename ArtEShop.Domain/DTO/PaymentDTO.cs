using ArtEShop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Domain.DTO
{
    public class PaymentDTO
    {
        public string? CardName { get; set; }
        public int? CardNumber { get; set; }
        public string? ExpiryDate { get; set; }
        public int? CVV { get; set; }
        public string? BillingAddress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public int? ZipCode { get; set; }
        public string? Email { get; set; }
        public int? TotalPrice { get; set; }
        public Guid? ArtPieceId { get; set; }
    }
}
