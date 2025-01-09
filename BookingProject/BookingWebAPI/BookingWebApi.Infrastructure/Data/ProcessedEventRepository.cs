using BookingWebApi.Application.Common.Interfaces;
using BookingWebApi.Domain.Entities;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Infrastructure.Data
{
    public class ProcessedEventRepository(ApplicationDbContext _context) : IProcessedEventRepository
    {
        public async Task<Result<bool>> AddProcessedEventAsync(ProcessedEvent processedEvent)
        {
            try
            {
                await _context.ProcessedEvents.AddAsync(processedEvent);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }

        public async Task<bool> ExistProcessedEventAsync(string processedEventId)
        {
            var processedEvent = await _context.ProcessedEvents.FindAsync(processedEventId);
            if (processedEvent is null) return false;

            return true;
        }
    }
}
