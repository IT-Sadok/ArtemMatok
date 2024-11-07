using FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace BookingWebApi.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ValidationMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

            if(actionDescriptor != null)
            {
                foreach (var parameter in actionDescriptor.Parameters)
                {
                    var argument = context.RequestServices.GetService(parameter.ParameterType);

                    //Dynamic creating type of validation
                    var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);
                    var validator = context.RequestServices.GetService(validatorType) as IValidator;

                    if(validator != null && argument != null)
                    {
                        var result = await validator.ValidateAsync(new ValidationContext<object>(argument));

                        if(!result.IsValid)
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsJsonAsync(result.Errors);
                            return;
                        }
                    }
                }
                await _requestDelegate(context);
            }
        }
    }
}
