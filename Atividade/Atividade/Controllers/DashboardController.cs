using Atividade.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atividade.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(StreamingRepository repository) : ControllerBase
{
    [HttpGet("resumo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResumo()
    {
        var resumo = await repository.ObterResumoAsync();
        return Ok(resumo);
    }
}
