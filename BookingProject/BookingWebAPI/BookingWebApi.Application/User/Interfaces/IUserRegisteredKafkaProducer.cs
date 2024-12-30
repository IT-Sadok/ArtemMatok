using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.User.Interfaces
{
    public interface IUserRegisteredKafkaProducer
    {
        Task ProduceUserRegisteredEventAsync(string userId, string email);
    }
}
