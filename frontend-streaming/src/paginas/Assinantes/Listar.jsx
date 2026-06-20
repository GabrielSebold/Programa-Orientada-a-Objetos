import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import assinantesServico from "../../servicos/assinantesServico";
import planosServico from "../../servicos/planosServico";
import Carregando from "../../componentes/Carregando";

function ListarAssinantes() {
  const [assinantes, definirAssinantes] = useState([]);
  const [planos, definirPlanos] = useState([]);
  const [carregando, definirCarregando] = useState(true);
  const [erro, definirErro] = useState(null);
  const [filtroAtivo, definirFiltroAtivo] = useState("");
  const [filtroPlano, definirFiltroPlano] = useState("");
  const [confirmandoExclusao, definirConfirmandoExclusao] = useState(null);

  useEffect(() => {
    planosServico.listar().then((r) => definirPlanos(r.data));
  }, []);

  useEffect(() => {
    carregarAssinantes();
  }, [filtroAtivo, filtroPlano]);

  function carregarAssinantes() {
    definirCarregando(true);
    definirErro(null);
    const params = {};
    if (filtroAtivo !== "") params.ativo = filtroAtivo === "true";
    if (filtroPlano) params.planoId = filtroPlano;

    assinantesServico
      .listar(params)
      .then((resposta) => definirAssinantes(resposta.data))
      .catch(() => definirErro("Não foi possível carregar os assinantes."))
      .finally(() => definirCarregando(false));
  }

  function excluirAssinante(id) {
    assinantesServico
      .excluir(id)
      .then(() => {
        definirConfirmandoExclusao(null);
        carregarAssinantes();
      })
      .catch((err) => {
        const mensagem = err.response?.data?.mensagem || "Erro ao excluir assinante.";
        alert(mensagem);
        definirConfirmandoExclusao(null);
      });
  }

  function nomePlano(planoId) {
    const plano = planos.find((p) => p.id === planoId);
    return plano ? plano.nome : `Plano #${planoId}`;
  }

  return (
    <div className="pagina">
      <div className="pagina-cabecalho">
        <div>
          <h1 className="pagina-titulo">👤 Assinantes</h1>
          <p className="pagina-subtitulo">Gerencie os assinantes da plataforma</p>
        </div>
        <Link to="/assinantes/criar" id="btn-novo-assinante" className="btn btn-primario">
          + Novo Assinante
        </Link>
      </div>

      <div className="filtros">
        <select
          id="filtro-status"
          className="campo-entrada campo-pequeno"
          value={filtroAtivo}
          onChange={(e) => definirFiltroAtivo(e.target.value)}
        >
          <option value="">Todos os status</option>
          <option value="true">✅ Ativos</option>
          <option value="false">❌ Inativos</option>
        </select>

        <select
          id="filtro-plano"
          className="campo-entrada campo-pequeno"
          value={filtroPlano}
          onChange={(e) => definirFiltroPlano(e.target.value)}
        >
          <option value="">Todos os planos</option>
          {planos.map((p) => (
            <option key={p.id} value={p.id}>{p.nome}</option>
          ))}
        </select>
      </div>

      {carregando && <Carregando mensagem="Carregando assinantes..." />}
      {erro && <div className="alerta alerta-erro">{erro}</div>}

      {!carregando && !erro && assinantes.length === 0 && (
        <div className="estado-vazio">
          <span>👤</span>
          <p>Nenhum assinante encontrado.</p>
          <Link to="/assinantes/criar" className="btn btn-primario">Cadastrar assinante</Link>
        </div>
      )}

      {!carregando && !erro && assinantes.length > 0 && (
        <div className="tabela-container">
          <table className="tabela" id="tabela-assinantes">
            <thead>
              <tr>
                <th>Nome</th>
                <th>E-mail</th>
                <th>Plano</th>
                <th>Status</th>
                <th>Assinatura</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {assinantes.map((assinante) => (
                <tr key={assinante.id}>
                  <td className="td-titulo">{assinante.nome}</td>
                  <td className="td-email">{assinante.email}</td>
                  <td><span className="badge badge-info">{nomePlano(assinante.planoId)}</span></td>
                  <td>
                    <span className={`badge ${assinante.ativo ? "badge-sucesso" : "badge-erro"}`}>
                      {assinante.ativo ? "✅ Ativo" : "❌ Inativo"}
                    </span>
                  </td>
                  <td>{new Date(assinante.dataAssinatura).toLocaleDateString("pt-BR")}</td>
                  <td>
                    <div className="acoes">
                      <Link to={`/assinantes/editar/${assinante.id}`} className="btn btn-secundario btn-pequeno">
                        ✏️
                      </Link>
                      <Link to={`/assinantes/${assinante.id}/perfis`} className="btn btn-info btn-pequeno">
                        👥
                      </Link>
                      {confirmandoExclusao === assinante.id ? (
                        <>
                          <button
                            id={`btn-confirmar-excluir-assinante-${assinante.id}`}
                            className="btn btn-perigo btn-pequeno"
                            onClick={() => excluirAssinante(assinante.id)}
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
                          id={`btn-excluir-assinante-${assinante.id}`}
                          className="btn btn-perigo btn-pequeno"
                          onClick={() => definirConfirmandoExclusao(assinante.id)}
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

export default ListarAssinantes;
