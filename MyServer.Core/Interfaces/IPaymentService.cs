using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe.Checkout;

namespace MyServer.Core.Interfaces
{
    public interface IPaymentService
    {
        Session CreateCheckoutSession(string successUrl, string cancelUrl, decimal amount, string productName);
    }
}
