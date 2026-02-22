using MediatR;
using TaskDigitalhub.Application.Tasks.DTOs;

namespace TaskDigitalhub.Application.Tasks.Queries;

public record GetTaskByIdQuery(int Id) : IRequest<TaskDto?>;
