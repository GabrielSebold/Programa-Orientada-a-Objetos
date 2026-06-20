import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import planosServico from "../servicos/planosServico";
import catalogoServico from "../servicos/catalogoServico";
import assinantesServico from "../servicos/assinantesServico";

function Inicio() {
  const [totais, definirTotais] = useState({ planos: 0, conteudos: 0, assinantes: 0, emAlta: 0 });
  const [emAlta, definirEmAlta] = useState([]);

  useEffect(() => {
    Promise.all([
      planosServico.listar(),
      catalogoServico.listar(),
      assinantesServico.listar(),
      catalogoServico.listarEmAlta(),
    ]).then(([resPlanos, resCatalogo, resAssinantes, resEmAlta]) => {
      definirTotais({
        planos: resPlanos.data.length,
        conteudos: resCatalogo.data.length,
        assinantes: resAssinantes.data.length,
        emAlta: resEmAlta.data.length,
      });
      definirEmAlta(resEmAlta.data.slice(0, 6));
    }).catch(() => {});
  }, []);

  const cards = [
    { icone: "💎", titulo: "Planos", valor: totais.planos, link: "/planos", cor: "roxo" },
    { icone: "🎬", titulo: "Conteúdos", valor: totais.conteudos, link: "/catalogo", cor: "azul" },
    { icone: "👤", titulo: "Assinantes", valor: totais.assinantes, link: "/assinantes", cor: "verde" },
    { icone: "🔥", titulo: "Em Alta", valor: totais.emAlta, link: "/catalogo", cor: "laranja" },
  ];

  return (
    <div className="pagina">
      <div className="hero">
        <h1 className="hero-titulo">
          <span className="hero-icone">▶</span> StreamFlix
        </h1>
        <p className="hero-subtitulo">Plataforma de gerenciamento do serviço de streaming</p>
      </div>

      <div className="grade-estatisticas">
        {cards.map((card) => (
          <Link key={card.titulo} to={card.link} className={`card-estatistica card-${card.cor}`}>
            <span className="estatistica-icone">{card.icone}</span>
            <div className="estatistica-info">
              <span className="estatistica-valor">{card.valor}</span>
              <span className="estatistica-rotulo">{card.titulo}</span>
            </div>
          </Link>
        ))}
      </div>

      {emAlta.length > 0 && (
        <div className="secao">
          <div className="secao-cabecalho">
            <h2 className="secao-titulo">🔥 Conteúdos em Alta</h2>
            <Link to="/catalogo" className="btn btn-neutro btn-pequeno">Ver todos →</Link>
          </div>
          <div className="grade-miniaturas">
            {emAlta.map((conteudo) => (
              <div key={conteudo.id} className="miniatura">
                <div className="miniatura-capa">
                  <span className="miniatura-categoria">{conteudo.categoria}</span>
                  <span className="miniatura-icone">
                    {conteudo.categoria === "Série" ? "📺" : conteudo.categoria === "Documentário" ? "📹" : "🎬"}
                  </span>
                </div>
                <div className="miniatura-info">
                  <p className="miniatura-titulo">{conteudo.titulo}</p>
                  <p className="miniatura-detalhe">{conteudo.genero} · {conteudo.anoLancamento}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      <div className="atalhos">
        <h2 className="secao-titulo">⚡ Atalhos</h2>
        <div className="grade-atalhos">
          <Link to="/planos/criar" id="atalho-novo-plano" className="atalho">
            <span>💎</span> Novo Plano
          </Link>
          <Link to="/catalogo/criar" id="atalho-novo-conteudo" className="atalho">
            <span>🎬</span> Novo Conteúdo
          </Link>
          <Link to="/assinantes/criar" id="atalho-novo-assinante" className="atalho">
            <span>👤</span> Novo Assinante
          </Link>
          <Link to="/catalogo" id="atalho-catalogo" className="atalho">
            <span>📋</span> Ver Catálogo
          </Link>
        </div>
      </div>
    </div>
  );
}

export default Inicio;
