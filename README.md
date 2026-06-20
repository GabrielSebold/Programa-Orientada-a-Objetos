# StreamFlix — Frontend React para API de Streaming

Frontend desenvolvido em **React JS** com **Vite** para consumir o WebService REST da **API de Streaming** (ASP.NET / C#).

## Tecnologias

- **React 19** + **Vite 8**
- **React Router DOM** — navegação SPA
- **Axios** — requisições HTTP à API REST
- CSS puro — design system próprio com tema escuro premium

## Recursos Implementados (4 serviços com CRUD completo)

| Recurso | Endpoints | Operações |
|---|---|---|
| **Planos** | `GET/POST/PUT/DELETE /api/planos` | Listar, criar, editar, excluir planos |
| **Catálogo** | `GET/POST/PUT/DELETE /api/catalogo` | Listar com filtros, criar, editar, excluir conteúdos |
| **Assinantes** | `GET/POST/PUT/DELETE /api/assinantes` | Listar com filtros, criar, editar, excluir assinantes |
| **Perfis** | `GET/POST/DELETE /api/assinantes/:id/perfis` | Gerenciar perfis vinculados a cada assinante |

## Estrutura de Pastas

```
frontend-streaming/
  src/
    servicos/
      api.js                    ← configuração base do Axios (URL base do backend)
      planosServico.js          ← CRUD de planos
      catalogoServico.js        ← CRUD do catálogo
      assinantesServico.js      ← CRUD de assinantes
      perfisServico.js          ← GET/POST/DELETE de perfis

    componentes/
      Menu.jsx                  ← navegação com destaque de rota ativa
      Menu.css
      Carregando.jsx            ← spinner de carregamento reutilizável

    paginas/
      Inicio.jsx                ← dashboard com estatísticas e conteúdos em alta
      Planos/
        Listar.jsx              ← listagem em cards com exclusão confirmada
        Formulario.jsx          ← criar e editar planos
      Catalogo/
        Listar.jsx              ← tabela com filtros por categoria/gênero/em alta
        Formulario.jsx          ← criar e editar conteúdos
      Assinantes/
        Listar.jsx              ← tabela com filtros por status/plano
        Formulario.jsx          ← criar e editar assinantes (com lista de planos)
        Perfis.jsx              ← gerenciar perfis de um assinante

    App.jsx                     ← rotas com React Router
    main.jsx
    index.css                   ← design system completo (tema dark)
```

## Como executar

### 1. Iniciar o Backend (ASP.NET)

```bash
cd Atividade/Atividade
dotnet run --launch-profile http
# API disponível em: http://localhost:5083
# Swagger: http://localhost:5083/swagger
```

### 2. Iniciar o Frontend (React)

```bash
cd frontend-streaming
npm install   # apenas na primeira vez
npm run dev
# Frontend disponível em: http://localhost:5173
```

> O CORS já está configurado no backend para aceitar requisições de `http://localhost:5173`.

## Funcionalidades

- ✅ Dashboard com estatísticas em tempo real (planos, conteúdos, assinantes, em alta)
- ✅ CRUD completo de **Planos** (criar, listar, editar, excluir)
- ✅ CRUD completo do **Catálogo** com filtros por categoria, gênero e destaque
- ✅ CRUD completo de **Assinantes** com filtros por status e plano
- ✅ Gerenciamento de **Perfis** por assinante (GET/POST/DELETE)
- ✅ Formulários com validação frontend e feedback de erros da API
- ✅ Confirmação antes de excluir
- ✅ Tratamento de erros de conexão
- ✅ Design responsivo com tema dark premium
