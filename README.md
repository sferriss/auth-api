## Api de autenticação

Esse projeto foi construido usando .Net 7, Podman/Docker e PostgresSql

### Instalação
1. Clonar repositório
```bash
git clone https://github.com/sferriss/auth-api.git
```
2. Instalar [docker](https://www.docker.com/products/docker-desktop/)

3. Criar container do banco
```bash
docker run --name db -p 5432:5432 -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -d postgres:latest
```
4. Criar banco
```sql
CREATE DATABASE auth;
```
5. Rodar as migrations
6. Subir a aplicação e chamar o endpoint abaixo para criar o usuário de admin
```bash
login/admin
```
7. Fazer autenticação no endpoint

```bash
login
```
```
login: admin
senha: admin123
```
8. Usar demais endpoints passando o token de autenticação como Bearer
### API Endpoints
Api possuí os seguintes endpoints presente no [swagger](swagger_auth_api.json) que pode ser importado [aqui](https://editor-next.swagger.io/) para maiores detalhes

**API Login**
```markdown
POST /login/admin - Cria um usuário Admin com informações default
```
**RESPONSE**
```json
204
```
***
```markdown
POST /login - Faz autenticação do usuário
```
**BODY**
```json
{
  "login": "admin",
  "password": "admin123"
}
```
**RESPONSE**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImFkbWluQGVtYWlsLmNvbSIsImxvZ2luIjoiYWRtaW4iLCJuYmYiOjE3MTgzMzAxNTIsImV4cCI6MTcxODM1ODk1MiwiaWF0IjoxNzE4MzMwMTUyfQ.O-C9jU8-tuNiJFk5yCl2H_ffPk_lrOsHlmG8zag0"
}
```

**API User**
```markdown
POST /user - Cadastra um usuário
```
**BODY**
```json
{
  "name": "John Doe",
  "email": "john@email.com",
  "login": "john.doe",
  "password": "password123445",
  "contact": {
    "phoneNumber": "99999999999"
  }
}
```
**RESPONSE**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```
***
```markdown
GET /user - Lista todos os usuários
```

**RESPONSE**
```json
[
  {
    "id": "6f4eb410-3c2d-4d4d-8100-b6d59a46f761",
    "name": "John Doe",
    "email": "john@email.com",
    "login": "john.doe",
    "contact": {
      "id": "5b897055-6aec-41bf-8047-aea9f1ab6949",
      "phoneNumber": "99999999999"
    }
  }
]
```
***
```markdown
GET /user/{id}  - Busca usuário por Id
```

**RESPONSE**
```json
{
  "id": "6f4eb410-3c2d-4d4d-8100-b6d59a46f761",
  "name": "John Doe",
  "email": "john@email.com",
  "login": "john.doe",
  "contact": {
    "id": "5b897055-6aec-41bf-8047-aea9f1ab6949",
    "phoneNumber": "99999999999"
  }
}
```
***
```markdown
PATCH /user/{id} - Atualiza  informações de um usuário
```

**BODY**
```json
{
  "name": "John Doe",
  "email": "john@email.com",
  "login": "john.doe",
  "password": "password123445",
  "contact": {
    "phoneNumber": "99999999999"
  }
}
```
**RESPONSE**
```json
204
```
***
```markdown
DELETE /user/{id}  - Deleta usuário por Id
```

**RESPONSE**
```json
204
```
