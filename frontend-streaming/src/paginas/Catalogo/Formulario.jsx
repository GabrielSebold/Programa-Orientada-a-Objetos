import { useEffect, useState } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import catalogoServico from "../../servicos/catalogoServico";
import Carregando from "../../componentes/Carregando";

function FormularioCatalogo() {
  const { id } = useParams();
  const navegar = useNavigate();
  const [titulo, definirTitulo] = useState("");
  const [categoria, definirCategoria] = useState("");
  const [genero, definirGenero] = useState("");
  const [anoLancamento, definirAnoLancamento] = useState(new Date().getFullYear());
  const [duracaoMinutos, definirDuracaoMinutos] = useState("");
  const [classificacaoIndicativa, definirClassificacaoIndicativa] = useState(0);
  const [emAlta, definirEmAlta] = useState(false);
  const [carregando, definirCarregando] = useState(false);
  const [enviando, definirEnviando] = useState(false);
  const [erros, definirErros] = useState({});

  useEffect(() => {
    if (id) {
      definirCarregando(true);
      catalogoServico
        .buscarPorId(id)
        .then((resposta) => {
          const dados = resposta.data;
          definirTitulo(dados.titulo);
          definirCategoria(dados.categoria);
          definirGenero(dados.genero);
          definirAnoLancamento(dados.anoLancamento);
          definirDuracaoMinutos(dados.duracaoMinutos);
          definirClassificacaoIndicativa(dados.classificacaoIndicativa);
          definirEmAlta(dados.emAlta);
        })
        .finally(() => definirCarregando(false));
    }
  }, [id]);

  function validar() {
    const novosErros = {};
    if (!titulo.trim() || titulo.length < 2) novosErros.titulo = "Título deve ter pelo menos 2 caracteres.";
    if (!categoria) novosErros.categoria = "Categoria é obrigatória.";
    if (!genero) novosErros.genero = "Gênero é obrigatório.";
    if (!anoLancamento || anoLancamento < 1900 || anoLancamento > 2100)
      novosErros.anoLancamento = "Ano deve ser entre 1900 e 2100.";
    if (!duracaoMinutos || duracaoMinutos < 1 || duracaoMinutos > 600)
      novosErros.duracaoMinutos = "Duração deve ser entre 1 e 600 minutos.";
    return novosErros;
  }

  function salvar(evento) {
    evento.preventDefault();
    const validacao = validar();
    if (Object.keys(validacao).length > 0) {
      definirErros(validacao);
      return;
    }
    definirErros({});
    definirEnviando(true);

    const dados = {
      titulo,
      categoria,
      genero,
      anoLancamento: parseInt(anoLancamento),
      duracaoMinutos: parseInt(duracaoMinutos),
      classificacaoIndicativa: parseInt(classificacaoIndicativa),
      emAlta,
    };

    const promessa = id
      ? catalogoServico.atualizar(id, dados)
      : catalogoServico.criar(dados);

    promessa
      .then(() => navegar("/catalogo"))
      .catch((err) => {
        const mensagem = err.response?.data?.mensagem || "Erro ao salvar conteúdo.";
        definirErros({ geral: mensagem });
      })
      .finally(() => definirEnviando(false));
  }

  if (carregando) return <Carregando mensagem="Carregando dados do conteúdo..." />;

  return (
    <div className="pagina">
      <div className="pagina-cabecalho">
        <div>
          <h1 className="pagina-titulo">{id ? "✏️ Editar Conteúdo" : "➕ Novo Conteúdo"}</h1>
          <p className="pagina-subtitulo">{id ? "Atualize os dados do conteúdo no catálogo" : "Adicione um novo filme, série ou documentário"}</p>
        </div>
        <Link to="/catalogo" className="btn btn-neutro">← Voltar</Link>
      </div>

      <div className="formulario-container">
        <form id="formulario-conteudo" onSubmit={salvar} className="formulario">
          {erros.geral && <div className="alerta alerta-erro">{erros.geral}</div>}

          <div className="campo">
            <label htmlFor="titulo-conteudo" className="campo-rotulo">Título *</label>
            <input
              id="titulo-conteudo"
              type="text"
              className={`campo-entrada ${erros.titulo ? "campo-invalido" : ""}`}
              value={titulo}
              onChange={(e) => definirTitulo(e.target.value)}
              placeholder="Ex: Interestelar"
              maxLength={80}
            />
            {erros.titulo && <span className="campo-erro">{erros.titulo}</span>}
          </div>

          <div className="linha-dupla">
            <div className="campo">
              <label htmlFor="categoria-conteudo" className="campo-rotulo">Categoria *</label>
              <select
                id="categoria-conteudo"
                className={`campo-entrada ${erros.categoria ? "campo-invalido" : ""}`}
                value={categoria}
                onChange={(e) => definirCategoria(e.target.value)}
              >
                <option value="">Selecione</option>
                <option value="Filme">Filme</option>
                <option value="Série">Série</option>
                <option value="Documentário">Documentário</option>
                <option value="Animação">Animação</option>
              </select>
              {erros.categoria && <span className="campo-erro">{erros.categoria}</span>}
            </div>

            <div className="campo">
              <label htmlFor="genero-conteudo" className="campo-rotulo">Gênero *</label>
              <select
                id="genero-conteudo"
                className={`campo-entrada ${erros.genero ? "campo-invalido" : ""}`}
                value={genero}
                onChange={(e) => definirGenero(e.target.value)}
              >
                <option value="">Selecione</option>
                <option value="Ação">Ação</option>
                <option value="Comédia">Comédia</option>
                <option value="Drama">Drama</option>
                <option value="Terror">Terror</option>
                <option value="Ficção Científica">Ficção Científica</option>
                <option value="Romance">Romance</option>
                <option value="Suspense">Suspense</option>
                <option value="Aventura">Aventura</option>
              </select>
              {erros.genero && <span className="campo-erro">{erros.genero}</span>}
            </div>
          </div>

          <div className="linha-dupla">
            <div className="campo">
              <label htmlFor="ano-lancamento" className="campo-rotulo">Ano de Lançamento *</label>
              <input
                id="ano-lancamento"
                type="number"
                min="1900"
                max="2100"
                className={`campo-entrada ${erros.anoLancamento ? "campo-invalido" : ""}`}
                value={anoLancamento}
                onChange={(e) => definirAnoLancamento(e.target.value)}
              />
              {erros.anoLancamento && <span className="campo-erro">{erros.anoLancamento}</span>}
            </div>

            <div className="campo">
              <label htmlFor="duracao-minutos" className="campo-rotulo">Duração (minutos) *</label>
              <input
                id="duracao-minutos"
                type="number"
                min="1"
                max="600"
                className={`campo-entrada ${erros.duracaoMinutos ? "campo-invalido" : ""}`}
                value={duracaoMinutos}
                onChange={(e) => definirDuracaoMinutos(e.target.value)}
                placeholder="120"
              />
              {erros.duracaoMinutos && <span className="campo-erro">{erros.duracaoMinutos}</span>}
            </div>
          </div>

          <div className="linha-dupla">
            <div className="campo">
              <label htmlFor="classificacao-indicativa" className="campo-rotulo">Classificação Indicativa *</label>
              <select
                id="classificacao-indicativa"
                className="campo-entrada"
                value={classificacaoIndicativa}
                onChange={(e) => definirClassificacaoIndicativa(e.target.value)}
              >
                <option value={0}>Livre</option>
                <option value={10}>10+</option>
                <option value={12}>12+</option>
                <option value={14}>14+</option>
                <option value={16}>16+</option>
                <option value={18}>18+</option>
              </select>
            </div>

            <div className="campo">
              <label className="campo-rotulo">Em Alta</label>
              <label id="toggle-em-alta" className="toggle">
                <input
                  type="checkbox"
                  checked={emAlta}
                  onChange={(e) => definirEmAlta(e.target.checked)}
                />
                <span className="toggle-slider"></span>
                <span className="toggle-texto">{emAlta ? "🔥 Em Alta" : "Normal"}</span>
              </label>
            </div>
          </div>

          <div className="formulario-acoes">
            <Link to="/catalogo" className="btn btn-neutro">Cancelar</Link>
            <button id="btn-salvar-conteudo" type="submit" className="btn btn-primario" disabled={enviando}>
              {enviando ? "Salvando..." : "💾 Salvar Conteúdo"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default FormularioCatalogo;
