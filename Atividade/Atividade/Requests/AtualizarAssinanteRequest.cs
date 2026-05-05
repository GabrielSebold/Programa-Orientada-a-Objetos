using System.ComponentModel.DataAnnotations;

namespace Atividade.Requests;

public class AtualizarAssinanteRequest
{
    [Required]
    [StringLength(80, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int PlanoId { get; set; }

    public bool Ativo { get; set; }
}
