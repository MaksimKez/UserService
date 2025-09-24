using Application.Abstractions;
using Application.Dtos;
using Application.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;
using Application.Abstractions.UserFilterNotificationService;
using Application.Services.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilterController(
    IUserFilterService userFilterService,
    IUserFilterNotificationService userFilterNotificationService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await userFilterService.GetByIdAsync(id, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    [HttpGet("by-profile/{profileId:guid}")]
    public async Task<IActionResult> GetByProfileId(Guid profileId, CancellationToken ct)
    {
        var result = await userFilterService.GetByProfileIdAsync(profileId, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] ListingDto dto, CancellationToken ct)
    {
        var result = await userFilterService.ListAsync(dto, ct);
        return Ok(result.Value);
    }

    [HttpPost("default/{userId:guid}")]
    public async Task<IActionResult> AddDefault(Guid userId, CancellationToken ct)
    {
        var id = await userFilterService.AddDefaultAsync(userId, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateFilterRequest request, CancellationToken ct)
    {
        var result = await userFilterService.UpdateAsync(request, ct);
        return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await userFilterService.DeleteAsync(id, ct);
        return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
    }
    
    [HttpPost("notify-single")]
    public async Task<IActionResult> NotifySingle([FromBody] ListingDto dto, CancellationToken ct)
    {
        var result = await userFilterNotificationService.NotifyUsersAsync(dto, ct);
        return result.IsSuccess ? Ok(result.Value) : StatusCode(207, result.Value);
    }

    [HttpPost("notify-multiple")]
    public async Task<IActionResult> NotifyMultiple([FromBody] List<ListingDto> dtos, CancellationToken ct)
    {
        var result = await userFilterNotificationService.NotifyUsersAsync(dtos, ct);
        return result.IsSuccess ? Ok(result.Value) : StatusCode(207, result.Value);
    }
}
