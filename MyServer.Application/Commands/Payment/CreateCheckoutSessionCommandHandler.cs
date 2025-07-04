using MediatR;
using MyServer.Core.Entities;
using MyServer.Core.Enums;
using MyServer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Application.Commands.Payment
{
    public class CreateCheckoutSessionCommandHandler : IRequestHandler<CreateCheckoutSessionCommand, CreateCheckoutSessionResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentRepository _paymentRepository;

        public CreateCheckoutSessionCommandHandler(
            IPaymentService paymentService,
            IPaymentRepository paymentRepository)
        {
            _paymentService = paymentService;
            _paymentRepository = paymentRepository;
        }

        public async Task<CreateCheckoutSessionResponse> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
        {
            // Create Stripe checkout session
            var session = _paymentService.CreateCheckoutSession(
                request.SuccessUrl,
                request.CancelUrl,
                request.Amount,
                request.ProductName);

            // Create payment entity
            var payment = new PaymentEntity
            {
                Id = Guid.NewGuid(),
                StripeSessionId = session.Id,
                StripePaymentIntentId = session.PaymentIntentId ?? string.Empty,
                Amount = request.Amount,
                Currency = request.Currency,
                OrderId = request.OrderId,
                Status = PaymentStatus.Pending,
                ClientSecret = session.ClientSecret,
                CreatedOn = DateTime.UtcNow
            };

            // Save to database
            await _paymentRepository.CreateAsync(payment, cancellationToken);

            // Return response
            return new CreateCheckoutSessionResponse
            {
                PaymentId = payment.Id,
                SessionId = session.Id,
                SessionUrl = session.Url,
                ClientSecret = session.ClientSecret ?? string.Empty,
                Amount = request.Amount,
                Currency = request.Currency
            };
        }
    }
}
