import { useEffect, useState } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import planosServico from "../../servicos/planosServico";
import Carregando from "../../componentes/Carregando";

function FormularioPlano() {
  const { id } = useParams();
  const navegar = useNavigate();
  const [nome, definirNome] = useState("");
  const [precoMensal, definirPrecoMensal] = useState("");
  const [qualidadeVideo, definirQualidadeVideo] = useState("");
  const [quantidadeTelas, definirQuantidadeTelas] = useState(1);
  const [carregando, definirCarregando] = useState(false);
  const [enviando, definirEnviando] = useState(false);
  const [erros, definirErros] = useState({});

  useEffect(() => {
    if (id) {
      definirCarregando(true);
      planosServico
        .buscarPorId(id)
        .then((resposta) => {
          const dados = resposta.data;
          definirNome(dados.nome);
          definirPrecoMensal(dados.precoMensal);
          definirQualidadeVideo(dados.qualidadeVideo);
          definirQuantidadeTelas(dados.quantidadeTelas);
        })
        .finally(() => definirCarregando(false));
    }
  }, [id]);

  function validar() {
    const novosErros = {};
    if (!nome.trim() || nome.length < 3) novosErros.nome = "Nome deve ter pelo menos 3 caracteres.";
    if (!precoMensal || precoMensal <= 0) novosErros.precoMensal = "Preço deve ser maior que zero.";
    if (!qualidadeVideo.trim()) novosErros.qualidadeVideo = "Qualidade de vídeo é obrigatória.";
    if (!quantidadeTelas || quantidadeTelas < 1 || quantidadeTelas > 10)
      novosErros.quantidadeTelas = "Quantidade de telas deve ser entre 1 e 10.";
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
      nome,
      precoMensal: parseFloat(precoMensal),
      qualidadeVideo,
      quantidadeTelas: parseInt(quantidadeTelas),
    };

    const promessa = id
      ? planosServico.atualizar(id, dados)
      : planosServico.criar(dados);

    promessa
      .then(() => navegar("/planos"))
      .catch((err) => {
        const mensagem = err.response?.data?.mensagem || "Erro ao salvar plano.";
        definirErros({ geral: mensagem });
      })
      .finally(() => definirEnviando(false));
  }

  if (carregando) return <Carregando mensagem="Carregando dados do plano..." />;

  return (
    <div className="pagina">
      <div className="pagina-cabecalho">
        <div>
          <h1 className="pagina-titulo">{id ? "✏️ Editar Plano" : "➕ Novo Plano"}</h1>
          <p className="pagina-subtitulo">{id ? "Atualize os dados do plano de assinatura" : "Preencha os dados do novo plano"}</p>
        </div>
        <Link to="/planos" className="btn btn-neutro">← Voltar</Link>
      </div>

      <div className="formulario-container">
        <form id="formulario-plano" onSubmit={salvar} className="formulario">
          {erros.geral && <div className="alerta alerta-erro">{erros.geral}</div>}

          <div className="campo">
            <label htmlFor="nome-plano" className="campo-rotulo">Nome do Plano *</label>
            <input
              id="nome-plano"
              type="text"
              className={`campo-entrada ${erros.nome ? "campo-invalido" : ""}`}
              value={nome}
              onChange={(e) => definirNome(e.target.value)}
              placeholder="Ex: Plano Premium"
              maxLength={40}
            />
            {erros.nome && <span className="campo-erro">{erros.nome}</span>}
          </div>

          <div className="campo">
            <label htmlFor="preco-mensal" className="campo-rotulo">Preço Mensal (R$) *</label>
            <input
              id="preco-mensal"
              type="number"
              step="0.01"
              min="0.01"
              max="1000"
              className={`campo-entrada ${erros.precoMensal ? "campo-invalido" : ""}`}
              value={precoMensal}
              onChange={(e) => definirPrecoMensal(e.target.value)}
              placeholder="29.90"
            />
            {erros.precoMensal && <span className="campo-erro">{erros.precoMensal}</span>}
          </div>

          <div className="campo">
            <label htmlFor="qualidade-video" className="campo-rotulo">Qualidade de Vídeo *</label>
            <select
              id="qualidade-video"
              className={`campo-entrada ${erros.qualidadeVideo ? "campo-invalido" : ""}`}
              value={qualidadeVideo}
              onChange={(e) => definirQualidadeVideo(e.target.value)}
            >
              <option value="">Selecione a qualidade</option>
              <option value="SD">SD (480p)</option>
              <option value="HD">HD (720p)</option>
              <option value="Full HD">Full HD (1080p)</option>
              <option value="4K">4K Ultra HD</option>
            </select>
            {erros.qualidadeVideo && <span className="campo-erro">{erros.qualidadeVideo}</span>}
          </div>

          <div className="campo">
            <label htmlFor="quantidade-telas" className="campo-rotulo">Quantidade de Telas *</label>
            <input
              id="quantidade-telas"
              type="number"
              min="1"
              max="10"
              className={`campo-entrada ${erros.quantidadeTelas ? "campo-invalido" : ""}`}
              value={quantidadeTelas}
              onChange={(e) => definirQuantidadeTelas(e.target.value)}
            />
            {erros.quantidadeTelas && <span className="campo-erro">{erros.quantidadeTelas}</span>}
          </div>

          <div className="formulario-acoes">
            <Link to="/planos" className="btn btn-neutro">Cancelar</Link>
            <button id="btn-salvar-plano" type="submit" className="btn btn-primario" disabled={enviando}>
              {enviando ? "Salvando..." : "💾 Salvar Plano"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default FormularioPlano;
