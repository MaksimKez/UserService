using Application.Abstractions;
using Application.Dtos;
using Application.Dtos.Requests;

namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class FilterController(IUserFilterService userFilterService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await userFilterService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    [HttpGet("by-profile")]
    public async Task<IActionResult> GetByProfileId(Guid profileId)
    {
        var result = await userFilterService.GetByProfileIdAsync(profileId);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    [HttpPost("list")]
    public async Task<IActionResult> List([FromBody] ListingDto dto)
    {
        var result = await userFilterService.ListAsync(dto);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> AddDefault(Guid userId)
    {
        var id = await userFilterService.AddDefaultAsync(userId);
        return CreatedAtAction(nameof(GetById), new { id });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateFilterRequest request)
    {
        var result = await userFilterService.Update(request);
        return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await userFilterService.DeleteAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
    }

    [HttpPost("notify-single")]
    public async Task<IActionResult> NotifySingle([FromBody] ListingDto dto, CancellationToken ct)
    {
        var result = await userFilterService.NotifyUsersAsync(dto, ct);
        return result.IsSuccess ? Ok(result.Value) : StatusCode(207, result.Value);
    }

    [HttpPost("notify-multiple")]
    public async Task<IActionResult> NotifyMultiple([FromBody] List<ListingDto> dtos, CancellationToken ct)
    {
        var result = await userFilterService.NotifyUsersAsync(dtos, ct);
        return result.IsSuccess ? Ok(result.Value) : StatusCode(207, result.Value);
    }
}
