-- Ejecutar en SQL Server Management Studio como un usuario con permiso para crear bases/tablas.
-- Este script es idempotente para objetos existentes: no elimina ni vacía datos.

IF DB_ID(N'CatalogoDB') IS NULL EXEC(N'CREATE DATABASE CatalogoDB');
IF DB_ID(N'InventarioDB') IS NULL EXEC(N'CREATE DATABASE InventarioDB');
IF DB_ID(N'PedidosDB') IS NULL EXEC(N'CREATE DATABASE PedidosDB');
GO

USE CatalogoDB;
GO
IF OBJECT_ID(N'dbo.Categoria', N'U') IS NULL
    CREATE TABLE dbo.Categoria (
        IdCategoria int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categoria PRIMARY KEY,
        Nombre nvarchar(100) NOT NULL,
        Descripcion nvarchar(300) NULL
    );
IF OBJECT_ID(N'dbo.Marca', N'U') IS NULL
    CREATE TABLE dbo.Marca (
        IdMarca int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Marca PRIMARY KEY,
        Nombre nvarchar(100) NOT NULL,
        PaisOrigen nvarchar(100) NULL
    );
IF OBJECT_ID(N'dbo.Producto', N'U') IS NULL
    CREATE TABLE dbo.Producto (
        IdProducto int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Producto PRIMARY KEY,
        IdCategoria int NOT NULL,
        IdMarca int NOT NULL,
        Nombre nvarchar(150) NOT NULL,
        SKU nvarchar(40) NOT NULL,
        Precio decimal(10,2) NOT NULL,
        Estado nvarchar(20) NOT NULL CONSTRAINT DF_Producto_Estado DEFAULT N'Activo',
        CONSTRAINT UQ_Producto_SKU UNIQUE (SKU),
        CONSTRAINT FK_Producto_Categoria FOREIGN KEY (IdCategoria) REFERENCES dbo.Categoria(IdCategoria),
        CONSTRAINT FK_Producto_Marca FOREIGN KEY (IdMarca) REFERENCES dbo.Marca(IdMarca),
        CONSTRAINT CK_Producto_Precio CHECK (Precio >= 0)
    );
GO

USE InventarioDB;
GO
IF OBJECT_ID(N'dbo.Bodega', N'U') IS NULL
    CREATE TABLE dbo.Bodega (
        IdBodega int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Bodega PRIMARY KEY,
        Nombre nvarchar(100) NOT NULL,
        Ciudad nvarchar(100) NOT NULL,
        Direccion nvarchar(250) NOT NULL
    );
IF OBJECT_ID(N'dbo.Existencia', N'U') IS NULL
    CREATE TABLE dbo.Existencia (
        IdExistencia int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Existencia PRIMARY KEY,
        IdProducto int NOT NULL, -- Referencia lógica a CatalogoDB.dbo.Producto
        IdBodega int NOT NULL,
        Stock int NOT NULL,
        CONSTRAINT AK_Existencia_Producto_Bodega UNIQUE (IdProducto, IdBodega),
        CONSTRAINT FK_Existencia_Bodega FOREIGN KEY (IdBodega) REFERENCES dbo.Bodega(IdBodega),
        CONSTRAINT CK_Existencia_Stock CHECK (Stock >= 0)
    );
IF OBJECT_ID(N'dbo.MovimientoInventario', N'U') IS NULL
    CREATE TABLE dbo.MovimientoInventario (
        IdMovimiento int IDENTITY(1,1) NOT NULL CONSTRAINT PK_MovimientoInventario PRIMARY KEY,
        IdProducto int NOT NULL,
        IdBodega int NOT NULL,
        Tipo nvarchar(20) NOT NULL,
        Cantidad int NOT NULL,
        Fecha datetime2 NOT NULL CONSTRAINT DF_Movimiento_Fecha DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_Movimiento_Existencia FOREIGN KEY (IdProducto, IdBodega) REFERENCES dbo.Existencia(IdProducto, IdBodega),
        CONSTRAINT CK_Movimiento_Tipo CHECK (Tipo IN (N'Entrada', N'Salida')),
        CONSTRAINT CK_Movimiento_Cantidad CHECK (Cantidad > 0)
    );
IF OBJECT_ID(N'dbo.ReservaStock', N'U') IS NULL
    CREATE TABLE dbo.ReservaStock (
        IdReserva int IDENTITY(1,1) NOT NULL CONSTRAINT PK_ReservaStock PRIMARY KEY,
        IdProducto int NOT NULL,
        IdBodega int NOT NULL,
        Cantidad int NOT NULL,
        Estado nvarchar(20) NOT NULL CONSTRAINT DF_Reserva_Estado DEFAULT N'Activa',
        CONSTRAINT FK_Reserva_Existencia FOREIGN KEY (IdProducto, IdBodega) REFERENCES dbo.Existencia(IdProducto, IdBodega),
        CONSTRAINT CK_Reserva_Cantidad CHECK (Cantidad > 0)
    );
GO

USE PedidosDB;
GO
-- Ajustes compatibles con la versión previa del proyecto, preservan las filas existentes.
IF OBJECT_ID(N'dbo.Cliente', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.Cliente', N'Nombres') IS NULL
   AND COL_LENGTH(N'dbo.Cliente', N'Nombre') IS NOT NULL
    EXEC sys.sp_rename N'dbo.Cliente.Nombre', N'Nombres', N'COLUMN';
IF OBJECT_ID(N'dbo.DetallePedido', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.DetallePedido', N'IdDetalle') IS NULL
   AND COL_LENGTH(N'dbo.DetallePedido', N'IdDetallePedido') IS NOT NULL
    EXEC sys.sp_rename N'dbo.DetallePedido.IdDetallePedido', N'IdDetalle', N'COLUMN';
IF OBJECT_ID(N'dbo.Pedido', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.Pedido', N'Total') IS NULL
    ALTER TABLE dbo.Pedido ADD Total decimal(12,2) NOT NULL CONSTRAINT DF_Pedido_Total DEFAULT (0);

IF OBJECT_ID(N'dbo.Cliente', N'U') IS NULL
    CREATE TABLE dbo.Cliente (
        IdCliente int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Cliente PRIMARY KEY,
        Nombres varchar(150) NOT NULL,
        Cedula varchar(20) NULL,
        Telefono varchar(20) NULL,
        Correo varchar(100) NULL
    );
IF OBJECT_ID(N'dbo.Pedido', N'U') IS NULL
    CREATE TABLE dbo.Pedido (
        IdPedido int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pedido PRIMARY KEY,
        IdCliente int NOT NULL,
        Fecha datetime2 NULL,
        Total decimal(12,2) NOT NULL CONSTRAINT DF_Pedido_Total_Nuevo DEFAULT (0),
        Estado varchar(30) NULL,
        CONSTRAINT FK_Pedido_Cliente FOREIGN KEY (IdCliente) REFERENCES dbo.Cliente(IdCliente)
    );
IF OBJECT_ID(N'dbo.DetallePedido', N'U') IS NULL
    CREATE TABLE dbo.DetallePedido (
        IdDetalle int IDENTITY(1,1) NOT NULL CONSTRAINT PK_DetallePedido PRIMARY KEY,
        IdPedido int NOT NULL,
        IdProducto int NOT NULL, -- Referencia lógica a CatalogoDB.dbo.Producto
        Cantidad int NOT NULL,
        PrecioUnitario decimal(10,2) NOT NULL,
        CONSTRAINT FK_DetallePedido_Pedido FOREIGN KEY (IdPedido) REFERENCES dbo.Pedido(IdPedido),
        CONSTRAINT CK_DetallePedido_Cantidad CHECK (Cantidad > 0),
        CONSTRAINT CK_DetallePedido_Precio CHECK (PrecioUnitario >= 0)
    );
GO
