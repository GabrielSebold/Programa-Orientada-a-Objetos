import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import planosServico from "../../servicos/planosServico";
import Carregando from "../../componentes/Carregando";

function ListarPlanos() {
  const [planos, definirPlanos] = useState([]);
  const [carregando, definirCarregando] = useState(true);
  const [erro, definirErro] = useState(null);
  const [confirmandoExclusao, definirConfirmandoExclusao] = useState(null);

  useEffect(() => {
    carregarPlanos();
  }, []);

  function carregarPlanos() {
    definirCarregando(true);
    definirErro(null);
    planosServico
      .listar()
      .then((resposta) => definirPlanos(resposta.data))
      .catch(() => definirErro("Não foi possível carregar os planos. Verifique se o backend está rodando."))
      .finally(() => definirCarregando(false));
  }

  function excluirPlano(id) {
    planosServico
      .excluir(id)
      .then(() => {
        definirConfirmandoExclusao(null);
        carregarPlanos();
      })
      .catch((err) => {
        const mensagem = err.response?.data?.mensagem || "Erro ao excluir plano.";
        alert(mensagem);
        definirConfirmandoExclusao(null);
      });
  }

  return (
    <div className="pagina">
      <div className="pagina-cabecalho">
        <div>
          <h1 className="pagina-titulo">💎 Planos de Assinatura</h1>
          <p className="pagina-subtitulo">Gerencie os planos disponíveis na plataforma</p>
        </div>
        <Link to="/planos/criar" id="btn-novo-plano" className="btn btn-primario">
          + Novo Plano
        </Link>
      </div>

      {carregando && <Carregando mensagem="Carregando planos..." />}
      {erro && <div className="alerta alerta-erro">{erro}</div>}

      {!carregando && !erro && planos.length === 0 && (
        <div className="estado-vazio">
          <span>💎</span>
          <p>Nenhum plano cadastrado ainda.</p>
          <Link to="/planos/criar" className="btn btn-primario">Criar primeiro plano</Link>
        </div>
      )}

      {!carregando && !erro && planos.length > 0 && (
        <div className="grade-cards">
          {planos.map((plano) => (
            <div key={plano.id} className="card">
              <div className="card-topo">
                <h2 className="card-titulo">{plano.nome}</h2>
                <span className="badge badge-info">{plano.qualidadeVideo}</span>
              </div>
              <div className="card-corpo">
                <div className="info-linha">
                  <span className="info-rotulo">💰 Preço Mensal</span>
                  <span className="info-valor destaque">
                    R$ {Number(plano.precoMensal).toFixed(2)}
                  </span>
                </div>
                <div className="info-linha">
                  <span className="info-rotulo">🖥️ Telas simultâneas</span>
                  <span className="info-valor">{plano.quantidadeTelas}</span>
                </div>
              </div>
              <div className="card-rodape">
                <Link to={`/planos/editar/${plano.id}`} className="btn btn-secundario btn-pequeno">
                  ✏️ Editar
                </Link>
                {confirmandoExclusao === plano.id ? (
                  <div className="confirmacao">
                    <span>Confirmar exclusão?</span>
                    <button
                      id={`btn-confirmar-excluir-plano-${plano.id}`}
                      className="btn btn-perigo btn-pequeno"
                      onClick={() => excluirPlano(plano.id)}
                    >
                      Sim, excluir
                    </button>
                    <button
                      className="btn btn-neutro btn-pequeno"
                      onClick={() => definirConfirmandoExclusao(null)}
                    >
                      Cancelar
                    </button>
                  </div>
                ) : (
                  <button
                    id={`btn-excluir-plano-${plano.id}`}
                    className="btn btn-perigo btn-pequeno"
                    onClick={() => definirConfirmandoExclusao(plano.id)}
                  >
                    🗑️ Excluir
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

export default ListarPlanos;
