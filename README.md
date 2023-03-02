# backend-project

- [Project version](https://github.com/wythh24/backend-project/tags)

## Table of Contents

* [About project](#about-project)
* [Technologies](#technologies)
* [Nuget packages](#nuget-packages)
* [Setup](#setup)
* [Usage](#usage)

## About project

- This project build with .Net entity framework core 6 and by code first it’s flexible to any database project because
  and any platforms

## Technologies

- [ASP.NET Core 6.2.3](https://dotnet.microsoft.com/en-us/download)
- [MySql](https://dev.mysql.com/)
- [Httprepl](https://www.nuget.org/packages/Microsoft.dotnet-httprepl)
- [Swagger](https://www.npmjs.com/package/swagger)
- [Postman](https://www.postman.com/downloads/)

## Nuget packages

> The following Nuget packages are already installed

- [Auto mapper](https://www.nuget.org/packages/AutoMapper)
- [AutoMapper.Extensions.Microsoft.DependencyInjection](https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection)
- [FluentValidation.AspNetCore](https://www.nuget.org/packages/FluentValidation.AspNetCore)
- [FluentValidation.DependencyInjectionExtensions](https://www.nuget.org/packages/FluentValidation.DependencyInjectionExtensions)
- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/8.0.0-preview.1.23111.4)
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages?q=Microsoft.EntityFrameworkCore.Design)
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/8.0.0-preview.1.23111.4)
- [Microsoft.EntityFrameworkCore.tools](https://www.nuget.org/packages?q=Microsoft.EntityFrameworkCore.tools&frameworks=&tfms=&packagetype=&prerel=true&sortby=relevance)
- [Microsoft.VisualStudio.Web.CodeGeneration.Design](https://www.nuget.org/packages/Microsoft.VisualStudio.Web.CodeGeneration.Design/8.0.0-preview.1.23117.2)
- [Mysql.Data.EntityFrameworkCore](https://www.nuget.org/packages/MySql.Data.EntityFrameworkCore)
- [MySql.EntityFrameworkCore](https://www.nuget.org/packages/MySql.EntityFrameworkCore)
- [Pomelo.EntityFrameworkCore.MySql](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql)
- [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore)

## Setup

#### Clone project

```bash
git clone https://github.com/wythh24/backend-project.git
```

#### Configuration connection string

When working with this project, which has been built using code first, it is possible to use it with any relational
database due to its flexibility. However, once the project has been cloned, it is necessary to configure the connection
string in the appsetting.json file

- This project was used mysql database

> Must create database before migration to get tables

```json
 "Development": "Server=localhost;Database=yourDatabaseName;Uid=yourUsername;Pwd=yourUserPassword"
```

#### Migration

Open terminal in project folder and run

- `initialize` it's just named for each migration

```bash
dotnet ef migrations add initialize 
```

And run

```bash
dotnet ef database update
```

## Usage

This project creates ths following API:

- Product controller

| API                     | Description              | Request body       | Response body        |
|-------------------------|--------------------------|--------------------|----------------------|
| POST api/product/getall | Get all product/specific | None/product list  | List of product item |
| GET api/product         | Get all product/specific | None/product list  | List of product item |
| GET api/product/getbyid | Get a product            | product list       | A product item       |
| GET api/product/{id}    | Get a product            | None               | A product item       |
| POST api/product        | Create product as List   | Product list       | List of product id   |
| PUT api/product         | Update product as List   | Product list       | List of product id   |
| DELETE api/product      | Delete product as List   | List of product id | List of product id   |

### Get all product using POST method

```http request
POST https://localhost:7170/api/Product/getAll
Content-Type: application/json

{
  "id": [
  "productId",
  "productId"
  ]
}
```

### Get all product or specific using GET method

```http request
GET https://localhost:7170/api/Product
Content-Type: application/json

{
  "id": [
    "73b3cb2f-77a2-41e5-ae86-014ba188843b"
    
  ]
}
```

### Get all of product using GET/POST

```http request
POST https://localhost:7170/api/Product/getAll
Content-Type: application/json

{
  "id": null
}
```

### Get a product with id using GET method (No body)

```http request
GET https://localhost:7170/api/Product/73b3cb2f-77a2-41e5-ae86-014ba188843
```

### Get a product with id using GET method (body)

```http request
GET https://localhost:7170/api/Product/GetById/
Content-Type: application/json

{
  "id": "73b3cb2f-77a2-41e5-ae86-014ba18883b"
}
```
### Create list of product using POST method 

```http request
POST https://localhost:7170/api/Product
Content-Type: application/json

{
  "command": [
    {
      "name": "product name",
      "price": "price(double)",
      "description": null,
      "code": "product code"
    },
    {
      "name": "apple",
      "price": 3.0,
      "description": "product from usa",
      "code": "us-9000-1"
    }

  ]
}
```

### Update list of product using PUT

```http request
PUT https://localhost:7170/api/Product/
Content-Type: application/json

{
  "command": [
    {
      "id": "df06fb32-d840-4abc-bbfd-178e3e742fc1",
      "name": "1111",
      "price": 20.0
    },
    {
      "id": "9bbd9f6f-5e1c-4cf8-a952-5bb65cd39dd2",
      "name": "2222",
      "price": 10.0
    }
  ]
}
```

### Delete list of Product using DELETE method

```http request
DELETE https://localhost:7170/api/Product/
Content-Type: application/json

{
  "id": [
    "a7441313-532e-4766-a972-47be0b62c155"
  ]
}
```
## License

MIT License

Copyright (c) 2023 wythh24

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
