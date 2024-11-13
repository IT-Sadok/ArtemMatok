namespace BookingWebApi.Application.DTOs.ApartamentDTOs;

public record ApartamentMigrationDto(
    string Id,
    string SourceCompanyId,
    string Address,
    double Area,
    decimal Latitude, 
    decimal Longtitude,
    int Bedrooms
);