namespace TaskDigitalhub.Application.Auth.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public AuthResponseDto() { }

    public AuthResponseDto(string token, int userId, string userName, string email, string role)
    {
        Token = token;
        UserId = userId;
        UserName = userName;
        Email = email;
        Role = role;
    }
}
