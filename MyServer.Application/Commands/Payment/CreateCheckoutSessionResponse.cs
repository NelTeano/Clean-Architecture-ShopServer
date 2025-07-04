using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Application.Commands.Payment
{
    public class CreateCheckoutSessionResponse
    {
        public Guid PaymentId { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public string SessionUrl { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
    }

}
