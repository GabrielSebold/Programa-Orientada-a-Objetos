import { Link, useLocation } from "react-router-dom";
import "./Menu.css";

function Menu() {
  const localizacao = useLocation();

  const links = [
    { para: "/", rotulo: "🏠 Início" },
    { para: "/planos", rotulo: "💎 Planos" },
    { para: "/catalogo", rotulo: "🎬 Catálogo" },
    { para: "/assinantes", rotulo: "👤 Assinantes" },
  ];

  return (
    <nav id="menu-principal" className="menu">
      <div className="menu-logo">
        <span className="logo-icone">▶</span>
        <span className="logo-texto">StreamFlix</span>
      </div>
      <ul className="menu-links">
        {links.map((link) => (
          <li key={link.para}>
            <Link
              to={link.para}
              className={`menu-link ${localizacao.pathname === link.para || (link.para !== "/" && localizacao.pathname.startsWith(link.para)) ? "ativo" : ""}`}
            >
              {link.rotulo}
            </Link>
          </li>
        ))}
      </ul>
    </nav>
  );
}

export default Menu;
