using InterviewPrep.Api.Extensions;
using InterviewPrep.Application.DTOs;
using InterviewPrep.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPrep.Api.Controllers;

[ApiController]
[Authorize(Policy = "EmployeeOrAdmin")]
[Route("api/[controller]")]
public sealed class WelcomeController : ControllerBase
{
    private readonly WelcomeService _welcomeService;

    public WelcomeController(WelcomeService welcomeService)
    {
        _welcomeService = welcomeService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(WelcomeDto), StatusCodes.Status200OK)]
    public ActionResult<WelcomeDto> Get()
    {
        var response = _welcomeService.BuildWelcome(User.GetDisplayName(), User.GetRole());
        return Ok(response);
    }
}
