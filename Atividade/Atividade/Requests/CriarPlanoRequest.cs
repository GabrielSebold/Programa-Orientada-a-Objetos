using System.ComponentModel.DataAnnotations;

namespace Atividade.Requests;

public class CriarPlanoRequest
{
    [Required]
    [StringLength(40, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Range(typeof(decimal), "1", "1000")]
    public decimal PrecoMensal { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 2)]
    public string QualidadeVideo { get; set; } = string.Empty;

    [Range(1, 10)]
    public int QuantidadeTelas { get; set; }
}
