using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Application.Commands.Payment
{
    public class CreateCheckoutSessionCommand : IRequest<CreateCheckoutSessionResponse>
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string SuccessUrl { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string CancelUrl { get; set; } = string.Empty;

        [MaxLength(3)]
        public string Currency { get; set; } = "PHP";
    }
}
