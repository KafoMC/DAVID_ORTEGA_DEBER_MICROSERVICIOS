# AD01 - Aplicaciones distribuidas para comercio
Solución .NET 10 con tres APIs y bases separadas, según el diagrama:
- Pedidos.Api (`PedidosDB`, http://localhost:5079): pedidos, detalle y cliente.
- Catalogo.Api (`CatalogoDB`, http://localhost:5069): productos, categoría, marca y precio.
- Inventario.Api (`InventarioDB`, http://localhost:5149): bodegas, existencias, movimientos y reservas.
Pedidos consulta productos en Catálogo y solicita reservas a Inventario. Cada servicio mantiene su propia base.

Abre `Microservicios1.slnx` en Visual Studio 2026 e inicia el perfil `AD01 - Comercio` para ejecutar los tres servicios. Ejecuta `database/AD01-Schema.sql` en SQL Server Management Studio y abre `http://localhost:5149/swagger`. Si tu SQL Server tiene otro nombre, ajusta Server en cada `appsettings.json`.

Flujo: crea producto en Catálogo; crea bodega y existencia para ese IdProducto en Inventario; crea el pedido en Pedidos indicando IdProducto, IdBodega y Cantidad. La colección está en `postman/AD01-Microservicios.postman_collection.json`.
