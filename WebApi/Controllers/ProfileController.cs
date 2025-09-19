using Application.Abstractions;
using Application.Dtos.Requests;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController(IUserProfileService userProfileService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await userProfileService.GetByIdAsync(id, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    [HttpGet("by-email")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email, CancellationToken ct)
    {
        var result = await userProfileService.GetByEmailAsync(email, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddUserProfileRequest request, CancellationToken ct)
    {
        var result = await userProfileService.AddAsync(request, ct);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { result.Value }, request)
            : BadRequest(result.Errors);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequest request, CancellationToken ct)
    {
        var result = await userProfileService.UpdateAsync(request, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await userProfileService.DeleteAsync(id, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }
}
