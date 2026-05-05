using Atividade.Models;
using Atividade.Requests;

namespace Atividade.Services;

/// <summary>
/// Repositorio em memoria com dados de exemplo do streaming.
/// </summary>
public class StreamingRepository
{
    private const int LimitePerfisPorAssinante = 5;

    private readonly List<ConteudoStreaming> _catalogo =
    [
        new()
        {
            Id = 1,
            Titulo = "Horizonte Final",
            Categoria = "Filme",
            Genero = "Ficcao Cientifica",
            AnoLancamento = 2024,
            DuracaoMinutos = 118,
            ClassificacaoIndicativa = 14,
            EmAlta = true
        },
        new()
        {
            Id = 2,
            Titulo = "Cidade em Codigo",
            Categoria = "Serie",
            Genero = "Drama",
            AnoLancamento = 2025,
            DuracaoMinutos = 50,
            ClassificacaoIndicativa = 16,
            EmAlta = true
        },
        new()
        {
            Id = 3,
            Titulo = "Noite Pixelada",
            Categoria = "Filme",
            Genero = "Suspense",
            AnoLancamento = 2023,
            DuracaoMinutos = 104,
            ClassificacaoIndicativa = 16,
            EmAlta = false
        },
        new()
        {
            Id = 4,
            Titulo = "Arena dos Campeoes",
            Categoria = "Documentario",
            Genero = "Esportes",
            AnoLancamento = 2022,
            DuracaoMinutos = 92,
            ClassificacaoIndicativa = 10,
            EmAlta = false
        }
    ];

    private readonly List<PlanoStreaming> _planos =
    [
        new()
        {
            Id = 1,
            Nome = "Basico",
            PrecoMensal = 19.90m,
            QualidadeVideo = "HD",
            QuantidadeTelas = 1
        },
        new()
        {
            Id = 2,
            Nome = "Padrao",
            PrecoMensal = 29.90m,
            QualidadeVideo = "Full HD",
            QuantidadeTelas = 2
        },
        new()
        {
            Id = 3,
            Nome = "Premium",
            PrecoMensal = 39.90m,
            QualidadeVideo = "4K",
            QuantidadeTelas = 4
        }
    ];

    private readonly List<AssinanteStreaming> _assinantes =
    [
        new()
        {
            Id = 1,
            Nome = "Ana Souza",
            Email = "ana@streamplay.com",
            PlanoId = 2,
            Ativo = true,
            DataAssinatura = new DateTime(2026, 1, 10),
            Perfis =
            [
                new()
                {
                    Id = 1,
                    Nome = "Ana",
                    Idioma = "Portugues",
                    Infantil = false
                },
                new()
                {
                    Id = 2,
                    Nome = "Kids",
                    Idioma = "Portugues",
                    Infantil = true
                }
            ]
        },
        new()
        {
            Id = 2,
            Nome = "Lucas Lima",
            Email = "lucas@streamplay.com",
            PlanoId = 1,
            Ativo = true,
            DataAssinatura = new DateTime(2026, 2, 5),
            Perfis =
            [
                new()
                {
                    Id = 1,
                    Nome = "Lucas",
                    Idioma = "Portugues",
                    Infantil = false
                }
            ]
        }
    ];

    /// <summary>
    /// Retorna todos os titulos do catalogo.
    /// </summary>
    public IReadOnlyList<ConteudoStreaming> ListarCatalogo(
        string? categoria = null,
        string? genero = null,
        bool? emAlta = null)
    {
        IEnumerable<ConteudoStreaming> query = _catalogo;

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            query = query.Where(conteudo =>
                conteudo.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(genero))
        {
            query = query.Where(conteudo =>
                conteudo.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase));
        }

        if (emAlta.HasValue)
        {
            query = query.Where(conteudo => conteudo.EmAlta == emAlta.Value);
        }

        return query.OrderBy(conteudo => conteudo.Titulo).ToList();
    }

    /// <summary>
    /// Retorna apenas os titulos marcados como em alta.
    /// </summary>
    public IReadOnlyList<ConteudoStreaming> ListarEmAlta() =>
        _catalogo.Where(conteudo => conteudo.EmAlta).ToList();

    /// <summary>
    /// Busca um conteudo pelo identificador.
    /// </summary>
    public ConteudoStreaming? BuscarConteudoPorId(int id) =>
        _catalogo.FirstOrDefault(conteudo => conteudo.Id == id);

    /// <summary>
    /// Adiciona um novo conteudo ao catalogo.
    /// </summary>
    public ConteudoStreaming AdicionarConteudo(CriarConteudoRequest request)
    {
        var conteudo = new ConteudoStreaming
        {
            Id = ObterProximoId(_catalogo.Select(conteudo => conteudo.Id)),
            Titulo = request.Titulo,
            Categoria = request.Categoria,
            Genero = request.Genero,
            AnoLancamento = request.AnoLancamento,
            DuracaoMinutos = request.DuracaoMinutos,
            ClassificacaoIndicativa = request.ClassificacaoIndicativa,
            EmAlta = request.EmAlta
        };

        _catalogo.Add(conteudo);
        return conteudo;
    }

    /// <summary>
    /// Atualiza os dados de um conteudo existente.
    /// </summary>
    public ConteudoStreaming? AtualizarConteudo(int id, AtualizarConteudoRequest request)
    {
        var conteudo = BuscarConteudoPorId(id);

        if (conteudo is null)
        {
            return null;
        }

        conteudo.Titulo = request.Titulo;
        conteudo.Categoria = request.Categoria;
        conteudo.Genero = request.Genero;
        conteudo.AnoLancamento = request.AnoLancamento;
        conteudo.DuracaoMinutos = request.DuracaoMinutos;
        conteudo.ClassificacaoIndicativa = request.ClassificacaoIndicativa;
        conteudo.EmAlta = request.EmAlta;

        return conteudo;
    }

    /// <summary>
    /// Remove um conteudo do catalogo.
    /// </summary>
    public bool RemoverConteudo(int id)
    {
        var conteudo = BuscarConteudoPorId(id);

        if (conteudo is null)
        {
            return false;
        }

        _catalogo.Remove(conteudo);
        return true;
    }

    /// <summary>
    /// Retorna os planos de assinatura disponiveis.
    /// </summary>
    public IReadOnlyList<PlanoStreaming> ListarPlanos() =>
        _planos.OrderBy(plano => plano.PrecoMensal).ToList();

    /// <summary>
    /// Busca um plano pelo identificador.
    /// </summary>
    public PlanoStreaming? BuscarPlanoPorId(int id) =>
        _planos.FirstOrDefault(plano => plano.Id == id);

    /// <summary>
    /// Verifica se ja existe um plano com o mesmo nome.
    /// </summary>
    public bool ExisteNomePlano(string nome, int? ignorarId = null) =>
        _planos.Any(plano =>
            plano.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase) &&
            (!ignorarId.HasValue || plano.Id != ignorarId.Value));

    /// <summary>
    /// Adiciona um novo plano de assinatura.
    /// </summary>
    public PlanoStreaming AdicionarPlano(CriarPlanoRequest request)
    {
        var plano = new PlanoStreaming
        {
            Id = ObterProximoId(_planos.Select(item => item.Id)),
            Nome = request.Nome,
            PrecoMensal = request.PrecoMensal,
            QualidadeVideo = request.QualidadeVideo,
            QuantidadeTelas = request.QuantidadeTelas
        };

        _planos.Add(plano);
        return plano;
    }

    /// <summary>
    /// Atualiza um plano existente.
    /// </summary>
    public PlanoStreaming? AtualizarPlano(int id, AtualizarPlanoRequest request)
    {
        var plano = BuscarPlanoPorId(id);

        if (plano is null)
        {
            return null;
        }

        plano.Nome = request.Nome;
        plano.PrecoMensal = request.PrecoMensal;
        plano.QualidadeVideo = request.QualidadeVideo;
        plano.QuantidadeTelas = request.QuantidadeTelas;

        return plano;
    }

    /// <summary>
    /// Remove um plano existente.
    /// </summary>
    public bool RemoverPlano(int id)
    {
        var plano = BuscarPlanoPorId(id);

        if (plano is null)
        {
            return false;
        }

        _planos.Remove(plano);
        return true;
    }

    /// <summary>
    /// Verifica se um plano possui assinantes vinculados.
    /// </summary>
    public bool PlanoEstaEmUso(int planoId) =>
        _assinantes.Any(assinante => assinante.PlanoId == planoId);

    /// <summary>
    /// Retorna os assinantes cadastrados.
    /// </summary>
    public IReadOnlyList<AssinanteStreaming> ListarAssinantes(bool? ativo = null, int? planoId = null)
    {
        IEnumerable<AssinanteStreaming> query = _assinantes;

        if (ativo.HasValue)
        {
            query = query.Where(assinante => assinante.Ativo == ativo.Value);
        }

        if (planoId.HasValue)
        {
            query = query.Where(assinante => assinante.PlanoId == planoId.Value);
        }

        return query.OrderBy(assinante => assinante.Nome).ToList();
    }

    /// <summary>
    /// Busca um assinante pelo identificador.
    /// </summary>
    public AssinanteStreaming? BuscarAssinantePorId(int id) =>
        _assinantes.FirstOrDefault(assinante => assinante.Id == id);

    /// <summary>
    /// Verifica se ja existe um assinante com o mesmo email.
    /// </summary>
    public bool ExisteEmailAssinante(string email, int? ignorarId = null) =>
        _assinantes.Any(assinante =>
            assinante.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            (!ignorarId.HasValue || assinante.Id != ignorarId.Value));

    /// <summary>
    /// Adiciona um novo assinante.
    /// </summary>
    public AssinanteStreaming AdicionarAssinante(CriarAssinanteRequest request)
    {
        var assinante = new AssinanteStreaming
        {
            Id = ObterProximoId(_assinantes.Select(item => item.Id)),
            Nome = request.Nome,
            Email = request.Email,
            PlanoId = request.PlanoId,
            Ativo = request.Ativo,
            DataAssinatura = DateTime.Now
        };

        _assinantes.Add(assinante);
        return assinante;
    }

    /// <summary>
    /// Atualiza os dados de um assinante.
    /// </summary>
    public AssinanteStreaming? AtualizarAssinante(int id, AtualizarAssinanteRequest request)
    {
        var assinante = BuscarAssinantePorId(id);

        if (assinante is null)
        {
            return null;
        }

        assinante.Nome = request.Nome;
        assinante.Email = request.Email;
        assinante.PlanoId = request.PlanoId;
        assinante.Ativo = request.Ativo;

        return assinante;
    }

    /// <summary>
    /// Remove um assinante do sistema.
    /// </summary>
    public bool RemoverAssinante(int id)
    {
        var assinante = BuscarAssinantePorId(id);

        if (assinante is null)
        {
            return false;
        }

        _assinantes.Remove(assinante);
        return true;
    }

    /// <summary>
    /// Retorna os perfis de um assinante.
    /// </summary>
    public IReadOnlyList<PerfilStreaming> ListarPerfis(int assinanteId) =>
        BuscarAssinantePorId(assinanteId)?.Perfis.OrderBy(perfil => perfil.Nome).ToList() ??
        [];

    /// <summary>
    /// Verifica se o assinante ainda pode criar mais perfis.
    /// </summary>
    public bool PodeAdicionarMaisPerfis(int assinanteId)
    {
        var assinante = BuscarAssinantePorId(assinanteId);
        return assinante is not null && assinante.Perfis.Count < LimitePerfisPorAssinante;
    }

    /// <summary>
    /// Verifica se ja existe um perfil com o mesmo nome para o assinante.
    /// </summary>
    public bool ExisteNomePerfil(int assinanteId, string nome) =>
        BuscarAssinantePorId(assinanteId)?.Perfis.Any(perfil =>
            perfil.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)) ?? false;

    /// <summary>
    /// Adiciona um novo perfil para um assinante.
    /// </summary>
    public PerfilStreaming? AdicionarPerfil(int assinanteId, CriarPerfilRequest request)
    {
        var assinante = BuscarAssinantePorId(assinanteId);

        if (assinante is null)
        {
            return null;
        }

        var perfil = new PerfilStreaming
        {
            Id = ObterProximoId(assinante.Perfis.Select(item => item.Id)),
            Nome = request.Nome,
            Idioma = request.Idioma,
            Infantil = request.Infantil
        };

        assinante.Perfis.Add(perfil);
        return perfil;
    }

    /// <summary>
    /// Remove um perfil de um assinante.
    /// </summary>
    public bool RemoverPerfil(int assinanteId, int perfilId)
    {
        var assinante = BuscarAssinantePorId(assinanteId);

        if (assinante is null)
        {
            return false;
        }

        var perfil = assinante.Perfis.FirstOrDefault(item => item.Id == perfilId);

        if (perfil is null)
        {
            return false;
        }

        assinante.Perfis.Remove(perfil);
        return true;
    }

    /// <summary>
    /// Retorna um resumo geral da plataforma.
    /// </summary>
    public ResumoStreaming ObterResumo()
    {
        var assinantesAtivos = _assinantes.Where(assinante => assinante.Ativo).ToList();

        return new ResumoStreaming
        {
            DataGeracao = DateTime.Now,
            TotalConteudos = _catalogo.Count,
            TotalConteudosEmAlta = _catalogo.Count(conteudo => conteudo.EmAlta),
            TotalPlanos = _planos.Count,
            TotalAssinantes = _assinantes.Count,
            TotalAssinantesAtivos = assinantesAtivos.Count,
            TotalPerfis = _assinantes.Sum(assinante => assinante.Perfis.Count),
            FaturamentoMensalEstimado = assinantesAtivos.Sum(assinante =>
                BuscarPlanoPorId(assinante.PlanoId)?.PrecoMensal ?? 0m)
        };
    }

    private static int ObterProximoId(IEnumerable<int> ids) =>
        ids.DefaultIfEmpty(0).Max() + 1;
}
