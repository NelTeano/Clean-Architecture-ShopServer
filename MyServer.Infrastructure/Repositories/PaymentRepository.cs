using Microsoft.EntityFrameworkCore;
using MyServer.Core.Entities;
using MyServer.Core.Interfaces;
using MyServer.Infrastructure.Data;

namespace MyServer.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationContextDB _context; // Your DbContext

        public PaymentRepository(ApplicationContextDB context)
        {
            _context = context;
        }

        public async Task<PaymentEntity> CreateAsync(PaymentEntity payment, CancellationToken cancellationToken = default)
        {
            _context.Payment.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);
            return payment;
        }

        public async Task<PaymentEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Payment.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PaymentEntity?> GetByStripeSessionIdAsync(string stripeSessionId, CancellationToken cancellationToken = default)
        {
            return await _context.Payment.FirstOrDefaultAsync(p => p.StripeSessionId == stripeSessionId, cancellationToken);
        }

        public async Task<PaymentEntity> UpdateAsync(PaymentEntity payment, CancellationToken cancellationToken = default)
        {
            _context.Payment.Update(payment);
            await _context.SaveChangesAsync(cancellationToken);
            return payment;
        }
    }
}
