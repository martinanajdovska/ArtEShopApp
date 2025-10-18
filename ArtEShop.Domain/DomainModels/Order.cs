using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public int? TotalPrice { get; set; }
        public Guid? OwnerId { get; set; }
    }
}
