namespace Atividade.Models;

public class ResumoStreaming
{
    public DateTime DataGeracao { get; set; }

    public int TotalConteudos { get; set; }

    public int TotalConteudosEmAlta { get; set; }

    public int TotalPlanos { get; set; }

    public int TotalAssinantes { get; set; }

    public int TotalAssinantesAtivos { get; set; }

    public int TotalPerfis { get; set; }

    public decimal FaturamentoMensalEstimado { get; set; }
}
