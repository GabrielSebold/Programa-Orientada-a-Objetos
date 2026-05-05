namespace Atividade.Models;

/// <summary>
/// Representa um plano de assinatura da plataforma.
/// </summary>
public class PlanoStreaming
{
    /// <summary>
    /// Identificador unico do plano.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome comercial do plano.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Valor da assinatura mensal.
    /// </summary>
    public decimal PrecoMensal { get; set; }

    /// <summary>
    /// Qualidade maxima de video oferecida.
    /// </summary>
    public string QualidadeVideo { get; set; } = string.Empty;

    /// <summary>
    /// Quantidade de telas simultaneas permitidas.
    /// </summary>
    public int QuantidadeTelas { get; set; }
}
