# NFE.Solution (.NET8)

## Stack

- **.NET 8** - Web API
- **PostgreSQL** - Banco de dados
- **RabbitMQ** - Mensageria assíncrona
- **Entity Framework Core** - ORM
- **MediatR** - CQRS Pattern
- **Docker** - Containerização

## Arquitetura

- **Clean Architecture** - Separação de responsabilidades em camadas
- **CQRS Pattern** - Command Query Responsibility Segregation com MediatR
- **Repository Pattern** - Abstração de acesso a dados
- **Domain-Driven Design** - Entidades e Value Objects
- **Background Services** - Processamento assíncrono com RabbitMQ


## Como Executar

### 1. Clonar o repositório
```bash
git clone https://github.com/RubTheKid/Nfe.Solution.git
cd Nfe.Solution
```

### 2. Subir o banco de dados
```bash
docker-compose up -d postgres rabbitmq
```

### 3. Restaurar dependências
```bash
dotnet restore
```

### 4. Executar migrações
```bash
dotnet ef database update --project Nfe.Infrastructure --startup-project Nfe.Api
```

### 5. Executar aplicação
```bash
dotnet run --project Nfe.Api
```

### 6. Acessar a api
- **Swagger UI**: https://localhost:7139/swagger
- **API Base URL**: https://localhost:7139/api


## Endpoints

### Consultar NF-e
- `GET /api/nfe/{id}` - Consultar NFe por ID

### Criar NF-E
- `POST /api/nfe/send-to-authorization` - Criar e enviar NFe para autorização

### Obter XML
- `GET /api/nfe/{id}/xml` - Obter XML da NFe autorizada

### Clientes
- `GET /api/clientes` - Listar clientes
- `GET /api/clientes/{id}` - Obter cliente por ID
- `POST /api/clientes` - Criar cliente
- `PUT /api/clientes/{id}` - Atualizar cliente


### Clientes
- `GET /api/clientes` - Listar clientes
- `GET /api/clientes/{id}` - Obter cliente por ID
- `POST /api/clientes` - Criar cliente
- `PUT /api/clientes/{id}` - Atualizar cliente

## Fluxo de Processamento

1. Criação - NFe criada via API
2. Fila - Enviada para processamento assíncrono (RabbitMQ)
3. Simulação - SEFAZ simulada processa automaticamente
4. Resultado - Status atualizado (Autorizada/Rejeitada)

## Estrutura do projeto
```
	Nfe.Solution/
	├── Nfe.Api/ # Controllers e configuração da API
	├── Nfe.Application/ # Handlers CQRS, Services e Features
	├── Nfe.Domain/ # Entidades, Value Objects e Contratos
	├── Nfe.Infrastructure/ # Repositories, DbContext e Migrations
	└── Nfe.Tests/ # Testes unitários
```

## Exemplos JSON

Exemplos de requisições disponíveis em `/ExemplosJson/`:
- `nfe-basico.json` - NFe com um item
- `nfe-multiplos-itens.json` - NFe com múltiplos itens
- `nfe-diferentes-tributacoes.json` - NFe com diferentes CSTs

## Modelo de Dados

### Entidades

- NotaFiscal
- Cleinte
- Produto
- ItemNfe

### Value Objects
- CNPJ
- Endereço
- Email
- Telefone
- StatusNFe