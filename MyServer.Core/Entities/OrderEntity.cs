using System.ComponentModel.DataAnnotations;
using MyServer.Core.Enums;

namespace MyServer.Core.Entities
{
    public class OrderEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        public Guid UserId { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedOn { get; set; }

        [MaxLength(500)]
        public string? SpecialInstructions { get; set; }

        public OrderType Type { get; set; } = OrderType.Pickup;

        [MaxLength(500)]
        public string? DeliveryAddress { get; set; }

        public DateTime? RequestedDeliveryTime { get; set; }

    }
}
