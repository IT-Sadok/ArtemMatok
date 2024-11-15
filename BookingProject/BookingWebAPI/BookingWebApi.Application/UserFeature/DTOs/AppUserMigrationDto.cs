using BookingWebApi.Application.ApartamentFeature.DTOs;

namespace BookingWebApi.Application.DTOs.AppUserDTOs;

public record AppUserMigrationDto(
    string Id,
    string SourceCompanyId,
    string? UserName,
    string? Email,
    List<ApartamentMigrationDto> Apartaments
);