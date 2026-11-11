# AD01 - Microservicios de libros e inventario

Solución .NET 10 con dos APIs independientes y bases de datos SQL Server separadas:

- `Libro2/Libro.Api/Libro.Api.csproj` -> `LibroDB`: consulta y administración de libros.
- `Inventario2/Inventario.Api.csproj` -> `InventarioDB`: bodegas, existencias, movimientos y reservas.

Abre `Microservicios1.slnx` en Visual Studio y ejecuta el perfil `Microservicios` para iniciar ambas APIs. También puedes iniciarlas por separado desde sus proyectos.

## Requisitos

- .NET SDK 10.
- SQL Server local. Revisa las cadenas de conexión en los `appsettings.json` si tu instancia tiene otro nombre.
- Libro HTTPS: `https://localhost:7244`; Inventario HTTPS: `https://localhost:7228`.

Ejecuta `database/AD01-Schema.sql` desde SQL Server Management Studio para crear `LibroDB` e `InventarioDB`. El script conserva las tablas y datos existentes.

## Swagger

La página Swagger de Libro permite escoger entre `Libro.Api v1` e `Inventario.Api v1`. También puedes abrir directamente `/swagger` en el puerto de cada API.

## Endpoints

- Libro: `/api/Libros` (GET, POST, PUT y DELETE) y `/health`.
- Inventario: `/api/Bodegas`, `/api/Existencias`, `/api/Movimientos`, `/api/Reservas` y `/health`.

El campo `IdProducto` de Inventario sirve como referencia lógica al identificador del libro; las bases de datos siguen separadas y no tienen claves foráneas entre servicios.

## Postman

Importa `postman/AD01-Microservicios.postman_collection.json`. Crea primero un libro y una bodega; usa el identificador del libro como `idProducto` al registrar su existencia.
