using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskDigitalhub.Application.Projects.Commands;
using TaskDigitalhub.Application.Projects.DTOs;
using TaskDigitalhub.Application.Projects.Queries;

namespace TaskDigitalHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10,[FromQuery] string? sortBy = null,[FromQuery] bool sortDesc = false,[FromQuery] string? searchTerm = null,[FromQuery] int? status = null)
    {
        var result = await _mediator.Send(new GetProjectsPagedQuery(pageNumber, pageSize, sortBy, sortDesc, searchTerm, status));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
    {
        var result = await _mediator.Send(new CreateProjectCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
    {
        var result = await _mediator.Send(new UpdateProjectCommand(id, dto));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteProjectCommand(id));
        return deleted ? NoContent() : NotFound();
    }
}
