using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Entities
{
    public class OrderItemEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; } // Price at time of order

        public decimal TotalPrice { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    }
}
