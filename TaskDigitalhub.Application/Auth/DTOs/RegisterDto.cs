using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Application.Auth.DTOs;

public record RegisterDto(string UserName, string Email, string Password, UserRole Role);
