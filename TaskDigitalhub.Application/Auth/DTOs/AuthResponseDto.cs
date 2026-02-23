namespace TaskDigitalhub.Application.Auth.DTOs;

public record AuthResponseDto(string Token, int UserId, string UserName, string Email, string Role);
