using Atividade.Requests;
using Atividade.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atividade.Controllers;

/// <summary>
/// Disponibiliza consultas do catalogo do streaming.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CatalogoController(StreamingRepository repository) : ControllerBase
{
    /// <summary>
    /// Lista todo o catalogo disponivel.
    /// </summary>
    /// <param name="categoria">Filtro opcional por categoria.</param>
    /// <param name="genero">Filtro opcional por genero.</param>
    /// <param name="emAlta">Filtro opcional para conteudos em destaque.</param>
    /// <returns>Uma lista com filmes, series e documentarios.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromQuery] string? categoria,
        [FromQuery] string? genero,
        [FromQuery] bool? emAlta)
    {
        var catalogo = await repository.ListarCatalogoAsync(categoria, genero, emAlta);
        return Ok(catalogo);
    }

    /// <summary>
    /// Lista apenas os conteudos marcados como em alta.
    /// </summary>
    /// <returns>Uma lista com os destaques do catalogo.</returns>
    [HttpGet("em-alta")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmAlta()
    {
        var conteudosEmAlta = await repository.ListarEmAltaAsync();
        return Ok(conteudosEmAlta);
    }

    /// <summary>
    /// Busca um conteudo especifico pelo identificador.
    /// </summary>
    /// <param name="id">Codigo do conteudo.</param>
    /// <returns>O conteudo encontrado ou um retorno 404.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var conteudo = await repository.BuscarConteudoPorIdAsync(id);

        if (conteudo is null)
        {
            return NotFound(new { mensagem = "Conteudo nao encontrado." });
        }

        return Ok(conteudo);
    }

    /// <summary>
    /// Cadastra um novo conteudo no catalogo.
    /// </summary>
    /// <param name="request">Dados do conteudo a ser cadastrado.</param>
    /// <returns>O conteudo criado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CriarConteudoRequest request)
    {
        var conteudo = await repository.AdicionarConteudoAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = conteudo.Id }, conteudo);
    }

    /// <summary>
    /// Atualiza os dados de um conteudo do catalogo.
    /// </summary>
    /// <param name="id">Codigo do conteudo.</param>
    /// <param name="request">Novos dados do conteudo.</param>
    /// <returns>O conteudo atualizado ou um retorno 404.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] AtualizarConteudoRequest request)
    {
        var conteudoAtualizado = await repository.AtualizarConteudoAsync(id, request);

        if (conteudoAtualizado is null)
        {
            return NotFound(new { mensagem = "Conteudo nao encontrado." });
        }

        return Ok(conteudoAtualizado);
    }

    /// <summary>
    /// Remove um conteudo do catalogo.
    /// </summary>
    /// <param name="id">Codigo do conteudo.</param>
    /// <returns>Sem conteudo no retorno quando a exclusao ocorrer.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var removido = await repository.RemoverConteudoAsync(id);

        if (!removido)
        {
            return NotFound(new { mensagem = "Conteudo nao encontrado." });
        }

        return NoContent();
    }
}
