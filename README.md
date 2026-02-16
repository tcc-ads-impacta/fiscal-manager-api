# Fiscal Manager API

Backend da aplicação de Controle Doméstico de Notas Fiscais. Este projeto foi desenvolvido como MVP para entrega acadêmica, focando em performance, arquitetura limpa e pragmatismo na persistência de dados.

## Visão Geral Técnica

A solução segue os princípios da **Clean Architecture**, isolando o domínio da aplicação de detalhes de infraestrutura e apresentação. O objetivo é garantir manutenibilidade e testabilidade, mesmo em um escopo de MVP.

## Tech Stack

* **Runtime:** .NET 9
* **API Framework:** ASP.NET Core Web API (Minimal APIs)
* **Database:** MySQL 8.0 (via Docker Container)
* **ORM:** Entity Framework Core 9 (Code-First)
* **Documentation:** Scalar API Reference / OpenAPI
* **Infrastructure:** Docker Compose


## Arquitetura da Solução

A solução `FiscalManager.sln` está dividida em 4 camadas lógicas:

1.  **FiscalManager.Domain:** (Core) Contém as entidades (`Invoice`) e regras de negócio puras. Zero dependências externas.
2.  **FiscalManager.Application:** (Use Cases) Contém DTOs, Interfaces de Serviço e Lógica de Aplicação. Depende apenas de Domain.
3.  **FiscalManager.Infrastructure:** (Adapters) Implementação de acesso a dados (`FiscalDbContext`), Migrations e serviços de I/O (File System). Depende de Application.
4.  **FiscalManager.Api:** (Entry Point) Configuração de Injeção de Dependência (DI), Controllers e Middleware.


## Guia de Execução (Getting Started)

Siga os passos abaixo para rodar a aplicação em ambiente local.

### Pré-requisitos

* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop) (ou Docker Engine + Compose)
* Ferramenta de CLI do EF Core (Opcional, mas recomendada):

```bash
dotnet tool install --global dotnet-ef
```

### 1. Clonar o Repositório

```bash
git clone https://github.com/tcc-ads-impacta/fiscal-manager-api.git
cd fiscal-manager-api
```

### 2. Subir a Infraestrutura (Banco de Dados)

Utilizei Docker Compose para orquestrar o MySQL e o Adminer. Na raiz da solução, execute:

```bash
docker-compose up -d
```

*Aguarde cerca de 30 segundos para que o MySQL esteja pronto para conexões*

### 3. Aplicar Migrations (Criação das Tabelas)

Para garantir que o banco de dados tenha a estrutura correta (Tabela `Invoices`), execute o comando de atualização a partir da raiz da solução:

```bash
dotnet ef database update --project FiscalManager.Infrastructure --startup-project FiscalManager.Api
```

### 4. Executar a API

Rode o projeto principal:

```bash
dotnet run --project FiscalManager.Api
```

*A aplicação estará disponível em `http://localhost:7XXX` (a porta será exibida no terminal).*

### 5. Gerenciamento do Banco de Dados

O ambiente Docker inclui o **Adminer**, uma interface web leve para gestão do MySQL.

- **URL:** `http://localhost:8080`
- **Sistema:** MySQL
- **Servidor:** `mysql-db` (conforme docker-compose)
- **Usuário:** `fiscal_user`
- **Senha:** `fiscal_password`
- **Database:** `fiscal_db`

#### Connection Strings

A configuração de conexão está localizada em `FiscalManager.Api/appsettings.json`.

- **DefaultConnection:** `Server=localhost;Port=3306;Database=fiscal_db;Uid=fiscal_user;Pwd=fiscal_password;Charset=utf8mb4`
