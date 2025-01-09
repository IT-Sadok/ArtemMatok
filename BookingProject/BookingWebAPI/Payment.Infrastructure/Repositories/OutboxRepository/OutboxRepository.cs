using Microsoft.EntityFrameworkCore;
using Payment.Domain.Models;
using Payment.Infrastructure.DataContext;
using Payment.Infrastructure.Interfaces.OutboxInterface;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories.OutboxRepository
{
    public class OutboxRepository(PaymentDbContext _context) : IOutboxRepository
    {
        public async Task<Result<bool>> AddAsync(OutboxEvent outbox)
        {
            try
            {
                await _context.AddAsync(outbox);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }

        public async Task<List<OutboxEvent>> GetUnprocessedOutboxEventsAsync()
        {
            return await _context.OutboxEvents
                .Where(x => !x.Processed)
                .ToListAsync();
        }

        public async Task<Result<bool>> MarkOutboxEventAsProcessedAsync(Guid outboxEventId)
        {
            var outbox = await _context.OutboxEvents.FindAsync(outboxEventId);
            if (outbox is null) return Result<bool>.Failure("This event wasn`t found");

            outbox.Processed = true;

            _context.Entry(outbox).Property(c => c.Processed).IsModified = true;
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
