# AD01 - Microservicios para comercio multibodega

Solución .NET 10 con tres APIs independientes y bases de datos SQL Server separadas:

- `Catalogo.Api` -> `CatalogoDB`: categorías, marcas y productos.
- `Inventario2/Inventario.Api.csproj` -> `InventarioDB`: bodegas, existencias por bodega, movimientos y reservas.
- `Pedidos.Api` -> `PedidosDB`: clientes, pedidos y detalles.

Cada servicio se puede ejecutar por separado. En Visual Studio, abre `Microservicios1.slnx` y usa el perfil de inicio `Microservicios` para iniciar los tres.

## Requisitos

- .NET SDK 10.
- SQL Server local con autenticación de Windows.
- Para los puertos de desarrollo configurados: Inventario HTTPS 44316, Catálogo HTTPS 7244 y Pedidos HTTPS 7290.

Los connection strings están en los `appsettings.json`; cámbialos si tu instancia SQL Server tiene otro nombre. Ejecuta `database/AD01-Schema.sql` una vez en SQL Server Management Studio para crear las tres bases y sus tablas. Ejecutar el script no borra ni sobrescribe las tablas que ya existan. Si tienes una versión anterior de PedidosDB, revisa el bloque de actualización del script antes de usarlo.

## Swagger

Abre `https://localhost:44316/swagger` para escoger en la lista Catálogo, Inventario o Pedidos. Los otros servicios también exponen Swagger en sus puertos.

## Endpoints principales

- Catálogo: `/api/Categorias`, `/api/Marcas`, `/api/Productos`.
- Inventario: `/api/Bodegas`, `/api/Existencias`, `/api/Movimientos`, `/api/Reservas`.
- Pedidos: `/api/Clientes`, `/api/Pedidos`.
- Cada API tiene `/health`.

Las claves de producto entre Catálogo, Inventario y Pedidos son referencias lógicas entre servicios; no se crean llaves foráneas entre bases independientes. Las llaves foráneas locales (Producto-Categoría/Marca, Existencia-Bodega, Pedido-Cliente y Detalle-Pedido) se configuran dentro de cada base.

## Colección de Postman

Importa `postman/AD01-Microservicios.postman_collection.json`. La colección trae variables de base URL y solicitudes CRUD para iniciar las comprobaciones. Ejecuta primero los POST de categoría, marca y producto, luego crea bodega y existencia antes de probar movimientos/reservas; crea un cliente antes del pedido.

## Repositorio

El `.gitignore` excluye `bin`, `obj`, configuración de desarrollo y los proyectos de ejemplo antiguos que no pertenecen a esta solución.
