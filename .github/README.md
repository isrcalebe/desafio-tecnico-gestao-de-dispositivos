# API de Gestão de Dispositivos e Eventos

Esta é uma API RESTful desenvolvida em .NET 9 para gerenciar clientes, seus dispositivos e os eventos de telemetria recebidos. O projeto segue os princípios da Clean Architecture para garantir uma base de código organizada, testável e de fácil manutenção.

## Requisitos

- Uma plataforma desktop com [.NET 9.0 SDK](https://dotnet.microsoft.com/download) ou SDK mais recente instalado.
- Uma plataforma desktop com [Docker](https://www.docker.com/) instalado.
- Ao trabalhar no projeto, recomendo usar um IDE com intellisense e realce de sintaxe, como [Visual Studio 2022+](https://visualstudio.microsoft.com/vs/), [JetBrains Rider](https://www.jetbrains.com/rider/), ou [Visual Studio Code](https://code.visualstudio.com/) com as extensões [EditorConfig](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig) e [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) instaladas.

## Setup e Configuração

1. Clonar o Repositório
```bash
git clone https://github.com/isrcalebe/desafio-tecnico-gestao-de-dispositivos
cd desafio-tecnico-gestao-de-dispositivos
```

2. Execute o Docker Compose

- Certifique-se de que o Docker está em execução e execute o seguinte comando na raiz do projeto:

```bash
docker compose up -d
```

3. Aplicar as Migrations do Banco de Dados

- Use a ferramenta de linha de comando do EF Core para criar o banco de dados e as tabelas:

```bash
cd device-manager/source
dotnet ef database update --project infrastructure --startup-project webapi
```

4. Acesse a API:

- URL para Scalar: http://localhost:8080/scalar

## Executando os Testes

Para garantir que a lógica da aplicação está correta, execute os testes unitários e de integração a partir da raiz da solução:

```bash
# device-manager/device-manager.sln
dotnet test
```
