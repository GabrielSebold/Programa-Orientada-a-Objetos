namespace Atividade.Models;

/// <summary>
/// Representa um titulo disponivel no catalogo do streaming.
/// </summary>
public class ConteudoStreaming
{
    /// <summary>
    /// Identificador unico do conteudo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome exibido para o usuario.
    /// </summary>
    public string Titulo { get; set; } = string.Empty;

    /// <summary>
    /// Tipo principal do conteudo, como filme ou serie.
    /// </summary>
    public string Categoria { get; set; } = string.Empty;

    /// <summary>
    /// Genero predominante do titulo.
    /// </summary>
    public string Genero { get; set; } = string.Empty;

    /// <summary>
    /// Ano de lancamento do conteudo.
    /// </summary>
    public int AnoLancamento { get; set; }

    /// <summary>
    /// Duracao em minutos.
    /// </summary>
    public int DuracaoMinutos { get; set; }

    /// <summary>
    /// Faixa etaria recomendada.
    /// </summary>
    public int ClassificacaoIndicativa { get; set; }

    /// <summary>
    /// Indica se o titulo esta em destaque na plataforma.
    /// </summary>
    public bool EmAlta { get; set; }
}
