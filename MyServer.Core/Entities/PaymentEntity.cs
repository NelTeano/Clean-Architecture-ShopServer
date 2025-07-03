using MyServer.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Entities
{
    public class PaymentEntity
    {
        [Key]
        public Guid Id { get; set; } 

        [Required]
        [MaxLength(100)]
        public string StripePaymentIntentId { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "PHP";

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [Required]
        public Guid OrderId { get; set; } 

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedOn { get; set; }

        [MaxLength(200)]
        public string? ClientSecret { get; set; }

        [MaxLength(100)]
        public string? StripeChargeId { get; set; } 

        [MaxLength(500)]
        public string? FailureReason { get; set; } 
    }
}
