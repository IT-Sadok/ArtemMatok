using BookingWebApi.Domain.Entities;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Common.Interfaces
{
    public interface IProcessedEventRepository
    {
        Task<Result<bool>> AddProcessedEventAsync(ProcessedEvent processedEvent);
        Task<bool> ExistProcessedEventAsync(string processedEventId);
    }
}
