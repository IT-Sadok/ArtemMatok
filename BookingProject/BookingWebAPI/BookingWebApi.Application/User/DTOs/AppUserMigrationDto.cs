using BookingWebApi.Application.Apartament.DTOs;

namespace BookingWebApi.Application.User.DTOs;

public record AppUserMigrationDto(
    string Id,
    string SourceCompanyId,
    string? UserName,
    string? Email,
    List<ApartamentMigrationDto> Apartaments
);