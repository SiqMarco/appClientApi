# ClientAPI Project

## Descrição

O ClientAPI é uma API para gerenciamento de clientes, desenvolvida com C#, Entity Framework Core e React no frontend. O projeto utiliza um banco de dados MySQL e inclui scripts para criar e gerenciar a base de dados. Ele foi projetado para ser escalável e fácil de manter, permitindo uma integração eficiente entre o backend e o frontend. \
Para utilizar o frontend e realizar as requisições HTTP, utilize o projeto [ClientApp](https://github.com/SiqMarco/project-ClientApp).

## Tecnologias Utilizadas

- **C#**: Linguagem de programação principal para a API.
- **Entity Framework Core**: ORM utilizado para interagir com o banco de dados.
- **React**: Biblioteca JavaScript para construção da interface do usuário.
- **MySQL**: Sistema de gerenciamento de banco de dados utilizado para armazenar informações dos clientes.
- **Docker**: Plataforma para facilitar a criação e gestão de contêineres.

## Estrutura do Projeto

A estrutura do projeto é organizada da seguinte forma:

- **ClientAPI.API**: Contém os controladores da API.
- **ClientAPI.Domain**: Contém as entidades de domínio e modelos de dados.
- **ClientAPI.Infrastructure**: Contém a implementação do repositório e a configuração do banco de dados.
- **ClientAPI.Tests**: Contém os testes unitários para o projeto.

## Configuração do Ambiente

### Pré-requisitos

Antes de iniciar, você precisará dos seguintes softwares instalados:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js e npm](https://nodejs.org/)
- [Docker](https://www.docker.com/get-started)
- [MySQL](https://www.mysql.com/)

### Passos para Configuração

1. **Clone o repositório:**

    ```sh
    git clone https://github.com/SiqMarco/project-ClientApi
    ```

2. **Configure o banco de dados MySQL:**
    - Certifique-se de que o MySQL está em execução e acessível.
    - Execute o script SQL para criar a tabela de clientes:

    ```sh
    mysql -u <usuario> -p < Sql/script.sql
    ```

3. **Configure o ambiente Docker:**
    - Construa e inicie os contêineres Docker:
    - Dentro da pasta raiz do projeto ClientAPI.API, execute o comando:
    ```sh
    docker-compose up --build
    ```

4. **Aplique as migrações do Entity Framework:**
    - Atualize o banco de dados com as migrações:

    ```sh
    dotnet ef database update --project ClientAPI.Infrastructure
    ```

5. **Inicie a aplicação:**
   - Dentro da pasta raiz do projeto ClientAPI.API, execute o comando:

    ```sh
    dotnet run --project ClientAPI.API.csproj
    ```

## Exemplos de Requests
Cria novo cliente
```
curl --location 'http://localhost:5000/api/Clients' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '    {
        "name": "Logitec-12 S/A",
        "size": "grande"
    }'
```

Busca Cliente pelo ID
```
curl --location 'http://localhost:5000/api/Clients/5d830ff8-25d7-457f-a0b6-394aca8e86dd' \
--header 'accept: */*'
```

Busca todos os clientes
```
curl --location 'http://localhost:5000/api/Clients' \
--header 'accept: */*'
```

## Executando os Testes

Para executar os testes unitários, use o comando:

```sh
dotnet test
