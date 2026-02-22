using MediatR;
using TaskDigitalhub.Application.Tasks.DTOs;

namespace TaskDigitalhub.Application.Tasks.Commands;

public record UpdateTaskCommand(int Id, UpdateTaskDto Dto) : IRequest<TaskDto?>;
