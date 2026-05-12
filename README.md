# Programa-o-Orientada-a-Objetos

## Passo a passo para testar (iniciante em C#)
### 1) Requisitos
- Windows com .NET SDK instalado (versao compatível com o projeto).
- VS Code ou Visual Studio (qualquer um serve).

### 2) Abrir o projeto no VS Code
1. Abra o VS Code.
2. Clique em **File > Open Folder**.
3. Selecione a pasta `Programa-Orientada-a-Objetos`.

### 3) Executar a API pelo terminal (VS Code)
1. Abra o terminal do VS Code (**Terminal > New Terminal**).
2. Entre na pasta do projeto:
	 - `Atividade/Atividade`
3. Rode o comando para iniciar a API:
	 - `dotnet run`
4. O terminal vai mostrar a URL, algo como:
	 - `https://localhost:xxxx` e `http://localhost:yyyy`

### 4) Acessar o Swagger
- Abra o navegador e acesse:
	- `https://localhost:xxxx/swagger`
- Use a porta exibida no terminal.
- Se aparecer aviso de certificado HTTPS, clique em **Avancar**.

### 5) Testar endpoints no Swagger
1. Clique em um endpoint, por exemplo **GET /api/catalogo**.
2. Clique em **Try it out**.
3. Clique em **Execute**.
4. Veja o **Response body** com o retorno.

### 6) Testar com Insomnia ou Postman (opcional)
- Base URL: `https://localhost:xxxx`
- Exemplo GET: `/api/planos`
- Exemplo POST: `/api/assinantes`

Exemplo de JSON para criar assinante:
{
	"nome": "Joao Silva",
	"email": "joao@email.com",
	"planoId": 1,
	"ativo": true
}

### Observacoes
- O banco SQLite e criado automaticamente ao iniciar a API.
- O arquivo do banco e `streaming.db` na pasta do projeto `Atividade/Atividade`.
- O primeiro inicio cria e preenche o banco com dados de exemplo.

### Endpoints principais
- GET /api/dashboard/resumo
- GET /api/catalogo
- GET /api/catalogo/em-alta
- GET /api/planos
- GET /api/assinantes