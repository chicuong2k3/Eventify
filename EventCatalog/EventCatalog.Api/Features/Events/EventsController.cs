using EventCatalog.Api.Events;
using Microsoft.AspNetCore.Mvc;

namespace EventCatalog.Api.Features.Events;

[ApiController]
[Route("api/v{apiVersion}/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMediator mediator;

    public EventsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var @event = await mediator.Send(new GetEventById(id));
        return Ok(@event);
    }

    [HttpGet]
    public async Task<IActionResult> SearchEvents([FromQuery] SearchEvent query)
    {
        var events = await mediator.Send(query);
        return Ok(events);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEvent command)
    {
        var @event = await mediator.Send(command);
        return CreatedAtAction(nameof(GetEvent), new { id = @event.Id, apiVersion = "1.0" }, @event);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelEvent(Guid id)
    {
        await mediator.Send(new CancelEvent(id));
        return NoContent();
    }

    [HttpPost("{id}/publish")]
    public async Task<IActionResult> PublishEvent(Guid id)
    {
        await mediator.Send(new PublishEvent(id));
        return NoContent();
    }

    [HttpPost("{id}/reschedule")]
    public async Task<IActionResult> RescheduleEvent(Guid id, [FromBody] RescheduleEventRequest request)
    {
        var command = new RescheduleEvent(id, request.StartsAt, request.EndsAt);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id}/ticket-types")]
    public async Task<IActionResult> AddTicketType(Guid id, [FromBody] AddTickecTypeRequest request)
    {
        var command = new AddTicketTypeToEvent(id, request.Name, request.Price, request.Currency, request.Quantity);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        await mediator.Send(new DeleteEvent(id));
        return NoContent();
    }
}

public record RescheduleEventRequest(DateTime StartsAt, DateTime? EndsAt);

public record AddTickecTypeRequest(
    string Name,
    decimal Price,
    string Currency,
    int Quantity);