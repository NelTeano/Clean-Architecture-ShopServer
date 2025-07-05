using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyServer.Application.Commands.Payment;
using MyServer.Infrastructure.Services;
using Stripe;
using Stripe.Checkout;

namespace MyServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(ISender sender) : ControllerBase
    {
        [HttpPost("checkout-session")]
        public async Task<ActionResult<CreateCheckoutSessionResponse>> CreateCheckoutSession(
            [FromBody] CreateCheckoutSessionCommand command,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await sender.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Or use proper logging like ILogger
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            const string endpointSecret = "whsec_bccbd718d70143ca61892932ac13e2d008295163be9cf212eff386f60cacfdb7";

            try
            {
                var signatureHeader = Request.Headers["Stripe-Signature"];
                var stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);

                // Add debug logging to see what events we're receiving
                Console.WriteLine($"🔍 Received Stripe webhook event: {stripeEvent.Type}");

                switch (stripeEvent.Type)
                {
                    // Checkout Session Events
                    case "checkout.session.completed":
                        var session = stripeEvent.Data.Object as Session;
                        Console.WriteLine($"✅ Checkout session completed: {session?.Id} - Payment Status: {session?.PaymentStatus}");
                        break;

                    case "checkout.session.async_payment_succeeded":
                        var asyncSession = stripeEvent.Data.Object as Session;
                        Console.WriteLine($"🎉 Async payment succeeded: {asyncSession?.Id}");
                        break;

                    case "checkout.session.async_payment_failed":
                        var failedSession = stripeEvent.Data.Object as Session;
                        Console.WriteLine($"❌ Async payment failed: {failedSession?.Id}");
                        break;

                    // Payment Intent Events
                    case "payment_intent.succeeded":
                        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        Console.WriteLine($"💰 PaymentIntent succeeded: {paymentIntent?.Id}");
                        break;

                    case "payment_intent.created":
                        var createdPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        Console.WriteLine($"🆕 PaymentIntent created: {createdPaymentIntent?.Id}");
                        break;

                    case "payment_intent.canceled":
                        var canceledPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        Console.WriteLine($"❌ PaymentIntent canceled: {canceledPaymentIntent?.Id} - Reason: {canceledPaymentIntent?.CancellationReason}");
                        break;

                    case "payment_intent.payment_failed":
                        var failedPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        Console.WriteLine($"⚠️ PaymentIntent failed: {failedPaymentIntent?.Id} - {failedPaymentIntent?.LastPaymentError?.Message}");
                        break;

                    // Payment Method Events
                    case "payment_method.attached":
                        var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                        Console.WriteLine($"💳 PaymentMethod attached: {paymentMethod?.Id}");
                        break;

                    // Customer Events
                    case "customer.created":
                        var customer = stripeEvent.Data.Object as Customer;
                        Console.WriteLine($"👤 Customer created: {customer?.Id} - {customer?.Email}");
                        break;

                    // Charge Events
                    case "charge.succeeded":
                        var charge = stripeEvent.Data.Object as Charge;
                        Console.WriteLine($"💸 Charge succeeded: {charge?.Id} - Amount: {charge?.Amount}");
                        break;

                    case "charge.failed":
                        var failedCharge = stripeEvent.Data.Object as Charge;
                        Console.WriteLine($"💸 Charge failed: {failedCharge?.Id} - {failedCharge?.FailureMessage}");
                        break;

                    // Invoice Events
                    case "invoice.payment_failed":
                        var failedInvoice = stripeEvent.Data.Object as Invoice;
                        Console.WriteLine($"📄 Invoice payment failed: {failedInvoice?.Id} - Amount: {failedInvoice?.AmountDue}");
                        break;

                    default:
                        Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine($"⚠️ Stripe exception: {e.Message}");
                return BadRequest();
            }
        }
    }
}