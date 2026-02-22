using MediatR;
using TaskDigitalhub.Application.Common.Models;
using TaskDigitalhub.Application.Tasks.DTOs;

namespace TaskDigitalhub.Application.Tasks.Queries;

public record GetTasksByProjectQuery(
    int ProjectId,
    int PageNumber = 1,
    int PageSize = 10,
    string? SortBy = null,
    bool SortDesc = false
) : IRequest<PagedResult<TaskDto>>;
