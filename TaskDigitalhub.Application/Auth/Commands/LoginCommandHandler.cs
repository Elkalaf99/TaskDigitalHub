using MediatR;
using TaskDigitalhub.Application.Auth.DTOs;
using TaskDigitalhub.Application.Common.Interfaces;

namespace TaskDigitalhub.Application.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Dto.Email);
        if (user == null || !_passwordHasher.Verify(request.Dto.Password, user.PasswordHash))
            return null;

        var token = _jwtService.GenerateToken(user.Id, user.UserName, user.Email, user.Role.ToString());
        return new AuthResponseDto(token, user.Id, user.UserName, user.Email, user.Role.ToString());
    }
}
