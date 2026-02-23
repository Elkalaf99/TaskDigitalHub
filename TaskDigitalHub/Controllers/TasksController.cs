using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskDigitalhub.Application.Tasks.Commands;
using TaskDigitalhub.Application.Tasks.DTOs;
using TaskDigitalhub.Application.Tasks.Queries;
using TaskStatusEnum = TaskDigitalhub.Domain.Enums.TaskStatus;

namespace TaskDigitalHub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "Authenticated")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetTaskByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("project/{projectId:int}")]
    public async Task<IActionResult> GetByProject(
        int projectId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = false)
    {
        var result = await _mediator.Send(
            new GetTasksByProjectQuery(projectId, pageNumber, pageSize, sortBy, sortDesc));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        var result = await _mediator.Send(new CreateTaskCommand(dto));
        return result is null ? BadRequest("Invalid project or assigned user") : CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var result = await _mediator.Send(new UpdateTaskCommand(id, dto));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteTaskCommand(id));
        return deleted ? NoContent() : NotFound();
    }

    [HttpPatch("bulk-status")]
    public async Task<IActionResult> BulkStatusUpdate([FromBody] BulkStatusUpdateRequest request)
    {
        var updated = await _mediator.Send(
            new BulkStatusUpdateCommand(request.TaskIds, request.NewStatus));
        return Ok(new { UpdatedCount = updated });
    }
}

public record BulkStatusUpdateRequest(IReadOnlyList<int> TaskIds, TaskStatusEnum NewStatus);
