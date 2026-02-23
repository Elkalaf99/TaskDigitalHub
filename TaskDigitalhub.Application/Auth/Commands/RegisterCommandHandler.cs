using MediatR;
using TaskDigitalhub.Application.Auth.DTOs;
using TaskDigitalhub.Application.Common.Interfaces;
using TaskDigitalhub.Domain.Entities;

namespace TaskDigitalhub.Application.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto?> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByEmailAsync(request.Dto.Email) != null)
            return null;

        if (await _userRepository.GetByUserNameAsync(request.Dto.UserName) != null)
            return null;

        var user = new User
        {
            UserName = request.Dto.UserName,
            Email = request.Dto.Email,
            PasswordHash = _passwordHasher.Hash(request.Dto.Password),
            Role = request.Dto.Role
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user.Id, user.UserName, user.Email, user.Role.ToString());
        return new AuthResponseDto(token, user.Id, user.UserName, user.Email, user.Role.ToString());
    }
}
