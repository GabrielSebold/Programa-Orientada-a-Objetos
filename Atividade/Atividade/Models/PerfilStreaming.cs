using System.Text.Json.Serialization;

namespace Atividade.Models;

public class PerfilStreaming
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Idioma { get; set; } = string.Empty;

    public bool Infantil { get; set; }

    public int AssinanteId { get; set; }

    [JsonIgnore]
    public AssinanteStreaming? Assinante { get; set; }
}
