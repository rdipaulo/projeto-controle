# Aplicação de Controle de Apostas Esportivas

Este projeto consiste em uma aplicação de controle de apostas esportivas, dividida em frontend (React) e backend (ASP.NET Core).

## Configurações e Execução Local

Para executar este projeto em sua máquina local, siga os passos abaixo:

### 1. Pré-requisitos

Certifique-se de ter os seguintes softwares instalados:

*   [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [Node.js e npm](https://nodejs.org/en/download/)
*   [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (ou SQL Server Express/Docker)
*   [Git](https://git-scm.com/downloads)

### 2. Configuração do Banco de Dados (SQL Server)

O backend está configurado para usar SQL Server. A string de conexão está definida no arquivo `appsettings.json` do projeto `BettingControl.API`.

**Caminho do arquivo:** `backend/BettingControl.API/appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=BettingControlDb;User Id=sa;Password=D@viHenri2411;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "9f1bce4b3c772e2b791d261a07b81e65f88835a0e9c6bbc4968f5f3edaa5ade1",
    "Issuer": "BettingControlAPI",
    "Audience": "BettingControlAPIUsers"
  }
}
```

**Observação:** A configuração `Server=localhost,1433` assume que o SQL Server está rodando na sua máquina local na porta padrão `1433`. Se o seu SQL Server estiver em um contêiner Docker, certifique-se de que a porta `1433` esteja mapeada corretamente e que você possa acessá-lo via `localhost` ou o endereço IP apropriado.

### 3. Aplicação das Migrações do Entity Framework Core

Para criar o banco de dados e as tabelas, você precisará aplicar as migrações:

1.  Navegue até o diretório do backend:
    ```bash
    cd final_project/backend/BettingControl.API
    ```
2.  Instale as ferramentas do Entity Framework Core CLI globalmente (se ainda não tiver):
    ```bash
    dotnet tool install --global dotnet-ef
    ```
3.  Adicione o diretório das ferramentas .NET ao PATH para a sessão atual (se necessário):
    ```bash
    export PATH="$PATH:/home/ubuntu/.dotnet/tools"
    ```
4.  Aplique as migrações para criar o banco de dados e as tabelas:
    ```bash
    dotnet ef database update
    ```

### 4. Execução do Backend

1.  No diretório `final_project/backend/BettingControl.API`:
    ```bash
    dotnet run --urls "http://localhost:5050"
    ```
    O backend será iniciado e estará acessível em `http://localhost:5050`.

### 5. Execução do Frontend

1.  Navegue até o diretório do frontend:
    ```bash
    cd final_project/frontend
    ```
2.  Instale as dependências do Node.js:
    ```bash
    npm install
    ```
3.  Inicie o servidor de desenvolvimento do frontend:
    ```bash
    npm run dev
    ```
    O frontend será iniciado e estará acessível em `http://localhost:5173`.

### 6. Acessando a Aplicação

Abra seu navegador e acesse `http://localhost:5173`.

Você poderá registrar um novo usuário, fazer login e começar a utilizar a aplicação. As funcionalidades de criação de ciclo e visualização de estatísticas já estão implementadas.
