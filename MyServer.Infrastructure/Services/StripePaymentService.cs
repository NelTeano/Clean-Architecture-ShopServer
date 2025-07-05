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
                            Currency = "php",
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

                // Add these configurations to ensure webhook events are triggered
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    CaptureMethod = "automatic",
                    SetupFutureUsage = null // Remove this if you don't want to save payment method
                },

                // Enable automatic tax calculation (optional)
                AutomaticTax = new SessionAutomaticTaxOptions
                {
                    Enabled = false
                }
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }
    }
}