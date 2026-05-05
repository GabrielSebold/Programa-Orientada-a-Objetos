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
    public IActionResult Get([FromQuery] bool? ativo, [FromQuery] int? planoId)
    {
        var assinantes = repository.ListarAssinantes(ativo, planoId);
        return Ok(assinantes);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var assinante = repository.BuscarAssinantePorId(id);

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
    public IActionResult Post([FromBody] CriarAssinanteRequest request)
    {
        if (repository.BuscarPlanoPorId(request.PlanoId) is null)
        {
            return BadRequest(new { mensagem = "Plano informado nao existe." });
        }

        if (repository.ExisteEmailAssinante(request.Email))
        {
            return Conflict(new { mensagem = "Ja existe um assinante com esse email." });
        }

        var assinante = repository.AdicionarAssinante(request);
        return CreatedAtAction(nameof(GetById), new { id = assinante.Id }, assinante);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Put(int id, [FromBody] AtualizarAssinanteRequest request)
    {
        if (repository.BuscarAssinantePorId(id) is null)
        {
            return NotFound(new { mensagem = "Assinante nao encontrado." });
        }

        if (repository.BuscarPlanoPorId(request.PlanoId) is null)
        {
            return BadRequest(new { mensagem = "Plano informado nao existe." });
        }

        if (repository.ExisteEmailAssinante(request.Email, id))
        {
            return Conflict(new { mensagem = "Ja existe um assinante com esse email." });
        }

        var assinanteAtualizado = repository.AtualizarAssinante(id, request);
        return Ok(assinanteAtualizado);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var removido = repository.RemoverAssinante(id);

        if (!removido)
        {
            return NotFound(new { mensagem = "Assinante nao encontrado." });
        }

        return NoContent();
    }

    [HttpGet("{id:int}/perfis")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPerfis(int id)
    {
        if (repository.BuscarAssinantePorId(id) is null)
        {
            return NotFound(new { mensagem = "Assinante nao encontrado." });
        }

        return Ok(repository.ListarPerfis(id));
    }

    [HttpPost("{id:int}/perfis")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult PostPerfil(int id, [FromBody] CriarPerfilRequest request)
    {
        if (repository.BuscarAssinantePorId(id) is null)
        {
            return NotFound(new { mensagem = "Assinante nao encontrado." });
        }

        if (!repository.PodeAdicionarMaisPerfis(id))
        {
            return Conflict(new { mensagem = "O assinante atingiu o limite maximo de perfis." });
        }

        if (repository.ExisteNomePerfil(id, request.Nome))
        {
            return Conflict(new { mensagem = "Ja existe um perfil com esse nome para o assinante." });
        }

        var perfil = repository.AdicionarPerfil(id, request);
        return CreatedAtAction(nameof(GetPerfis), new { id }, perfil);
    }

    [HttpDelete("{id:int}/perfis/{perfilId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeletePerfil(int id, int perfilId)
    {
        if (repository.BuscarAssinantePorId(id) is null)
        {
            return NotFound(new { mensagem = "Assinante nao encontrado." });
        }

        var removido = repository.RemoverPerfil(id, perfilId);

        if (!removido)
        {
            return NotFound(new { mensagem = "Perfil nao encontrado." });
        }

        return NoContent();
    }
}
