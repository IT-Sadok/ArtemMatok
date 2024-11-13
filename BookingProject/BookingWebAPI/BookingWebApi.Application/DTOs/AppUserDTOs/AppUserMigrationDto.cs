using BookingWebApi.Application.DTOs.ApartamentDTOs;

namespace BookingWebApi.Application.DTOs.AppUserDTOs;

public record AppUserMigrationDto(
    string Id,
    string SourceCompanyId,
    string? UserName,
    string? Email,
    List<ApartamentMigrationDto> Apartaments
);