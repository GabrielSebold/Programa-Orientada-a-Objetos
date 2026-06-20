import { useEffect, useState } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import assinantesServico from "../../servicos/assinantesServico";
import planosServico from "../../servicos/planosServico";
import Carregando from "../../componentes/Carregando";

function FormularioAssinante() {
  const { id } = useParams();
  const navegar = useNavigate();
  const [nome, definirNome] = useState("");
  const [email, definirEmail] = useState("");
  const [planoId, definirPlanoId] = useState("");
  const [ativo, definirAtivo] = useState(true);
  const [planos, definirPlanos] = useState([]);
  const [carregando, definirCarregando] = useState(false);
  const [enviando, definirEnviando] = useState(false);
  const [erros, definirErros] = useState({});

  useEffect(() => {
    planosServico.listar().then((r) => definirPlanos(r.data));

    if (id) {
      definirCarregando(true);
      assinantesServico
        .buscarPorId(id)
        .then((resposta) => {
          const dados = resposta.data;
          definirNome(dados.nome);
          definirEmail(dados.email);
          definirPlanoId(dados.planoId);
          definirAtivo(dados.ativo);
        })
        .finally(() => definirCarregando(false));
    }
  }, [id]);

  function validar() {
    const novosErros = {};
    if (!nome.trim() || nome.length < 3) novosErros.nome = "Nome deve ter pelo menos 3 caracteres.";
    if (!email.trim() || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email))
      novosErros.email = "E-mail inválido.";
    if (!planoId) novosErros.planoId = "Selecione um plano.";
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

    const dados = { nome, email, planoId: parseInt(planoId), ativo };

    const promessa = id
      ? assinantesServico.atualizar(id, dados)
      : assinantesServico.criar(dados);

    promessa
      .then(() => navegar("/assinantes"))
      .catch((err) => {
        const mensagem = err.response?.data?.mensagem || "Erro ao salvar assinante.";
        definirErros({ geral: mensagem });
      })
      .finally(() => definirEnviando(false));
  }

  if (carregando) return <Carregando mensagem="Carregando dados do assinante..." />;

  return (
    <div className="pagina">
      <div className="pagina-cabecalho">
        <div>
          <h1 className="pagina-titulo">{id ? "✏️ Editar Assinante" : "➕ Novo Assinante"}</h1>
          <p className="pagina-subtitulo">
            {id ? "Atualize os dados do assinante" : "Cadastre um novo assinante na plataforma"}
          </p>
        </div>
        <Link to="/assinantes" className="btn btn-neutro">← Voltar</Link>
      </div>

      <div className="formulario-container">
        <form id="formulario-assinante" onSubmit={salvar} className="formulario">
          {erros.geral && <div className="alerta alerta-erro">{erros.geral}</div>}

          <div className="campo">
            <label htmlFor="nome-assinante" className="campo-rotulo">Nome Completo *</label>
            <input
              id="nome-assinante"
              type="text"
              className={`campo-entrada ${erros.nome ? "campo-invalido" : ""}`}
              value={nome}
              onChange={(e) => definirNome(e.target.value)}
              placeholder="Ex: João da Silva"
              maxLength={80}
            />
            {erros.nome && <span className="campo-erro">{erros.nome}</span>}
          </div>

          <div className="campo">
            <label htmlFor="email-assinante" className="campo-rotulo">E-mail *</label>
            <input
              id="email-assinante"
              type="email"
              className={`campo-entrada ${erros.email ? "campo-invalido" : ""}`}
              value={email}
              onChange={(e) => definirEmail(e.target.value)}
              placeholder="joao@email.com"
              maxLength={120}
            />
            {erros.email && <span className="campo-erro">{erros.email}</span>}
          </div>

          <div className="campo">
            <label htmlFor="plano-assinante" className="campo-rotulo">Plano de Assinatura *</label>
            <select
              id="plano-assinante"
              className={`campo-entrada ${erros.planoId ? "campo-invalido" : ""}`}
              value={planoId}
              onChange={(e) => definirPlanoId(e.target.value)}
            >
              <option value="">Selecione um plano</option>
              {planos.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.nome} — R$ {Number(p.precoMensal).toFixed(2)} / mês ({p.qualidadeVideo})
                </option>
              ))}
            </select>
            {erros.planoId && <span className="campo-erro">{erros.planoId}</span>}
          </div>

          <div className="campo">
            <label className="campo-rotulo">Status da Assinatura</label>
            <label id="toggle-ativo" className="toggle">
              <input
                type="checkbox"
                checked={ativo}
                onChange={(e) => definirAtivo(e.target.checked)}
              />
              <span className="toggle-slider"></span>
              <span className="toggle-texto">{ativo ? "✅ Ativo" : "❌ Inativo"}</span>
            </label>
          </div>

          <div className="formulario-acoes">
            <Link to="/assinantes" className="btn btn-neutro">Cancelar</Link>
            <button id="btn-salvar-assinante" type="submit" className="btn btn-primario" disabled={enviando}>
              {enviando ? "Salvando..." : "💾 Salvar Assinante"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default FormularioAssinante;
