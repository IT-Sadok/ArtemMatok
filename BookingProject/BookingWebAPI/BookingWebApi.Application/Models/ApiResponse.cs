using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApi.Application.Models
{
    public class ApiResponse<T>
    {
        public T Value { get; set; }
        public string Description { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public ApiResponse(T value, string description)
        {
            Value = value;
            Description = description;
            Success = true;
            Errors = new List<string>();
        }

        public ApiResponse(string errorMessages, List<string> errors)
        {
            Success = false;
            ErrorMessage = errorMessages;   
            Errors = errors;    
        }

    }
}
