using MediatR;
using TaskDigitalhub.Application.Auth.DTOs;

namespace TaskDigitalhub.Application.Auth.Commands;

public record RegisterCommand(RegisterDto Dto) : IRequest<AuthResponseDto?>;
