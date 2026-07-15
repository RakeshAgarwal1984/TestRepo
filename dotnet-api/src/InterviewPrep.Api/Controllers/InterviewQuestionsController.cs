using InterviewPrep.Api.Extensions;
using InterviewPrep.Application.DTOs;
using InterviewPrep.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPrep.Api.Controllers;

[ApiController]
[Authorize(Policy = "EmployeeOrAdmin")]
[Route("api/interview-questions")]
public sealed class InterviewQuestionsController : ControllerBase
{
    private readonly InterviewQuestionService _service;

    public InterviewQuestionsController(InterviewQuestionService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<InterviewQuestionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<InterviewQuestionDto>>> List(CancellationToken cancellationToken)
    {
        var questions = await _service.ListVisibleAsync(User.GetRole(), cancellationToken);
        return Ok(questions);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InterviewQuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InterviewQuestionDto>> Get(Guid id, CancellationToken cancellationToken)
    {
        var question = await _service.GetByIdAsync(id, User.GetRole(), cancellationToken);
        return question is null ? NotFound() : Ok(question);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(InterviewQuestionDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<InterviewQuestionDto>> Create(
        CreateInterviewQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var question = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = question.Id }, question);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(InterviewQuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InterviewQuestionDto>> Update(
        Guid id,
        UpdateInterviewQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var question = await _service.UpdateAsync(id, request, cancellationToken);
        return question is null ? NotFound() : Ok(question);
    }
}
