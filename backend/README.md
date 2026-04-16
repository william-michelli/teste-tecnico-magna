# Task Management Application

Aplicação de gestão de tarefas desenvolvida com .NET 9 (backend) e Vue.js (frontend), utilizando MySQL como banco de dados.

## Arquitetura

O projeto segue uma arquitetura em camadas:

- **API**: Controladores REST e configuração da aplicação
- **Application**: Serviços de negócio e DTOs
- **Domain**: Entidades e interfaces de repositório
- **Infrastructure**: Implementações de repositório e contexto do Entity Framework

## Tecnologias

- **Backend**: .NET 9 Web API, Entity Framework Core, MySQL
- **Frontend**: Vue.js 3, Vite
- **Banco de dados**: MySQL 8.0+

## Pré-requisitos

- .NET 9 SDK
- Node.js 18+
- MySQL Server 8.0+
- npm ou yarn

## Configuração

### Banco de Dados

1. Instale e configure o MySQL Server
2. Execute o script `Database/create_task_management_database.sql` para criar o banco e tabelas

### Backend

1. Navegue para a pasta raiz do projeto
2. Restaure as dependências: `dotnet restore`
3. Configure a string de conexão no `TaskManagement.API/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=TaskManagementDb;User=seu_usuario;Password=sua_senha;"
     }
   }
   ```
4. Execute as migrações (opcional, já que o script cria a tabela): `dotnet ef database update` (se usar EF migrations)

### Frontend

1. Navegue para a pasta `frontend`
2. Instale as dependências: `npm install`
3. Configure a URL da API no arquivo `.env` (já configurado para localhost:5000)

## Execução

### Backend

```bash
cd TaskManagement.API
dotnet run
```

A API estará disponível em: http://localhost:5000

### Frontend

```bash
cd frontend
npm run dev
```

O frontend estará disponível em: http://localhost:5173

## Funcionalidades

- ✅ Criar tarefa
- ✅ Editar tarefa
- ✅ Listar tarefas
- ✅ Filtrar por status
- ✅ Buscar por texto (título ou descrição)
- ✅ Excluir tarefa (com validação para tarefas concluídas)

## Regras de Negócio

- Uma tarefa não pode ser concluída sem título válido
- Tarefas concluídas não podem ser excluídas
- Status possíveis: Pendente, Em andamento, Concluído

## API Endpoints

- `GET /api/tasks` - Lista todas as tarefas (suporta filtros `status` e `search`)
- `GET /api/tasks/{id}` - Obtém tarefa por ID
- `POST /api/tasks` - Cria nova tarefa
- `PUT /api/tasks/{id}` - Atualiza tarefa
- `DELETE /api/tasks/{id}` - Exclui tarefa

## Estrutura do Projeto

```
MagnaTeste/
├── Database/
│   └── create_task_management_database.sql
├── TaskManagement.API/
│   ├── Controllers/
│   ├── Program.cs
│   └── appsettings.json
├── TaskManagement.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
├── TaskManagement.Domain/
│   ├── Entities/
│   └── Interfaces/
├── TaskManagement.Infrastructure/
│   ├── Data/
│   └── Repositories/
├── TaskManagement.Application.Tests/
│   └── TaskServiceTests.cs
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   ├── services/
│   │   └── App.vue
│   ├── Dockerfile
│   ├── nginx.conf
│   └── .env
├── docker-compose.yml
├── Dockerfile
└── README.md
```

## Desenvolvimento

### Adicionando Novos Recursos

1. Defina a entidade no `TaskManagement.Domain`
2. Crie interfaces no `TaskManagement.Domain.Interfaces`
3. Implemente serviços no `TaskManagement.Application`
4. Implemente repositórios no `TaskManagement.Infrastructure`
5. Adicione controladores no `TaskManagement.API`
6. Atualize o frontend conforme necessário

### Testes

Para executar os testes unitários:

```bash
dotnet test
```

Os testes cobrem:
- Validações de negócio
- Casos de erro
- Mapeamento de DTOs
- Regras específicas (exclusão de tarefas concluídas)

## Docker (Opcional)

Para executar a aplicação completa com Docker:

1. Instale Docker e Docker Compose
2. Execute: `docker-compose up --build`
3. Acesse:
   - Frontend: http://localhost:3000
   - API: http://localhost:8080
   - MySQL: localhost:3306

Para desenvolvimento, execute apenas o banco:
```bash
docker-compose up mysql
```

E execute API e frontend localmente conforme instruções acima.

## Licença

Este projeto é para fins educacionais e de avaliação técnica.

## Diferenciais Implementados

- ✅ **Separação de Camadas**: Arquitetura limpa com API, Application, Domain e Infrastructure
- ✅ **DTOs**: Uso de Data Transfer Objects para comunicação entre camadas
- ✅ **Testes Unitários**: 14 testes cobrindo validações e regras de negócio
- ✅ **Docker**: Containerização completa com docker-compose
- ✅ **Paginação**: Implementada na API (não demonstrada no frontend, mas disponível)
- ✅ **Validações Robusta**: Regras de negócio implementadas e testadas
- ✅ **Tratamento de Erros**: HTTP status codes apropriados e mensagens claras
- ✅ **CORS**: Configurado para desenvolvimento local
- ✅ **Entity Framework**: Mapeamento objeto-relacional com MySQL