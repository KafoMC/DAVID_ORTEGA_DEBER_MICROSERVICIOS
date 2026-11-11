# AD01 - Microservicios de libros e inventario

Solución .NET 10 con dos APIs independientes y bases de datos SQL Server separadas:

- `Libro2/Libro.Api/Libro.Api.csproj` -> `LibroDB`: consulta y administración de libros.
- `Inventario2/Inventario.Api.csproj` -> `InventarioDB`: bodegas, existencias, movimientos y reservas.

Abre `Microservicios1.slnx` en Visual Studio y ejecuta el perfil `Microservicios` para iniciar ambas APIs. También puedes iniciarlas por separado desde sus proyectos.

## Requisitos

- .NET SDK 10.
- SQL Server local. Revisa las cadenas de conexión en los `appsettings.json` si tu instancia tiene otro nombre.
- Libro HTTP: `http://localhost:5069`; Inventario HTTP: `http://localhost:5149`.

Ejecuta `database/AD01-Schema.sql` desde SQL Server Management Studio para crear `LibroDB` e `InventarioDB`. El script conserva las tablas y datos existentes.

## Swagger

Abre `http://localhost:5149/swagger` para escoger `Libro.Api v1` o `Inventario.Api v1`. También puedes abrir Libro en `http://localhost:5069/swagger`.

## Endpoints

- Libro: `/api/Libros` (GET, POST, PUT y DELETE) y `/health`.
- Inventario: `/api/Bodegas`, `/api/Existencias`, `/api/Movimientos`, `/api/Reservas` y `/health`.

El campo `IdProducto` de Inventario sirve como referencia lógica al identificador del libro; las bases de datos siguen separadas y no tienen claves foráneas entre servicios.

## Postman

Importa `postman/AD01-Microservicios.postman_collection.json`. Crea primero un libro y una bodega; usa el identificador del libro como `idProducto` al registrar su existencia.
