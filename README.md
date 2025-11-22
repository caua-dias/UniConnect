# UniConnect — Rede Acadêmica Colaborativa

[![.NET 8](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![C# 12](https://img.shields.io/badge/C%23-12.0-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-9.0-blue)](https://docs.microsoft.com/en-us/ef/core/)
[![JWT](https://img.shields.io/badge/Auth-JWT-green)](https://jwt.io/)

## ?? Visão Geral

**UniConnect** é uma plataforma web moderna para conectar alunos e professores, criando comunidades temáticas, grupos de estudo e facilitando o compartilhamento de materiais educacionais. Desenvolvida com **ASP.NET Core 8**, utiliza arquitetura REST, autenticação JWT, persistência com Entity Framework Core e está totalmente documentada com Swagger.

### Objetivo
Criar um espaço colaborativo onde:
- ????? **Alunos** podem se conectar com professores e colegas
- ????? **Professores** podem organizar comunidades e compartilhar conteúdo
- ????? **Admins** gerenciam a plataforma
- ?? **Comunidades temáticas** funcionam como grupos de estudo
- ?? **Postagens** permitem compartilhar materiais e discussões

---

## ??? Arquitetura e Tecnologias

### Stack Tecnológico

| Tecnologia | Versão | Propósito |
|-----------|--------|----------|
| **.NET** | 8.0 | Runtime / Framework |
| **C#** | 12.0 | Linguagem de Programação |
| **ASP.NET Core** | 8.0 | Web API |
| **Entity Framework Core** | 9.0.11 | ORM / Persistência |
| **SQL Server** | LocalDB | Banco de Dados |
| **JWT Bearer** | 8.0 | Autenticação |
| **BCrypt.Net** | 4.0.2 | Hash de Senhas |
| **Swagger** | 6.6.2 | Documentação de API |

### Padrões de Projeto

- **API RESTful**: Endpoints seguem convenção REST (GET, POST, PUT, DELETE)
- **POO com Herança**: `Usuario` ? `Aluno`, `Professor`, `Admin` (Table-Per-Hierarchy)
- **DTO Pattern**: Transferência de dados entre cliente/servidor
- **Dependency Injection**: Configurado em `Program.cs`
- **Async/Await**: Operações I/O não-bloqueantes
- **LINQ**: Queries para ranking e filtragem

---

## ?? Estrutura do Projeto

```
UniConnect/
??? Controllers/
?   ??? AuthController.cs              # Autenticação (Login/Register)
?   ??? UsuarioController.cs           # CRUD de Usuários
?   ??? ComunidadeTematicaController.cs # CRUD de Comunidades + Ranking
?   ??? ParticipacaoComunidadeController.cs # Gerenciar participações
?   ??? PostagemController.cs          # CRUD de Postagens (Materiais)
??? Models/
?   ??? Usuario.cs                     # Entidade base (herança)
?   ??? Aluno.cs                       # Especialização: Aluno
?   ??? Professor.cs                   # Especialização: Professor
?   ??? Admin.cs                       # Especialização: Admin
?   ??? ComunidadeTematica.cs          # Grupo de estudo
?   ??? ParticipacaoComunidade.cs      # Membership na comunidade
?   ??? Postagem.cs                    # Conteúdo/Material
??? DTOs/
?   ??? Auth/
?   ?   ??? LoginRequestDTO.cs
?   ?   ??? LoginResponseDTO.cs
?   ?   ??? RegisterDTO.cs
?   ?   ??? RegisterResponseDTO.cs
?   ??? Usuario/
?   ?   ??? UsuarioCreateDTO.cs
?   ?   ??? UsuarioUpdateDTO.cs
?   ?   ??? UsuarioResponseDTO.cs
?   ?   ??? UsuarioSenhaUpdateDTO.cs
?   ??? Comunidade/
?   ?   ??? ComunidadeCreateDTO.cs
?   ?   ??? ComunidadeUpdateDTO.cs
?   ?   ??? ComunidadeResponseDTO.cs
?   ??? Postagem/
?   ?   ??? PostagemCreateDTO.cs
?   ?   ??? PostagemUpdateDTO.cs
?   ?   ??? PostagemResponseDTO.cs
?   ??? Participacao/
?       ??? ParticipacaoCreateDTO.cs
?       ??? ParticipacaoResponseDTO.cs
??? Services/
?   ??? TokenService.cs                # Geração de JWT
??? Data/
?   ??? DataContext.cs                 # DbContext (EF Core)
??? Enums/
?   ??? UsuarioEnum.cs                 # Tipo de usuário
?   ??? AdminEnum.cs                   # Cargo de admin
??? Program.cs                         # Configuração da API
??? appsettings.json                   # Configurações gerais
??? UniConnect.csproj                  # Projeto C#
??? CONTRIBUTING.md                    # Padrões de desenvolvimento
??? README.md                          # Este arquivo
```

---

## ?? Modelos de Dados

### Diagrama de Relacionamentos

```
Usuario (Base)
??? Aluno
?   ??? Curso: string
?   ??? Semestre: int
??? Professor
?   ??? Departamento: string
?   ??? Titulacao: string
??? Admin
    ??? Cargo: enum (Diretor, Coordenador, Secretario)

ComunidadeTematica
??? Nome: string
??? Descricao: string
??? UsuarioCriador (1:N)
??? Participacoes (1:N) ? ParticipacaoComunidade
??? Postagens (1:N) ? Postagem

ParticipacaoComunidade (Join Table com payload)
??? Usuario (N:1)
??? ComunidadeTematica (N:1)
??? Tipo: enum (Membro, Moderador)
??? DataEntrada: datetime

Postagem
??? Usuario (N:1) - Quem postou
??? ComunidadeTematica (N:1) - Em qual comunidade
??? Conteudo: string
??? ArquivoUrl: string (Material/Link)
??? DataPublicacao: datetime
??? DataAtualizacao: datetime?
```

---

## ?? Guia de Instalação e Configuração

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recomendado) ou [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) ou SQL Server Express (LocalDB)
- [Postman](https://www.postman.com/) ou [Insomnia](https://insomnia.rest/) (para testar endpoints)

### Passo 1: Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/UniConnect.git
cd UniConnect
```

### Passo 2: Restaurar Dependências

```bash
dotnet restore
```

### Passo 3: Configurar Segredos JWT (Development)

Use User Secrets para armazenar a chave JWT localmente (não comitar no git):

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "sua-chave-super-secreta-aqui-com-no-minimo-32-caracteres"
dotnet user-secrets set "Jwt:Issuer" "UniConnect"
dotnet user-secrets set "Jwt:Audience" "UniConnectUsers"
```

**Alternativa (variáveis de ambiente):**
```bash
set Jwt__Key=sua-chave-super-secreta-aqui-com-no-minimo-32-caracteres
set Jwt__Issuer=UniConnect
set Jwt__Audience=UniConnectUsers
```

**Para macOS/Linux:**
```bash
export Jwt__Key=sua-chave-super-secreta-aqui-com-no-minimo-32-caracteres
export Jwt__Issuer=UniConnect
export Jwt__Audience=UniConnectUsers
```

### Passo 4: Criar e Aplicar Migrations

```bash
# Criar migration inicial
dotnet ef migrations add InitialCreate

# Aplicar migration ao banco
dotnet ef database update
```

Se receber erro, verifique se a string de conexão em `appsettings.json` está correta:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=UniConnect_DB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### Passo 5: Executar a Aplicação

```bash
dotnet run
```

A API estará disponível em:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger**: `https://localhost:5001/swagger/index.html`

---

## ?? Autenticação e Segurança

### Fluxo de Autenticação

1. **Registro** (POST `/api/auth/register`)
   - Usuário se registra com email, senha e tipo (Aluno/Professor/Admin)
   - Senha é **hasheada com BCrypt** antes de ser armazenada
   - Retorna dados do usuário (sem senha)

2. **Login** (POST `/api/auth/login`)
   - Usuário faz login com email e senha
   - Sistema verifica credenciais com BCrypt
   - Gera **JWT Token** válido por 4 horas
   - Retorna token + dados do usuário

3. **Acesso a Endpoints Protegidos**
   - Cliente envia token no header: `Authorization: Bearer <token>`
   - API valida token (issuer, audience, assinatura, expiração)
   - Se válido, requisição é processada; caso contrário, retorna **401 Unauthorized**

### Exemplo de Uso do JWT no Swagger

1. Faça login (POST `/api/auth/login`)
2. Copie o `token` retornado
3. Clique no botão ?? **"Authorize"** no Swagger
4. Cole: `Bearer {seu_token}`
5. Agora todos os endpoints protegidos estarão acessíveis

### Políticas de Segurança

- ? Senhas **nunca** armazenadas em texto puro (BCrypt PBKDF2)
- ? JWT com assinatura **HMAC SHA-256**
- ? Validação de **issuer, audience e expiração**
- ? Endpoints sensíveis protegidos com `[Authorize]`
- ? Campos sensíveis nunca expostos em DTOs
- ?? **TODO**: Implementar rate-limiting
- ?? **TODO**: HTTPS obrigatório em produção

---

## ?? Endpoints da API

### Autenticação (Públicos)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/auth/register` | Registrar novo usuário | ? |
| POST | `/api/auth/login` | Login e obter JWT | ? |

### Usuários (Protegidos)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/usuario` | Listar todos os usuários | ? |
| GET | `/api/usuario/{id}` | Obter usuário por ID | ? |
| POST | `/api/usuario` | Criar novo usuário | ? |
| PUT | `/api/usuario/{id}` | Atualizar dados do usuário | ? |
| PUT | `/api/usuario/alterar-senha/{id}` | Alterar senha | ? |
| DELETE | `/api/usuario/{id}` | Deletar usuário | ? |

### Comunidades (Mistas)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/comunidadetematica` | Listar todas as comunidades | ? |
| GET | `/api/comunidadetematica/top` | Top 5 comunidades (ranking por membros/posts) | ? |
| GET | `/api/comunidadetematica/{id}` | Obter comunidade por ID | ? |
| POST | `/api/comunidadetematica` | Criar nova comunidade | ? |
| PUT | `/api/comunidadetematica/{id}` | Atualizar comunidade | ? |
| DELETE | `/api/comunidadetematica/{id}` | Deletar comunidade | ? |

### Participações

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/participacaocomunidade` | Listar todas as participações | ? |
| GET | `/api/participacaocomunidade/{id}` | Obter participação por ID | ? |
| POST | `/api/participacaocomunidade` | Entrar em uma comunidade | ? |
| DELETE | `/api/participacaocomunidade/{id}` | Sair de uma comunidade | ? |

### Postagens (Mistas)

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/postagem` | Listar todas as postagens | ? |
| GET | `/api/postagem/{id}` | Obter postagem por ID | ? |
| POST | `/api/postagem` | Criar nova postagem/material | ? |
| PUT | `/api/postagem/{id}` | Atualizar postagem | ? |
| DELETE | `/api/postagem/{id}` | Deletar postagem | ? |

---

## ?? Exemplos de Uso

### 1. Registrar um Novo Usuário (Aluno)

**Request:**
```bash
POST /api/auth/register
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@example.com",
  "senha": "Senha123!@#",
  "tipo": 0,
  "curso": "Engenharia de Software",
  "semestre": 3
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@example.com",
  "tipo": 0,
  "dataCriacao": "2024-01-15T10:30:00Z"
}
```

### 2. Fazer Login e Obter Token

**Request:**
```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "joao@example.com",
  "senha": "Senha123!@#"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuarioId": 1,
  "nome": "João Silva",
  "email": "joao@example.com"
}
```

### 3. Criar uma Comunidade Temática

**Request:**
```bash
POST /api/comunidadetematica
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "nome": "Desenvolvimento Web",
  "descricao": "Comunidade para discutir técnicas modernas de web",
  "usuarioId": 1
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "nome": "Desenvolvimento Web",
  "descricao": "Comunidade para discutir técnicas modernas de web",
  "usuarioId": 1,
  "dataCriacao": "2024-01-15T10:35:00Z",
  "dataAtualizacao": null
}
```

### 4. Listar Top 5 Comunidades (Ranking)

**Request:**
```bash
GET /api/comunidadetematica/top?take=5
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "nome": "Desenvolvimento Web",
    "members": 45,
    "posts": 128
  },
  {
    "id": 2,
    "nome": "Banco de Dados",
    "members": 32,
    "posts": 87
  }
]
```

### 5. Participar de uma Comunidade

**Request:**
```bash
POST /api/participacaocomunidade
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "usuarioId": 1,
  "comunidadeTematicaId": 1,
  "tipo": 0
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "usuarioId": 1,
  "comunidadeTematicaId": 1,
  "tipo": 0,
  "dataEntrada": "2024-01-15T10:40:00Z"
}
```

### 6. Compartilhar um Material (Postagem)

**Request:**
```bash
POST /api/postagem
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "conteudo": "Ótimo material sobre ASP.NET Core!",
  "arquivoUrl": "https://example.com/material.pdf",
  "usuarioId": 1,
  "comunidadeTematicaId": 1
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "conteudo": "Ótimo material sobre ASP.NET Core!",
  "arquivoUrl": "https://example.com/material.pdf",
  "usuarioId": 1,
  "comunidadeTematicaId": 1,
  "dataPublicacao": "2024-01-15T10:45:00Z",
  "dataAtualizacao": null
}
```

---

## ?? Testes e Validação

### Testar com cURL

```bash
# Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"joao@example.com","senha":"Senha123!@#"}'

# Obter token (copie do response anterior)
export TOKEN="seu_token_aqui"

# Criar comunidade
curl -X POST http://localhost:5000/api/comunidadetematica \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"nome":"Dev","descricao":"Dev","usuarioId":1}'
```

### Testar com Postman

1. Abra [Postman](https://www.postman.com/downloads/)
2. Crie uma Collection "UniConnect"
3. Configure variáveis de ambiente:
   - `base_url`: `http://localhost:5000`
   - `token`: (deixe em branco, será preenchido após login)
4. Importe os endpoints usando a coleção (ou crie manualmente)
5. No ambiente, use: `{{base_url}}/api/...`

### Usar Swagger UI

Acesse `https://localhost:5001/swagger/index.html` no navegador e teste todos os endpoints interativamente.

---

## ?? Fluxo Completo de Uso

### Cenário: Aluno cria comunidade, professor participa, aluno compartilha material

```
1. ALUNO se registra
   POST /api/auth/register (Aluno, Curso: Eng. Software)
   
2. ALUNO faz login
   POST /api/auth/login ? Recebe JWT Token
   
3. ALUNO cria comunidade
   POST /api/comunidadetematica (Desenvolvimento Web)
   
4. ALUNO participa da comunidade
   POST /api/participacaocomunidade (UsuarioId=1, ComunidadeId=1)
   
5. PROFESSOR se registra e faz login
   POST /api/auth/register (Professor, Departamento: Eng.)
   POST /api/auth/login ? JWT Token
   
6. PROFESSOR participa da comunidade
   POST /api/participacaocomunidade (UsuarioId=2, ComunidadeId=1)
   
7. ALUNO compartilha material
   POST /api/postagem (ArquivoUrl: link_do_pdf, ComunidadeId=1)
   
8. PROFESSOR consulta top comunidades
   GET /api/comunidadetematica/top ? Vê ranking
   
9. PROFESSOR consulta postagens
   GET /api/postagem ? Vê material compartilhado
```

---

## ?? Configurações Importantes

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=UniConnect_DB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Key": "sua-chave-super-secreta-aqui-com-no-minimo-32-caracteres",
    "Issuer": "UniConnect",
    "Audience": "UniConnectUsers"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Variáveis de Ambiente (Produção)

```bash
# Banco de dados
ConnectionStrings__DefaultConnection="Server=seu-servidor;Database=UniConnect;User Id=sa;Password=sua-senha;"

# JWT (NUNCA em texto puro no git)
Jwt__Key="sua-chave-super-secreta-muito-longa"
Jwt__Issuer="UniConnect"
Jwt__Audience="UniConnectUsers"

# ASP.NET Core
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=https://+:443
```

---

## ?? Troubleshooting

### Erro 401 (Unauthorized) ao tentar acessar endpoint protegido

**Solução:**
- Verifique se você enviou o token no header: `Authorization: Bearer <token>`
- Confirme se o token não expirou (válido por 4 horas)
- Verifique se as configurações de JWT estão corretas em `appsettings.json`

### Erro ao criar migration

**Solução:**
```bash
# Verificar se EntityFrameworkCore Tools está instalado
dotnet tool install --global dotnet-ef

# Remover migration anterior se houver erro
dotnet ef migrations remove

# Tentar novamente
dotnet ef migrations add InitialCreate
```

### Erro de conexão com banco de dados

**Solução:**
- Verifique se SQL Server/LocalDB está rodando
- Confirme string de conexão em `appsettings.json`
- Teste: `sqlcmd -S (localdb)\mssqllocaldb`

### Token inválido ou expirado

**Solução:**
- Faça novo login para obter novo token
- Verifique se `Jwt:Key` é a mesma em geração e validação
- Verifique se `Jwt:Issuer` e `Jwt:Audience` conferem

---

## ?? Padrões de Código

Este projeto segue padrões definidos em `CONTRIBUTING.md`:

- ? Nullable: `enable`
- ? NamingConvention: PascalCase (classes), camelCase (variáveis)
- ? Async/Await para operações I/O
- ? LINQ para queries
- ? Validações de entrada nos DTOs
- ? Tratamento de exceções em controllers

---

## ?? Deploy em Produção

### Azure App Service

```bash
# Publicar perfil de release
dotnet publish -c Release -o ./publish

# Fazer upload para Azure
az webapp deployment source config-zip --resource-group seu-grupo --name seu-app --src-path publish.zip
```

### Docker

```bash
# Criar imagem
docker build -t uniconnect:latest .

# Rodar container
docker run -p 5000:80 -e "Jwt__Key=sua-chave" uniconnect:latest
```

---

## ?? Recursos e Documentação

- [Microsoft Learn - ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)
- [JWT.io](https://jwt.io/)
- [Swagger/OpenAPI](https://swagger.io/)
- [BCrypt.Net-Next](https://github.com/BcryptNet/bcrypt.net-next)

---

## ?? Contribuindo

Leia `CONTRIBUTING.md` para:
- Padrões de código
- Fluxo de pull requests
- Convenções de commits
- Políticas de segurança

---

## ?? Licença

Este projeto está disponível sob a licença MIT. Veja `LICENSE` para detalhes.

---

## ?? Contato e Suporte

- **Issues**: Abra uma issue no repositório para bugs e sugestões
- **Discussões**: Use a seção Discussions para dúvidas gerais
- **Email**: seu-email@example.com

---

## ?? Roadmap Futuro

- [ ] Upload de arquivos (não apenas URLs)
- [ ] Notificações em tempo real (WebSockets)
- [ ] Avaliação de postagens (likes, comentários)
- [ ] Busca e filtros avançados
- [ ] Rate-limiting e proteção contra abuse
- [ ] Dashboard de admin
- [ ] Mobile App (React Native)
- [ ] Testes unitários e integração (xUnit)
- [ ] CI/CD pipeline (GitHub Actions)
- [ ] Documentação OpenAPI aprimorada

---

**Desenvolvido com ?? usando .NET 8 e ASP.NET Core**

Última atualização: Janeiro 2024
