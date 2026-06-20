import { BrowserRouter, Routes, Route } from "react-router-dom";
import Menu from "./componentes/Menu";
import Inicio from "./paginas/Inicio";
import ListarPlanos from "./paginas/Planos/Listar";
import FormularioPlano from "./paginas/Planos/Formulario";
import ListarCatalogo from "./paginas/Catalogo/Listar";
import FormularioCatalogo from "./paginas/Catalogo/Formulario";
import ListarAssinantes from "./paginas/Assinantes/Listar";
import FormularioAssinante from "./paginas/Assinantes/Formulario";
import GerenciarPerfis from "./paginas/Assinantes/Perfis";

function App() {
  return (
    <BrowserRouter>
      <Menu />
      <main className="conteudo-principal">
        <Routes>
          <Route path="/" element={<Inicio />} />

          {/* Planos */}
          <Route path="/planos" element={<ListarPlanos />} />
          <Route path="/planos/criar" element={<FormularioPlano />} />
          <Route path="/planos/editar/:id" element={<FormularioPlano />} />

          {/* Catálogo */}
          <Route path="/catalogo" element={<ListarCatalogo />} />
          <Route path="/catalogo/criar" element={<FormularioCatalogo />} />
          <Route path="/catalogo/editar/:id" element={<FormularioCatalogo />} />

          {/* Assinantes */}
          <Route path="/assinantes" element={<ListarAssinantes />} />
          <Route path="/assinantes/criar" element={<FormularioAssinante />} />
          <Route path="/assinantes/editar/:id" element={<FormularioAssinante />} />
          <Route path="/assinantes/:id/perfis" element={<GerenciarPerfis />} />
        </Routes>
      </main>
    </BrowserRouter>
  );
}

export default App;
