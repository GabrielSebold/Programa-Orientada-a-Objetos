using System.ComponentModel.DataAnnotations;

namespace Atividade.Requests;

public class CriarConteudoRequest
{
    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string Categoria { get; set; } = string.Empty;

    [Required]
    [StringLength(40, MinimumLength = 3)]
    public string Genero { get; set; } = string.Empty;

    [Range(1900, 2100)]
    public int AnoLancamento { get; set; }

    [Range(1, 600)]
    public int DuracaoMinutos { get; set; }

    [Range(0, 18)]
    public int ClassificacaoIndicativa { get; set; }

    public bool EmAlta { get; set; }
}
