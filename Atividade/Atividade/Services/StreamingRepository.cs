using Atividade.Data;
using Atividade.Models;
using Atividade.Requests;
using Microsoft.EntityFrameworkCore;

namespace Atividade.Services;

/// <summary>
/// Repositorio com persistencia via Entity Framework Core.
/// </summary>
public class StreamingRepository(StreamingDbContext context)
{
    private const int LimitePerfisPorAssinante = 5;

    /// <summary>
    /// Retorna todos os titulos do catalogo.
    /// </summary>
    public async Task<IReadOnlyList<ConteudoStreaming>> ListarCatalogoAsync(
        string? categoria = null,
        string? genero = null,
        bool? emAlta = null)
    {
        IQueryable<ConteudoStreaming> query = context.Conteudos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            query = query.Where(conteudo =>
                conteudo.Categoria.ToLower() == categoria.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(genero))
        {
            query = query.Where(conteudo =>
                conteudo.Genero.ToLower() == genero.ToLower());
        }

        if (emAlta.HasValue)
        {
            query = query.Where(conteudo => conteudo.EmAlta == emAlta.Value);
        }

        return await query.OrderBy(conteudo => conteudo.Titulo).ToListAsync();
    }

    /// <summary>
    /// Retorna apenas os titulos marcados como em alta.
    /// </summary>
    public async Task<IReadOnlyList<ConteudoStreaming>> ListarEmAltaAsync() =>
        await context.Conteudos.AsNoTracking()
            .Where(conteudo => conteudo.EmAlta)
            .ToListAsync();

    /// <summary>
    /// Busca um conteudo pelo identificador.
    /// </summary>
    public async Task<ConteudoStreaming?> BuscarConteudoPorIdAsync(int id) =>
        await context.Conteudos.AsNoTracking()
            .FirstOrDefaultAsync(conteudo => conteudo.Id == id);

    /// <summary>
    /// Adiciona um novo conteudo ao catalogo.
    /// </summary>
    public async Task<ConteudoStreaming> AdicionarConteudoAsync(CriarConteudoRequest request)
    {
        var conteudo = new ConteudoStreaming
        {
            Titulo = request.Titulo,
            Categoria = request.Categoria,
            Genero = request.Genero,
            AnoLancamento = request.AnoLancamento,
            DuracaoMinutos = request.DuracaoMinutos,
            ClassificacaoIndicativa = request.ClassificacaoIndicativa,
            EmAlta = request.EmAlta
        };

        context.Conteudos.Add(conteudo);
        await context.SaveChangesAsync();
        return conteudo;
    }

    /// <summary>
    /// Atualiza os dados de um conteudo existente.
    /// </summary>
    public async Task<ConteudoStreaming?> AtualizarConteudoAsync(int id, AtualizarConteudoRequest request)
    {
        var conteudo = await context.Conteudos.FirstOrDefaultAsync(item => item.Id == id);

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

        await context.SaveChangesAsync();
        return conteudo;
    }

    /// <summary>
    /// Remove um conteudo do catalogo.
    /// </summary>
    public async Task<bool> RemoverConteudoAsync(int id)
    {
        var conteudo = await context.Conteudos.FirstOrDefaultAsync(item => item.Id == id);

        if (conteudo is null)
        {
            return false;
        }

        context.Conteudos.Remove(conteudo);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retorna os planos de assinatura disponiveis.
    /// </summary>
    public async Task<IReadOnlyList<PlanoStreaming>> ListarPlanosAsync() =>
        await context.Planos.AsNoTracking()
            .OrderBy(plano => plano.PrecoMensal)
            .ToListAsync();

    /// <summary>
    /// Busca um plano pelo identificador.
    /// </summary>
    public async Task<PlanoStreaming?> BuscarPlanoPorIdAsync(int id) =>
        await context.Planos.AsNoTracking()
            .FirstOrDefaultAsync(plano => plano.Id == id);

    /// <summary>
    /// Verifica se ja existe um plano com o mesmo nome.
    /// </summary>
    public async Task<bool> ExisteNomePlanoAsync(string nome, int? ignorarId = null) =>
        await context.Planos.AnyAsync(plano =>
            plano.Nome.ToLower() == nome.ToLower() &&
            (!ignorarId.HasValue || plano.Id != ignorarId.Value));

    /// <summary>
    /// Adiciona um novo plano de assinatura.
    /// </summary>
    public async Task<PlanoStreaming> AdicionarPlanoAsync(CriarPlanoRequest request)
    {
        var plano = new PlanoStreaming
        {
            Nome = request.Nome,
            PrecoMensal = request.PrecoMensal,
            QualidadeVideo = request.QualidadeVideo,
            QuantidadeTelas = request.QuantidadeTelas
        };

        context.Planos.Add(plano);
        await context.SaveChangesAsync();
        return plano;
    }

    /// <summary>
    /// Atualiza um plano existente.
    /// </summary>
    public async Task<PlanoStreaming?> AtualizarPlanoAsync(int id, AtualizarPlanoRequest request)
    {
        var plano = await context.Planos.FirstOrDefaultAsync(item => item.Id == id);

        if (plano is null)
        {
            return null;
        }

        plano.Nome = request.Nome;
        plano.PrecoMensal = request.PrecoMensal;
        plano.QualidadeVideo = request.QualidadeVideo;
        plano.QuantidadeTelas = request.QuantidadeTelas;

        await context.SaveChangesAsync();
        return plano;
    }

    /// <summary>
    /// Remove um plano existente.
    /// </summary>
    public async Task<bool> RemoverPlanoAsync(int id)
    {
        var plano = await context.Planos.FirstOrDefaultAsync(item => item.Id == id);

        if (plano is null)
        {
            return false;
        }

        context.Planos.Remove(plano);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Verifica se um plano possui assinantes vinculados.
    /// </summary>
    public async Task<bool> PlanoEstaEmUsoAsync(int planoId) =>
        await context.Assinantes.AnyAsync(assinante => assinante.PlanoId == planoId);

    /// <summary>
    /// Retorna os assinantes cadastrados.
    /// </summary>
    public async Task<IReadOnlyList<AssinanteStreaming>> ListarAssinantesAsync(
        bool? ativo = null,
        int? planoId = null)
    {
        IQueryable<AssinanteStreaming> query = context.Assinantes
            .AsNoTracking()
            .Include(assinante => assinante.Perfis);

        if (ativo.HasValue)
        {
            query = query.Where(assinante => assinante.Ativo == ativo.Value);
        }

        if (planoId.HasValue)
        {
            query = query.Where(assinante => assinante.PlanoId == planoId.Value);
        }

        return await query.OrderBy(assinante => assinante.Nome).ToListAsync();
    }

    /// <summary>
    /// Busca um assinante pelo identificador.
    /// </summary>
    public async Task<AssinanteStreaming?> BuscarAssinantePorIdAsync(int id) =>
        await context.Assinantes
            .AsNoTracking()
            .Include(assinante => assinante.Perfis)
            .FirstOrDefaultAsync(assinante => assinante.Id == id);

    /// <summary>
    /// Verifica se ja existe um assinante com o mesmo email.
    /// </summary>
    public async Task<bool> ExisteEmailAssinanteAsync(string email, int? ignorarId = null) =>
        await context.Assinantes.AnyAsync(assinante =>
            assinante.Email.ToLower() == email.ToLower() &&
            (!ignorarId.HasValue || assinante.Id != ignorarId.Value));

    /// <summary>
    /// Adiciona um novo assinante.
    /// </summary>
    public async Task<AssinanteStreaming> AdicionarAssinanteAsync(CriarAssinanteRequest request)
    {
        var assinante = new AssinanteStreaming
        {
            Nome = request.Nome,
            Email = request.Email,
            PlanoId = request.PlanoId,
            Ativo = request.Ativo,
            DataAssinatura = DateTime.Now
        };

        context.Assinantes.Add(assinante);
        await context.SaveChangesAsync();
        return assinante;
    }

    /// <summary>
    /// Atualiza os dados de um assinante.
    /// </summary>
    public async Task<AssinanteStreaming?> AtualizarAssinanteAsync(int id, AtualizarAssinanteRequest request)
    {
        var assinante = await context.Assinantes.FirstOrDefaultAsync(item => item.Id == id);

        if (assinante is null)
        {
            return null;
        }

        assinante.Nome = request.Nome;
        assinante.Email = request.Email;
        assinante.PlanoId = request.PlanoId;
        assinante.Ativo = request.Ativo;

        await context.SaveChangesAsync();
        return assinante;
    }

    /// <summary>
    /// Remove um assinante do sistema.
    /// </summary>
    public async Task<bool> RemoverAssinanteAsync(int id)
    {
        var assinante = await context.Assinantes.FirstOrDefaultAsync(item => item.Id == id);

        if (assinante is null)
        {
            return false;
        }

        context.Assinantes.Remove(assinante);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retorna os perfis de um assinante.
    /// </summary>
    public async Task<IReadOnlyList<PerfilStreaming>> ListarPerfisAsync(int assinanteId) =>
        await context.Perfis.AsNoTracking()
            .Where(perfil => perfil.AssinanteId == assinanteId)
            .OrderBy(perfil => perfil.Nome)
            .ToListAsync();

    /// <summary>
    /// Verifica se o assinante ainda pode criar mais perfis.
    /// </summary>
    public async Task<bool> PodeAdicionarMaisPerfisAsync(int assinanteId)
    {
        var assinanteExiste = await context.Assinantes.AnyAsync(assinante => assinante.Id == assinanteId);
        if (!assinanteExiste)
        {
            return false;
        }

        var totalPerfis = await context.Perfis.CountAsync(perfil => perfil.AssinanteId == assinanteId);
        return totalPerfis < LimitePerfisPorAssinante;
    }

    /// <summary>
    /// Verifica se ja existe um perfil com o mesmo nome para o assinante.
    /// </summary>
    public async Task<bool> ExisteNomePerfilAsync(int assinanteId, string nome) =>
        await context.Perfis.AnyAsync(perfil =>
            perfil.AssinanteId == assinanteId &&
            perfil.Nome.ToLower() == nome.ToLower());

    /// <summary>
    /// Adiciona um novo perfil para um assinante.
    /// </summary>
    public async Task<PerfilStreaming?> AdicionarPerfilAsync(int assinanteId, CriarPerfilRequest request)
    {
        var assinanteExiste = await context.Assinantes.AnyAsync(assinante => assinante.Id == assinanteId);
        if (!assinanteExiste)
        {
            return null;
        }

        var perfil = new PerfilStreaming
        {
            Nome = request.Nome,
            Idioma = request.Idioma,
            Infantil = request.Infantil,
            AssinanteId = assinanteId
        };

        context.Perfis.Add(perfil);
        await context.SaveChangesAsync();
        return perfil;
    }

    /// <summary>
    /// Remove um perfil de um assinante.
    /// </summary>
    public async Task<bool> RemoverPerfilAsync(int assinanteId, int perfilId)
    {
        var perfil = await context.Perfis
            .FirstOrDefaultAsync(item => item.Id == perfilId && item.AssinanteId == assinanteId);

        if (perfil is null)
        {
            return false;
        }

        context.Perfis.Remove(perfil);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retorna um resumo geral da plataforma.
    /// </summary>
    public async Task<ResumoStreaming> ObterResumoAsync()
    {
        var totalConteudos = await context.Conteudos.CountAsync();
        var totalConteudosEmAlta = await context.Conteudos.CountAsync(conteudo => conteudo.EmAlta);
        var totalPlanos = await context.Planos.CountAsync();
        var totalAssinantes = await context.Assinantes.CountAsync();
        var totalAssinantesAtivos = await context.Assinantes.CountAsync(assinante => assinante.Ativo);
        var totalPerfis = await context.Perfis.CountAsync();

        var faturamentoMensalEstimado = await context.Assinantes
            .Where(assinante => assinante.Ativo)
            .Join(context.Planos,
                assinante => assinante.PlanoId,
                plano => plano.Id,
                (_, plano) => plano.PrecoMensal)
            .DefaultIfEmpty(0m)
            .SumAsync();

        return new ResumoStreaming
        {
            DataGeracao = DateTime.Now,
            TotalConteudos = totalConteudos,
            TotalConteudosEmAlta = totalConteudosEmAlta,
            TotalPlanos = totalPlanos,
            TotalAssinantes = totalAssinantes,
            TotalAssinantesAtivos = totalAssinantesAtivos,
            TotalPerfis = totalPerfis,
            FaturamentoMensalEstimado = faturamentoMensalEstimado
        };
    }
}
