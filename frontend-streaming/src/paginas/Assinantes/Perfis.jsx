import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import perfisServico from "../../servicos/perfisServico";
import assinantesServico from "../../servicos/assinantesServico";
import Carregando from "../../componentes/Carregando";

function GerenciarPerfis() {
  const { id: assinanteId } = useParams();
  const [assinante, definirAssinante] = useState(null);
  const [perfis, definirPerfis] = useState([]);
  const [carregando, definirCarregando] = useState(true);
  const [erro, definirErro] = useState(null);
  const [mostrarFormulario, definirMostrarFormulario] = useState(false);
  const [nome, definirNome] = useState("");
  const [idioma, definirIdioma] = useState("Português");
  const [infantil, definirInfantil] = useState(false);
  const [enviando, definirEnviando] = useState(false);
  const [erroForm, definirErroForm] = useState(null);
  const [confirmandoExclusao, definirConfirmandoExclusao] = useState(null);

  useEffect(() => {
    carregarDados();
  }, [assinanteId]);

  function carregarDados() {
    definirCarregando(true);
    definirErro(null);
    Promise.all([
      assinantesServico.buscarPorId(assinanteId),
      perfisServico.listar(assinanteId),
    ])
      .then(([resAssinante, resPerfis]) => {
        definirAssinante(resAssinante.data);
        definirPerfis(resPerfis.data);
      })
      .catch(() => definirErro("Não foi possível carregar os dados."))
      .finally(() => definirCarregando(false));
  }

  function adicionarPerfil(evento) {
    evento.preventDefault();
    if (!nome.trim() || nome.length < 2) {
      definirErroForm("Nome deve ter pelo menos 2 caracteres.");
      return;
    }
    definirErroForm(null);
    definirEnviando(true);

    perfisServico
      .criar(assinanteId, { nome, idioma, infantil })
      .then(() => {
        definirNome("");
        definirIdioma("Português");
        definirInfantil(false);
        definirMostrarFormulario(false);
        carregarDados();
      })
      .catch((err) => {
        const mensagem = err.response?.data?.mensagem || "Erro ao criar perfil.";
        definirErroForm(mensagem);
      })
      .finally(() => definirEnviando(false));
  }

  function excluirPerfil(perfilId) {
    perfisServico
      .excluir(assinanteId, perfilId)
      .then(() => {
        definirConfirmandoExclusao(null);
        carregarDados();
      })
      .catch((err) => {
        const mensagem = err.response?.data?.mensagem || "Erro ao excluir perfil.";
        alert(mensagem);
        definirConfirmandoExclusao(null);
      });
  }

  if (carregando) return <Carregando mensagem="Carregando perfis..." />;

  return (
    <div className="pagina">
      <div className="pagina-cabecalho">
        <div>
          <h1 className="pagina-titulo">👥 Perfis do Assinante</h1>
          {assinante && (
            <p className="pagina-subtitulo">
              Gerenciando perfis de <strong>{assinante.nome}</strong> — {assinante.email}
            </p>
          )}
        </div>
        <div className="cabecalho-acoes">
          <Link to="/assinantes" className="btn btn-neutro">← Voltar</Link>
          <button
            id="btn-adicionar-perfil"
            className="btn btn-primario"
            onClick={() => definirMostrarFormulario(!mostrarFormulario)}
          >
            {mostrarFormulario ? "✗ Cancelar" : "+ Adicionar Perfil"}
          </button>
        </div>
      </div>

      {erro && <div className="alerta alerta-erro">{erro}</div>}

      {mostrarFormulario && (
        <div className="formulario-container formulario-inline">
          <form id="formulario-perfil" onSubmit={adicionarPerfil} className="formulario">
            <h3 className="formulario-titulo">Novo Perfil</h3>
            {erroForm && <div className="alerta alerta-erro">{erroForm}</div>}

            <div className="linha-tripla">
              <div className="campo">
                <label htmlFor="nome-perfil" className="campo-rotulo">Nome do Perfil *</label>
                <input
                  id="nome-perfil"
                  type="text"
                  className="campo-entrada"
                  value={nome}
                  onChange={(e) => definirNome(e.target.value)}
                  placeholder="Ex: Leonardo"
                  maxLength={40}
                />
              </div>

              <div className="campo">
                <label htmlFor="idioma-perfil" className="campo-rotulo">Idioma Preferido *</label>
                <select
                  id="idioma-perfil"
                  className="campo-entrada"
                  value={idioma}
                  onChange={(e) => definirIdioma(e.target.value)}
                >
                  <option>Português</option>
                  <option>Inglês</option>
                  <option>Espanhol</option>
                  <option>Francês</option>
                  <option>Japonês</option>
                </select>
              </div>

              <div className="campo">
                <label className="campo-rotulo">Perfil Infantil</label>
                <label id="toggle-infantil" className="toggle">
                  <input
                    type="checkbox"
                    checked={infantil}
                    onChange={(e) => definirInfantil(e.target.checked)}
                  />
                  <span className="toggle-slider"></span>
                  <span className="toggle-texto">{infantil ? "🧒 Infantil" : "Adulto"}</span>
                </label>
              </div>
            </div>

            <div className="formulario-acoes">
              <button id="btn-salvar-perfil" type="submit" className="btn btn-primario" disabled={enviando}>
                {enviando ? "Salvando..." : "➕ Adicionar Perfil"}
              </button>
            </div>
          </form>
        </div>
      )}

      {!carregando && perfis.length === 0 && (
        <div className="estado-vazio">
          <span>👥</span>
          <p>Nenhum perfil criado ainda.</p>
          <button className="btn btn-primario" onClick={() => definirMostrarFormulario(true)}>
            Criar primeiro perfil
          </button>
        </div>
      )}

      {perfis.length > 0 && (
        <div className="grade-cards grade-cards-pequenos">
          {perfis.map((perfil) => (
            <div key={perfil.id} className="card card-perfil">
              <div className="perfil-avatar">
                {perfil.infantil ? "🧒" : "👤"}
              </div>
              <div className="card-corpo">
                <h3 className="card-titulo">{perfil.nome}</h3>
                <div className="info-linha">
                  <span className="info-rotulo">🌐 Idioma</span>
                  <span className="info-valor">{perfil.idioma}</span>
                </div>
                <div className="info-linha">
                  <span className="info-rotulo">Tipo</span>
                  <span className={`badge ${perfil.infantil ? "badge-infantil" : "badge-adulto"}`}>
                    {perfil.infantil ? "🧒 Infantil" : "👤 Adulto"}
                  </span>
                </div>
              </div>
              <div className="card-rodape">
                {confirmandoExclusao === perfil.id ? (
                  <div className="confirmacao">
                    <span>Excluir perfil?</span>
                    <button
                      id={`btn-confirmar-excluir-perfil-${perfil.id}`}
                      className="btn btn-perigo btn-pequeno"
                      onClick={() => excluirPerfil(perfil.id)}
                    >
                      Sim
                    </button>
                    <button
                      className="btn btn-neutro btn-pequeno"
                      onClick={() => definirConfirmandoExclusao(null)}
                    >
                      Não
                    </button>
                  </div>
                ) : (
                  <button
                    id={`btn-excluir-perfil-${perfil.id}`}
                    className="btn btn-perigo btn-pequeno"
                    onClick={() => definirConfirmandoExclusao(perfil.id)}
                  >
                    🗑️ Remover
                  </button>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default GerenciarPerfis;
