import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import catalogoServico from "../../servicos/catalogoServico";
import Carregando from "../../componentes/Carregando";

const CATEGORIAS = ["", "Filme", "Série", "Documentário", "Animação"];
const GENEROS = ["", "Ação", "Comédia", "Drama", "Terror", "Ficção Científica", "Romance", "Suspense", "Aventura"];

function ListarCatalogo() {
  const [conteudos, definirConteudos] = useState([]);
  const [carregando, definirCarregando] = useState(true);
  const [erro, definirErro] = useState(null);
  const [filtroCategoria, definirFiltroCategoria] = useState("");
  const [filtroGenero, definirFiltroGenero] = useState("");
  const [filtroEmAlta, definirFiltroEmAlta] = useState("");
  const [confirmandoExclusao, definirConfirmandoExclusao] = useState(null);

  useEffect(() => {
    carregarConteudos();
  }, [filtroCategoria, filtroGenero, filtroEmAlta]);

  function carregarConteudos() {
    definirCarregando(true);
    definirErro(null);
    const params = {};
    if (filtroCategoria) params.categoria = filtroCategoria;
    if (filtroGenero) params.genero = filtroGenero;
    if (filtroEmAlta !== "") params.emAlta = filtroEmAlta === "true";

    catalogoServico
      .listar(params)
      .then((resposta) => definirConteudos(resposta.data))
      .catch(() => definirErro("Não foi possível carregar o catálogo."))
      .finally(() => definirCarregando(false));
  }

  function excluirConteudo(id) {
    catalogoServico
      .excluir(id)
      .then(() => {
        definirConfirmandoExclusao(null);
        carregarConteudos();
      })
      .catch((err) => {
        const mensagem = err.response?.data?.mensagem || "Erro ao excluir conteúdo.";
        alert(mensagem);
        definirConfirmandoExclusao(null);
      });
  }

  function classificacaoTexto(val) {
    if (val === 0) return "Livre";
    return `${val}+`;
  }

  return (
    <div className="pagina">
      <div className="pagina-cabecalho">
        <div>
          <h1 className="pagina-titulo">🎬 Catálogo</h1>
          <p className="pagina-subtitulo">Gerencie filmes, séries e documentários</p>
        </div>
        <Link to="/catalogo/criar" id="btn-novo-conteudo" className="btn btn-primario">
          + Novo Conteúdo
        </Link>
      </div>

      <div className="filtros">
        <select
          id="filtro-categoria"
          className="campo-entrada campo-pequeno"
          value={filtroCategoria}
          onChange={(e) => definirFiltroCategoria(e.target.value)}
        >
          <option value="">Todas as categorias</option>
          {CATEGORIAS.filter(Boolean).map((c) => <option key={c} value={c}>{c}</option>)}
        </select>

        <select
          id="filtro-genero"
          className="campo-entrada campo-pequeno"
          value={filtroGenero}
          onChange={(e) => definirFiltroGenero(e.target.value)}
        >
          <option value="">Todos os gêneros</option>
          {GENEROS.filter(Boolean).map((g) => <option key={g} value={g}>{g}</option>)}
        </select>

        <select
          id="filtro-em-alta"
          className="campo-entrada campo-pequeno"
          value={filtroEmAlta}
          onChange={(e) => definirFiltroEmAlta(e.target.value)}
        >
          <option value="">Todos</option>
          <option value="true">🔥 Em Alta</option>
          <option value="false">Demais</option>
        </select>
      </div>

      {carregando && <Carregando mensagem="Carregando catálogo..." />}
      {erro && <div className="alerta alerta-erro">{erro}</div>}

      {!carregando && !erro && conteudos.length === 0 && (
        <div className="estado-vazio">
          <span>🎬</span>
          <p>Nenhum conteúdo encontrado.</p>
          <Link to="/catalogo/criar" className="btn btn-primario">Adicionar conteúdo</Link>
        </div>
      )}

      {!carregando && !erro && conteudos.length > 0 && (
        <div className="tabela-container">
          <table className="tabela" id="tabela-catalogo">
            <thead>
              <tr>
                <th>Título</th>
                <th>Categoria</th>
                <th>Gênero</th>
                <th>Ano</th>
                <th>Duração</th>
                <th>Classificação</th>
                <th>Em Alta</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {conteudos.map((conteudo) => (
                <tr key={conteudo.id}>
                  <td className="td-titulo">{conteudo.titulo}</td>
                  <td><span className="badge badge-categoria">{conteudo.categoria}</span></td>
                  <td>{conteudo.genero}</td>
                  <td>{conteudo.anoLancamento}</td>
                  <td>{conteudo.duracaoMinutos} min</td>
                  <td>{classificacaoTexto(conteudo.classificacaoIndicativa)}</td>
                  <td>{conteudo.emAlta ? "🔥" : "—"}</td>
                  <td>
                    <div className="acoes">
                      <Link to={`/catalogo/editar/${conteudo.id}`} className="btn btn-secundario btn-pequeno">
                        ✏️
                      </Link>
                      {confirmandoExclusao === conteudo.id ? (
                        <>
                          <button
                            id={`btn-confirmar-excluir-conteudo-${conteudo.id}`}
                            className="btn btn-perigo btn-pequeno"
                            onClick={() => excluirConteudo(conteudo.id)}
                          >
                            ✓
                          </button>
                          <button
                            className="btn btn-neutro btn-pequeno"
                            onClick={() => definirConfirmandoExclusao(null)}
                          >
                            ✗
                          </button>
                        </>
                      ) : (
                        <button
                          id={`btn-excluir-conteudo-${conteudo.id}`}
                          className="btn btn-perigo btn-pequeno"
                          onClick={() => definirConfirmandoExclusao(conteudo.id)}
                        >
                          🗑️
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}

export default ListarCatalogo;
