using MyServer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Core.Interfaces
{
        public interface IPaymentRepository // Fixed: Should be interface, not class
        {
            Task<PaymentEntity> CreateAsync(PaymentEntity payment, CancellationToken cancellationToken = default);
            Task<PaymentEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
            Task<PaymentEntity?> GetByStripeSessionIdAsync(string stripeSessionId, CancellationToken cancellationToken = default);
            Task<PaymentEntity> UpdateAsync(PaymentEntity payment, CancellationToken cancellationToken = default);
        }
}
