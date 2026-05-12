using Atividade.Requests;
using Atividade.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atividade.Controllers;

/// <summary>
/// Disponibiliza a consulta dos planos de assinatura.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PlanosController(StreamingRepository repository) : ControllerBase
{
    /// <summary>
    /// Lista os planos disponiveis no streaming.
    /// </summary>
    /// <returns>Uma lista com os planos e seus beneficios.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var planos = await repository.ListarPlanosAsync();
        return Ok(planos);
    }

    /// <summary>
    /// Busca um plano especifico pelo identificador.
    /// </summary>
    /// <param name="id">Codigo do plano.</param>
    /// <returns>O plano encontrado ou um retorno 404.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var plano = await repository.BuscarPlanoPorIdAsync(id);

        if (plano is null)
        {
            return NotFound(new { mensagem = "Plano nao encontrado." });
        }

        return Ok(plano);
    }

    /// <summary>
    /// Cadastra um novo plano de assinatura.
    /// </summary>
    /// <param name="request">Dados do plano a ser cadastrado.</param>
    /// <returns>O plano criado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Post([FromBody] CriarPlanoRequest request)
    {
        if (await repository.ExisteNomePlanoAsync(request.Nome))
        {
            return Conflict(new { mensagem = "Ja existe um plano com esse nome." });
        }

        var plano = await repository.AdicionarPlanoAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = plano.Id }, plano);
    }

    /// <summary>
    /// Atualiza os dados de um plano de assinatura.
    /// </summary>
    /// <param name="id">Codigo do plano.</param>
    /// <param name="request">Novos dados do plano.</param>
    /// <returns>O plano atualizado ou um retorno 404.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Put(int id, [FromBody] AtualizarPlanoRequest request)
    {
        if (await repository.BuscarPlanoPorIdAsync(id) is null)
        {
            return NotFound(new { mensagem = "Plano nao encontrado." });
        }

        if (await repository.ExisteNomePlanoAsync(request.Nome, id))
        {
            return Conflict(new { mensagem = "Ja existe um plano com esse nome." });
        }

        var planoAtualizado = await repository.AtualizarPlanoAsync(id, request);
        return Ok(planoAtualizado);
    }

    /// <summary>
    /// Remove um plano de assinatura.
    /// </summary>
    /// <param name="id">Codigo do plano.</param>
    /// <returns>Sem conteudo no retorno quando a exclusao ocorrer.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        if (await repository.BuscarPlanoPorIdAsync(id) is null)
        {
            return NotFound(new { mensagem = "Plano nao encontrado." });
        }

        if (await repository.PlanoEstaEmUsoAsync(id))
        {
            return Conflict(new { mensagem = "O plano possui assinantes vinculados e nao pode ser removido." });
        }

        await repository.RemoverPlanoAsync(id);
        return NoContent();
    }
}
