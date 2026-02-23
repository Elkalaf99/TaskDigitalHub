using MediatR;
using TaskDigitalhub.Application.Auth.DTOs;

namespace TaskDigitalhub.Application.Auth.Commands;

public record LoginCommand(LoginDto Dto) : IRequest<AuthResponseDto?>;
