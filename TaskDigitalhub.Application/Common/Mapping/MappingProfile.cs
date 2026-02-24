using AutoMapper;
using TaskDigitalhub.Application.Projects.DTOs;
using TaskDigitalhub.Application.Tasks.DTOs;
using TaskDigitalhub.Domain.Entities;

namespace TaskDigitalhub.Application.Common.Mapping;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(d => d.TaskCount, opt => opt.MapFrom(s => s.Tasks.Count))
            .ForMember(d => d.ProjectManagerName, opt => opt.MapFrom(s => s.ProjectManager != null ? s.ProjectManager.UserName : null));

        CreateMap<Project, CreateProjectDto>();
        CreateMap<Project, UpdateProjectDto>();

        CreateMap<TaskItem, TaskDto>()
            .ForMember(d => d.AssignedToUserName, opt => opt.MapFrom(s => s.AssignedToUser != null ? s.AssignedToUser.UserName : null));

        CreateMap<CreateTaskDto, TaskItem>();
        CreateMap<UpdateTaskDto, TaskItem>();
    }
}
