using Microsoft.AspNetCore.Mvc;
using TabibApp.Application;
using TabibApp.Application.Dtos;

namespace TabibApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssistantController : ControllerBase
{
    private readonly IAssistantRepository _assistantRepository;

    public AssistantController(IAssistantRepository assistantRepository)
    {
        _assistantRepository = assistantRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssistantDto>>> GetAll([FromQuery] string doctorId)
    {
        var assistants = await _assistantRepository.GetAllAsync(doctorId);
        return Ok(assistants);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssistantDto>> GetById(int id)
    {
        var assistant = await _assistantRepository.GetByIdAsync(id);
        if (assistant == null)
        {
            return NotFound();
        }

        return Ok(assistant);
    }

    [HttpPost]
    public async Task<ActionResult<AssistantDto>> Create(CreateAssistantDto createAssistantDto)
    {
        var assistantDto = await _assistantRepository.AddAsync(createAssistantDto);
        if (assistantDto is null)
            return BadRequest("Invalid Email");
        return Ok(assistantDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, CreateAssistantDto updateAssistantDto)
    {
        var assistantDto = await _assistantRepository.UpdateAsync(id, updateAssistantDto);
        if (assistantDto == null)
        {
            return NotFound();
        }

        return Ok(assistantDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _assistantRepository.DeleteAsync(id);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}