using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedInfrastructure
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

            if (actionDescriptor != null)
            {
                context.Request.EnableBuffering();

                foreach (var parameter in actionDescriptor.Parameters)
                {
                    if (!parameter.BindingInfo?.BindingSource?.Id.Equals("Body", StringComparison.OrdinalIgnoreCase) ?? true)
                        continue;

                    if (context.Request.ContentLength > 0)
                    {
                        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                        var body = await reader.ReadToEndAsync();

                        if (string.IsNullOrWhiteSpace(body))
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("Тіло запиту не може бути порожнім.");
                            return;
                        }

                        context.Request.Body.Position = 0;

                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        object argument;
                        try
                        {
                            argument = JsonSerializer.Deserialize(body, parameter.ParameterType, options);
                        }
                        catch (JsonException ex)
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync($"Невірний формат JSON: {ex.Message}");
                            return;
                        }

                        if (argument is IEnumerable<object> collection) 
                        {
                            foreach (var item in collection)
                            {
                                var itemValidatorType = typeof(IValidator<>).MakeGenericType(item.GetType());
                                var itemValidator = context.RequestServices.GetService(itemValidatorType) as IValidator;

                                if (itemValidator != null)
                                {
                                    var result = await itemValidator.ValidateAsync(new ValidationContext<object>(item));

                                    if (!result.IsValid)
                                    {
                                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                        await context.Response.WriteAsJsonAsync(result.Errors);
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);
                            var validator = context.RequestServices.GetService(validatorType) as IValidator;

                            if (validator != null)
                            {
                                var result = await validator.ValidateAsync(new ValidationContext<object>(argument));

                                if (!result.IsValid)
                                {
                                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                    await context.Response.WriteAsJsonAsync(result.Errors);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            await _requestDelegate(context);
        }

    }
}
