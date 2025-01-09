using Payment.Domain.Models;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Interfaces.OutboxInterface
{
    public interface IOutboxRepository
    {
        Task<Result<bool>> AddAsync(OutboxEvent outbox);
        Task<List<OutboxEvent>> GetUnprocessedOutboxEventsAsync();
        Task<Result<bool>> MarkOutboxEventAsProcessedAsync(Guid outboxEventId);
    }
}
