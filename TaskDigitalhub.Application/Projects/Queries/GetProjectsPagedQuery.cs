using MediatR;
using TaskDigitalhub.Application.Common.Models;
using TaskDigitalhub.Application.Projects.DTOs;

namespace TaskDigitalhub.Application.Projects.Queries;

public record GetProjectsPagedQuery(int PageNumber = 1,int PageSize = 10,string? SortBy = null,bool SortDesc = false,string? SearchTerm = null,int? StatusFilter = null) : IRequest<PagedResult<ProjectDto>>;
