// MyServer.Infrastructure/Services/StripePaymentService.cs
using Microsoft.Extensions.Options;
using MyServer.Core.Interfaces;
using MyServer.Infrastructure.Configurations;
using Stripe;
using Stripe.Checkout;

namespace MyServer.Infrastructure.Services
{
    public class StripePaymentService : IPaymentService
    {
        private readonly StripeSettings _stripeSettings;

        public StripePaymentService(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        public Session CreateCheckoutSession(string successUrl, string cancelUrl, decimal amount, string productName)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(amount * 100), // Convert to cents
                            Currency = "php", // Fixed: Changed from "usd" to "php"
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = productName,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }
    }
}