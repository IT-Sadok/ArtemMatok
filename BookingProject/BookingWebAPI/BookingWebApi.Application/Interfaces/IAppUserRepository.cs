using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Interfaces
{
    public interface IAppUserRepository
    {
        Task<bool> IsUserExist(string userId);
    }
}
