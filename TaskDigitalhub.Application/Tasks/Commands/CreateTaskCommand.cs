using MediatR;
using TaskDigitalhub.Application.Tasks.DTOs;

namespace TaskDigitalhub.Application.Tasks.Commands;

public record CreateTaskCommand(CreateTaskDto Dto) : IRequest<TaskDto?>;
