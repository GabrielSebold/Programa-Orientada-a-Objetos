function Carregando({ mensagem = "Carregando..." }) {
  return (
    <div className="carregando">
      <div className="spinner" aria-label={mensagem}></div>
      <p>{mensagem}</p>
    </div>
  );
}

export default Carregando;
