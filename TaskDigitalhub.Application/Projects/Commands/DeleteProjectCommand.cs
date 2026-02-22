using MediatR;

namespace TaskDigitalhub.Application.Projects.Commands;

public record DeleteProjectCommand(int Id) : IRequest<bool>;
