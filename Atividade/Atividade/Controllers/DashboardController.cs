using Atividade.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atividade.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(StreamingRepository repository) : ControllerBase
{
    [HttpGet("resumo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetResumo()
    {
        var resumo = repository.ObterResumo();
        return Ok(resumo);
    }
}
