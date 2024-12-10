using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.User.Query
{
    public class UserCustomData
    { 
        public string Key { get; set; }  
        public string Value { get; set; }   
    }

    public class UserUpdateQuery
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public List<UserCustomData>? CustomUserData { get; set; }
    }
}
