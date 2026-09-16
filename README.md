# Bilheteria de Cinema — API

CP4 de Advanced Business Development with .NET (FIAP — Tecnologia em Analise e Desenvolvimento de Sistemas)

**Aluno:** Gabriel Cabral Mendes Mariano — RM 563230

## Descricao do projeto

API RESTful para uma bilheteria de cinema. O sistema permite cadastrar filmes, programar sessoes de exibicao,
manter um cardapio de snack bar (comidas e bebidas) e realizar a compra de ingressos com **assento marcado**,
opcionalmente acompanhada de itens do snack bar, tudo em uma unica transacao de checkout.

A regra de negocio central do sistema e impedir que o mesmo assento (fileira + numero) seja vendido duas vezes
para a mesma sessao, validar a capacidade da sala e o estoque dos produtos, e calcular o valor total do pedido
automaticamente no momento da compra.

## Arquitetura

O projeto segue Clean Architecture em camadas, organizadas em pastas dentro do projeto `Bilheteria.API`:

- **Domain** — entidades (`FilmeEntity`, `SessaoEntity`, `ProdutoEntity`, `PedidoEntity`, `ItemPedidoEntity`,
  `IngressoEntity`), interfaces de repositorio e excecoes de negocio (`AssentoIndisponivelException`,
  `CapacidadeExcedidaException`, `EstoqueInsuficienteException`, `RegistroNaoEncontradoException`).
- **Application** — DTOs de request/response, mappers (extension methods) e UseCases com as regras de negocio.
- **Infrastructure** — `ApplicationContext` (EF Core + Oracle), repositorios, health check customizado e a
  extensao `AddInfrastructure` que registra toda a injecao de dependencia.
- **Presentation** — controllers REST (`FilmeController`, `SessaoController`, `ProdutoController`, `PedidoController`).

### Modelo de dados (Oracle)

| Tabela | Relacionamento |
|---|---|
| `tb_filme` | 1 filme → N sessoes |
| `tb_sessao` | 1 sessao → N pedidos, N ingressos |
| `tb_produto` | 1 produto → N itens de pedido |
| `tb_pedido` | 1 pedido → N itens, N ingressos |
| `tb_item_pedido` | liga pedido + produto (snack bar) |
| `tb_ingresso` | liga pedido + sessao, com fileira/assento — **indice unico** em `(SessaoId, Fileira, NumeroAssento)` |

### Componentes e boas praticas aplicadas

- **Repository Pattern** + DTOs com mapeamento manual (sem AutoMapper).
- **Paginacao** (`deslocamento`, `registroRetornado`) com total de registros no retorno, nas listagens de todas as entidades.
- **Indices de banco** simples e compostos (ver `[Index]` nas entidades), incluindo o indice unico que impede
  a venda duplicada de assento diretamente no banco.
- **Compressao de resposta** (Brotli/Gzip).
- **Rate Limiting**: uma politica global (30 requisicoes / 10s por IP) e uma politica mais restrita,
  `politica_checkout` (3 requisicoes / 30s), aplicada especificamente no `POST /api/pedido` — o rate limiter
  fica desativado no ambiente `Testing` para nao interferir nos testes automatizados.
- **Swagger avancado**: `Swashbuckle.AspNetCore.Annotations` (descricoes e codigos de status por endpoint) e
  `.Filters` (exemplos de request/response em `Doc/Samples`).
- **Health Check** customizado em `/health`, testando a conexao com o Oracle, com resposta em JSON.
- **Logging estruturado** (`ILogger`) no fluxo de checkout — sucesso e cada motivo de recusa.
- **Application Insights** (tracing/metricas) via `AddApplicationInsightsTelemetry()`.

## Configuracao necessaria

Edite `Bilheteria.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "Oracle": "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))) (CONNECT_DATA=(SERVER=DEDICATED)(SID=ORCL)));User Id=SEU_RM;Password=SUA_SENHA;"
  },
  "ApplicationInsights": {
    "ConnectionString": "SUA_CONNECTION_STRING_DO_APP_INSIGHTS"
  }
}
```

O campo do Application Insights pode ficar em branco para rodar localmente — a telemetria simplesmente nao e enviada.

## Como rodar o projeto

```bash
# Restaurar pacotes e compilar
dotnet restore
dotnet build

# Criar a primeira migration (gera o script de criacao das tabelas no Oracle)
cd Bilheteria.API
dotnet ef migrations add InitialCreate
dotnet ef database update

# Rodar a API
dotnet run
```

O Swagger fica disponivel em `https://localhost:{porta}/swagger` (ambiente Development).

## Como rodar os testes

```bash
dotnet test
```

O projeto `Bilheteria.Tests` cobre:
- **Testes de unidade** dos UseCases (com Moq), incluindo todas as regras de negocio do checkout
  (assento duplicado, capacidade excedida, estoque insuficiente).
- **Testes de repositorio** com EF Core InMemory.
- **Testes funcionais/integracao** com `WebApplicationFactory`, exercitando o ciclo completo de requisicao
  HTTP dos endpoints, incluindo o retorno `409 Conflict` ao tentar vender o mesmo assento duas vezes.

## Endpoints principais

| Metodo | Rota | Descricao |
|---|---|---|
| GET | `/api/filme?deslocamento=0&registroRetornado=10` | Lista filmes (paginado) |
| GET | `/api/filme/em-cartaz` | Lista apenas filmes em cartaz |
| POST | `/api/filme` | Cadastra um filme |
| GET | `/api/sessao?deslocamento=0&registroRetornado=10` | Lista sessoes com assentos disponiveis |
| POST | `/api/sessao` | Cadastra uma sessao |
| GET | `/api/produto?deslocamento=0&registroRetornado=10` | Lista produtos do snack bar |
| POST | `/api/produto` | Cadastra um produto |
| POST | `/api/pedido` | **Checkout**: compra ingressos (com assento) e itens do snack bar |
| PUT | `/api/pedido/{id}/cancelar` | Cancela um pedido |
| GET | `/health` | Health check da API e do banco |

### Exemplo — checkout (`POST /api/pedido`)

```json
{
  "sessaoId": 1,
  "nomeCliente": "Gabriel Mariano",
  "ingressos": [
    { "fileira": "D", "numeroAssento": 12 },
    { "fileira": "D", "numeroAssento": 13 }
  ],
  "itens": [
    { "produtoId": 1, "quantidade": 1 },
    { "produtoId": 2, "quantidade": 2 }
  ]
}
```

Repetir a compra do mesmo assento (`fileira` + `numeroAssento`) para a mesma sessao retorna `409 Conflict`.
Exceder a capacidade da sala ou o estoque de um produto retorna `422 Unprocessable Entity`.
