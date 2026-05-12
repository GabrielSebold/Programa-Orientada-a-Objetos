using Atividade.Requests;
using Atividade.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atividade.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssinantesController(StreamingRepository repository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] bool? ativo, [FromQuery] int? planoId)
    {
        var assinantes = await repository.ListarAssinantesAsync(ativo, planoId);
        return Ok(assinantes);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var assinante = await repository.BuscarAssinantePorIdAsync(id);

        if (assinante is null)
        {
            return NotFound(new { mensagem = "Assinante nao encontrado." });
        }

        return Ok(assinante);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Post([FromBody] CriarAssinanteRequest request)
    {
        if (await repository.BuscarPlanoPorIdAsync(request.PlanoId) is null)
        {
            return BadRequest(new { mensagem = "Plano informado nao existe." });
        }

        if (await repository.ExisteEmailAssinanteAsync(request.Email))
        {
            return Conflict(new { mensagem = "Ja existe um assinante com esse email." });
        }

        var assinante = await repository.AdicionarAssinanteAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = assinante.Id }, assinante);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Put(int id, [FromBody] AtualizarAssinanteRequest request)
    {
        if (await repository.BuscarAssinantePorIdAsync(id) is null)
        {
            return NotFound(new { mensagem = "Assinante nao encontrado." });
        }

        if (await repository.BuscarPlanoPorIdAsync(request.PlanoId) is null)
        {
            return BadRequest(new { mensagem = "Plano informado nao existe." });
        }

        if (await repository.ExisteEmailAssinanteAsync(request.Email, id))
        {
            return Conflict(new { mensagem = "Ja existe um assinante com esse email." });
        }

        var assinanteAtualizado = await repository.AtualizarAssinanteAsync(id, request);
        return Ok(assinanteAtualizado);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var removido = await repository.RemoverAssinanteAsync(id);

        if (!removido)
        {
            return NotFound(new { mensagem = "Assinante nao encontrado." });
        }

        return NoContent();
    }

    [HttpGet("{id:int}/perfis")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPerfis(int id)
    {
        if (await repository.BuscarAssinantePorIdAsync(id) is null)
        {
            return NotFound(new { mensagem = "Assinante nao encontrado." });
        }

        return Ok(await repository.ListarPerfisAsync(id));
    }

    [HttpPost("{id:int}/perfis")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PostPerfil(int id, [FromBody] CriarPerfilRequest request)
    {
        if (await repository.BuscarAssinantePorIdAsync(id) is null)
        {
            return NotFound(new { mensagem = "Assinante nao encontrado." });
        }

        if (!await repository.PodeAdicionarMaisPerfisAsync(id))
        {
            return Conflict(new { mensagem = "O assinante atingiu o limite maximo de perfis." });
        }

        if (await repository.ExisteNomePerfilAsync(id, request.Nome))
        {
            return Conflict(new { mensagem = "Ja existe um perfil com esse nome para o assinante." });
        }

        var perfil = await repository.AdicionarPerfilAsync(id, request);
        return CreatedAtAction(nameof(GetPerfis), new { id }, perfil);
    }

    [HttpDelete("{id:int}/perfis/{perfilId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePerfil(int id, int perfilId)
    {
        if (await repository.BuscarAssinantePorIdAsync(id) is null)
        {
            return NotFound(new { mensagem = "Assinante nao encontrado." });
        }

        var removido = await repository.RemoverPerfilAsync(id, perfilId);

        if (!removido)
        {
            return NotFound(new { mensagem = "Perfil nao encontrado." });
        }

        return NoContent();
    }
}
