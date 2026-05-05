namespace Atividade.Models;

public class AssinanteStreaming
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int PlanoId { get; set; }

    public bool Ativo { get; set; }

    public DateTime DataAssinatura { get; set; }

    public List<PerfilStreaming> Perfis { get; set; } = [];
}
