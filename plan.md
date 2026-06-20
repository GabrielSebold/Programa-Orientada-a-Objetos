# PLANO.md — Frontend React consumindo WebService REST (Backend já existente)

> Documento de especificação completo do projeto, para ser usado por uma IA de
> desenvolvimento (ou por você) como guia único de implementação.
> Atividade: Frontend para integração com Backend REST | Individual ou em dupla

---

## 1. Visão Geral

Aplicação **Frontend em React JS** que consome um **WebService REST** já
desenvolvido (backend pronto), com pelo menos **4 recursos diferentes**, cada
um com rotinas de acesso implementando os 4 métodos do CRUD: **GET, POST, PUT
e DELETE**.

Todo o código (pastas, arquivos, componentes, funções, variáveis) deve ser
escrito **em português**, exceto palavras-chave e nomes que o próprio React /
JavaScript exigem em inglês (`useState`, `useEffect`, `import`, `export`,
`props`, etc.).

> Este plano usa **nomes genéricos de exemplo** (`recurso1`, `recurso2`,
> `produtos`, etc.). Antes de implementar, troque pelos nomes reais dos 4
> recursos do seu backend (ex.: `produtos`, `clientes`, `pedidos`,
> `categorias`) e ajuste os campos de cada formulário conforme os campos
> reais de cada tabela.

---

## 2. Requisitos do trabalho (o que precisa existir no final)

| Requisito                                        | Onde fica resolvido neste plano      |
| ------------------------------------------------ | ------------------------------------ |
| Frontend em React JS                             | Seção 4 (estrutura do projeto)       |
| Integração com o Backend                         | Seção 5 (camada de serviços / Axios) |
| Pelo menos 4 serviços com GET, POST, PUT, DELETE | Seção 5                              |
| Visual do projeto                                | Seção 7 (layout e estilo)            |
| Funcionamento da integração                      | Seção 8 (passo a passo / testes)     |
| Estrutura do código fonte                        | Seção 4                              |

---

## 3. Pré-requisitos antes de começar

- [ ] Confirmar a **URL base** da API do backend (ex.: `http://localhost:8000/api`)
- [ ] Confirmar se o backend tem **CORS liberado** para o domínio do React
- [ ] Anotar o nome de cada um dos 4 recursos e os campos de cada um (necessários para os formulários)
- [ ] Backend rodando localmente (ou publicado) durante todo o desenvolvimento do front, para testar a integração em tempo real

---

## 4. Estrutura de pastas (tudo em português)

```
src/
  servicos/
    api.js                   <- configuração base do Axios
    recurso1Servico.js
    recurso2Servico.js
    recurso3Servico.js
    recurso4Servico.js

  componentes/
    Menu.jsx
    Cabecalho.jsx
    Carregando.jsx            <- indicador de "carregando..." (opcional)

  paginas/
    Recurso1/
      Listar.jsx
      Formulario.jsx          <- serve para criar e editar
    Recurso2/
      Listar.jsx
      Formulario.jsx
    Recurso3/
      Listar.jsx
      Formulario.jsx
    Recurso4/
      Listar.jsx
      Formulario.jsx

  App.jsx
  index.js
  index.css
```

---

## 5. Camada de serviços (Axios) — repetida 4 vezes

### `servicos/api.js`

```js
import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:8000/api", // trocar pela URL real do backend
});

export default api;
```

### `servicos/produtoServico.js` (exemplo — replicar para os outros 3 recursos)

```js
import api from "./api";

const listar = () => api.get("/produtos");
const buscarPorId = (id) => api.get(`/produtos/${id}`);
const criar = (dados) => api.post("/produtos", dados);
const atualizar = (id, dados) => api.put(`/produtos/${id}`, dados);
const excluir = (id) => api.delete(`/produtos/${id}`);

export default { listar, buscarPorId, criar, atualizar, excluir };
```

**Checklist por recurso (repetir para cada um dos 4):**

- [ ] Arquivo `recursoXServico.js` criado
- [ ] Método `listar()` (GET) funcionando
- [ ] Método `buscarPorId(id)` (GET) funcionando
- [ ] Método `criar(dados)` (POST) funcionando
- [ ] Método `atualizar(id, dados)` (PUT) funcionando
- [ ] Método `excluir(id)` (DELETE) funcionando

---

## 6. Páginas — Listagem e Formulário (por recurso)

### `paginas/Recurso/Listar.jsx`

```jsx
import { useEffect, useState } from "react";
import produtoServico from "../../servicos/produtoServico";

function Listar() {
  const [produtos, definirProdutos] = useState([]);

  useEffect(() => {
    carregarProdutos();
  }, []);

  function carregarProdutos() {
    produtoServico.listar().then((resposta) => definirProdutos(resposta.data));
  }

  function excluirProduto(id) {
    produtoServico.excluir(id).then(() => carregarProdutos());
  }

  return (
    <div>
      <h2>Produtos</h2>
      <a href="/produtos/criar">Novo Produto</a>
      <table>
        <thead>
          <tr>
            <th>Nome</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {produtos.map((produto) => (
            <tr key={produto.id}>
              <td>{produto.nome}</td>
              <td>
                <a href={`/produtos/editar/${produto.id}`}>Editar</a>
                <button onClick={() => excluirProduto(produto.id)}>
                  Excluir
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
export default Listar;
```

### `paginas/Recurso/Formulario.jsx`

Lógica: se a rota trouxer um `id`, busca os dados (`buscarPorId`) e usa
`atualizar` ao salvar; se não trouxer `id`, é um cadastro novo e usa `criar`.

```jsx
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import produtoServico from "../../servicos/produtoServico";

function Formulario() {
  const { id } = useParams();
  const navegar = useNavigate();
  const [nome, definirNome] = useState("");
  const [preco, definirPreco] = useState("");

  useEffect(() => {
    if (id) {
      produtoServico.buscarPorId(id).then((resposta) => {
        definirNome(resposta.data.nome);
        definirPreco(resposta.data.preco);
      });
    }
  }, [id]);

  function salvar(evento) {
    evento.preventDefault();
    const dados = { nome, preco };

    if (id) {
      produtoServico.atualizar(id, dados).then(() => navegar("/produtos"));
    } else {
      produtoServico.criar(dados).then(() => navegar("/produtos"));
    }
  }

  return (
    <form onSubmit={salvar}>
      <label>Nome</label>
      <input value={nome} onChange={(e) => definirNome(e.target.value)} />

      <label>Preço</label>
      <input value={preco} onChange={(e) => definirPreco(e.target.value)} />

      <button type="submit">Salvar</button>
    </form>
  );
}
export default Formulario;
```

> Repetir essa mesma estrutura de `Listar.jsx` + `Formulario.jsx` para os
> outros 3 recursos, trocando o serviço importado e os campos do formulário.

---

## 7. Navegação, layout e visual

### `componentes/Menu.jsx`

```jsx
function Menu() {
  return (
    <nav>
      <a href="/produtos">Produtos</a>
      <a href="/clientes">Clientes</a>
      <a href="/pedidos">Pedidos</a>
      <a href="/categorias">Categorias</a>
    </nav>
  );
}
export default Menu;
```

### `App.jsx` (rotas com React Router)

```jsx
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Menu from "./componentes/Menu";
import ListarProdutos from "./paginas/Produtos/Listar";
import FormularioProdutos from "./paginas/Produtos/Formulario";
// importar as demais páginas dos outros 3 recursos...

function App() {
  return (
    <BrowserRouter>
      <Menu />
      <Routes>
        <Route path="/produtos" element={<ListarProdutos />} />
        <Route path="/produtos/criar" element={<FormularioProdutos />} />
        <Route path="/produtos/editar/:id" element={<FormularioProdutos />} />
        {/* repetir para os outros 3 recursos */}
      </Routes>
    </BrowserRouter>
  );
}
export default App;
```

### Visual

- CSS básico em `index.css`: espaçamento, cores neutras, tabela com bordas leves, botões com destaque (ex.: vermelho para excluir, azul para editar/salvar)
- Manter o mesmo layout (menu fixo no topo) em todas as páginas — reforça a organização do código e fica visualmente coerente
- Instalar e usar **Bootstrap** ou **Tailwind** via CDN/npm se quiser ganhar tempo na estilização sem escrever muito CSS

---

## 8. Passo a passo de implementação

1. Criar o projeto: `npx create-react-app meu-frontend` (ou `npm create vite@latest`)
2. Instalar dependências: `npm install axios react-router-dom`
3. Criar `servicos/api.js` com a URL base do backend
4. Implementar o serviço do **1º recurso** e testar `listar()` isoladamente (console.log da resposta)
5. Criar `Listar.jsx` do 1º recurso e confirmar que os dados aparecem na tela
6. Criar `Formulario.jsx` do 1º recurso e testar **criar** e **editar** (POST e PUT)
7. Testar o **excluir** (DELETE) na listagem
8. Repetir os passos 4–7 para o **2º, 3º e 4º recurso**
9. Criar `Menu.jsx` e configurar todas as rotas em `App.jsx`
10. Ajustar o CSS/visual geral da aplicação
11. Testar o fluxo completo de ponta a ponta com o backend rodando (criar, listar, editar, excluir em todos os 4 recursos)
12. Revisar a estrutura de pastas e nomes de arquivos antes da entrega

---

## 9. Checklist final mapeado para avaliação

- [ ] Visual do projeto (layout organizado, CSS aplicado, navegação clara)
- [ ] Rotinas de consumo implementadas para os 4 recursos (GET, POST, PUT, DELETE em cada um)
- [ ] Integração com o backend funcionando de ponta a ponta (testado com o backend real rodando)
- [ ] Estrutura do código fonte organizada (`servicos/`, `componentes/`, `paginas/`)
- [ ] Repositório no GitHub atualizado
- [ ] Conferir se é entrega individual ou em dupla, conforme definido no Backend
