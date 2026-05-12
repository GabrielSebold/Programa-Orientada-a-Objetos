using Atividade.Models;
using Microsoft.EntityFrameworkCore;

namespace Atividade.Data;

public static class StreamingSeeder
{
    public static async Task SeedAsync(StreamingDbContext context)
    {
        if (await context.Conteudos.AnyAsync() ||
            await context.Planos.AnyAsync() ||
            await context.Assinantes.AnyAsync())
        {
            return;
        }

        var conteudos = new List<ConteudoStreaming>
        {
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
        };

        var planos = new List<PlanoStreaming>
        {
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
        };

        var assinantes = new List<AssinanteStreaming>
        {
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
                        Infantil = false,
                        AssinanteId = 1
                    },
                    new()
                    {
                        Id = 2,
                        Nome = "Kids",
                        Idioma = "Portugues",
                        Infantil = true,
                        AssinanteId = 1
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
                        Id = 3,
                        Nome = "Lucas",
                        Idioma = "Portugues",
                        Infantil = false,
                        AssinanteId = 2
                    }
                ]
            }
        };

        await context.Conteudos.AddRangeAsync(conteudos);
        await context.Planos.AddRangeAsync(planos);
        await context.Assinantes.AddRangeAsync(assinantes);
        await context.SaveChangesAsync();
    }
}
