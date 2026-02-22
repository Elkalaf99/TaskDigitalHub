using MediatR;

namespace TaskDigitalhub.Application.Tasks.Commands;

public record DeleteTaskCommand(int Id) : IRequest<bool>;
