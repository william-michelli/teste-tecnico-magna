## Obrgatórios ter instalado

 - Node.js
 - .NET
 - MySQL
 - Docker (Se for rodar com ele)

## Execução

### Banco de Dados
O script de criação de banco está na pasta backend/database

### Backend

```bash
cd TaskManagement.API
dotnet run
```

A API estará disponível em: https://localhost:44309

### Frontend

```bash
cd frontend
npm run dev
```

O frontend estará disponível em: http://localhost:5173




### Para executar os testes unitários:

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