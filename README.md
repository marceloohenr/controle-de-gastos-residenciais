# Controle de Gastos Residenciais

Aplicação full stack para registrar moradores, receitas e despesas e acompanhar o saldo individual e consolidado da residência.

## Autor

Marcelo Henrique de Albuquerque Bezerra Malagueta

## Tecnologias

- ASP.NET Core 8 Web API, C# e Entity Framework Core
- SQLite com exclusão em cascata
- React 19, TypeScript, Vite e CSS responsivo
- xUnit, EF Core InMemory e Vitest

## Como executar

Pré-requisitos: .NET SDK 8 e Node.js 20 ou superior.

```bash
cd backend
dotnet restore
dotnet run --project src/HouseholdExpenses.Api
```

A API inicia em `http://localhost:5080` e o Swagger em `/swagger`. Em outro terminal:

```bash
cd frontend
npm install
npm run dev
```

O frontend estará em `http://localhost:5173`. Para usar outro endereço da API, copie `.env.example` para `.env` e altere `VITE_API_URL`. O arquivo SQLite é criado automaticamente na primeira execução e permanece entre reinicializações.

## Estrutura e decisões

O backend separa controllers, serviços, repositórios, contratos e persistência. Controllers tratam apenas HTTP; serviços concentram regras; repositórios isolam operações de dados. Exceções de domínio são convertidas centralmente em `ProblemDetails`. O relacionamento obrigatório entre pessoa e transação usa cascade no banco, evitando exclusões manuais.

As entidades, contratos e principais abstrações possuem documentação XML sobre suas responsabilidades. Comentários internos foram reservados para decisões que não são evidentes pela leitura das instruções: a localização da regra de menores no domínio, a escolha de iniciar o resumo pelas pessoas e a responsabilidade do banco na exclusão em cascata. As demais operações usam nomes e métodos pequenos para evitar comentários que apenas repetiriam o código.

O frontend é organizado por páginas, componentes, hooks, contexto de notificações, serviços e tipos. A comunicação HTTP fica centralizada e todas as telas contemplam carregamento, erro e ausência de dados.

## Endpoints

| Método | Rota | Finalidade |
|---|---|---|
| GET | `/api/people?search=` | Listar e buscar pessoas |
| POST | `/api/people` | Cadastrar pessoa |
| DELETE | `/api/people/{id}` | Excluir pessoa e suas transações |
| GET | `/api/transactions` | Listar transações |
| POST | `/api/transactions` | Registrar transação |
| GET | `/api/summary` | Obter totais individuais e gerais |

O tipo da transação é enviado como `Income` (receita) ou `Expense` (despesa).

## Regras de negócio

- Nome, idade, descrição, valor positivo, tipo e pessoa são obrigatórios. A idade aceita valores entre 0 e 130 e é anulável apenas no contrato de entrada, permitindo distinguir o valor zero de um campo não enviado.
- Pessoas menores de 18 anos podem registrar somente despesas. A API valida a regra no serviço, mesmo que a interface também antecipe o bloqueio.
- A exclusão de uma pessoa remove suas transações por integridade referencial.
- Saldo é a diferença entre receitas e despesas.

## Testes e qualidade

```bash
cd backend && dotnet test
cd frontend && npm run lint && npm run test && npm run build
```

Os testes de backend cobrem a restrição de menores, referências inexistentes, idade obrigatória, cálculos do resumo e a exclusão em cascata com SQLite. A solução foi validada em Release com .NET SDK 8.0.422: compilação sem erros ou avisos e todos os testes aprovados.

## Melhorias futuras

Paginação para grandes históricos, autenticação por residência, exportação de relatórios, orçamentos mensais e testes ponta a ponta com Playwright.
