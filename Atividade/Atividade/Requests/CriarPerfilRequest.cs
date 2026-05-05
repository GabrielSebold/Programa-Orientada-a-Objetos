using System.ComponentModel.DataAnnotations;

namespace Atividade.Requests;

public class CriarPerfilRequest
{
    [Required]
    [StringLength(40, MinimumLength = 2)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(20, MinimumLength = 2)]
    public string Idioma { get; set; } = string.Empty;

    public bool Infantil { get; set; }
}
