-- Esquema AD01: bases separadas, preparado para las tablas del proyecto existente.
IF DB_ID(N'CatalogoDB') IS NULL EXEC(N'CREATE DATABASE CatalogoDB');
IF DB_ID(N'PedidosDB') IS NULL EXEC(N'CREATE DATABASE PedidosDB');
IF DB_ID(N'InventarioDB') IS NULL EXEC(N'CREATE DATABASE InventarioDB');
GO
USE CatalogoDB;
GO
IF OBJECT_ID(N'dbo.Categoria',N'U') IS NULL CREATE TABLE dbo.Categoria(IdCategoria int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categoria PRIMARY KEY,Nombre varchar(80) NOT NULL);
IF OBJECT_ID(N'dbo.Marca',N'U') IS NULL CREATE TABLE dbo.Marca(IdMarca int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Marca PRIMARY KEY,Nombre varchar(80) NOT NULL);
IF OBJECT_ID(N'dbo.Productos',N'U') IS NULL CREATE TABLE dbo.Productos(IdProducto int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Productos PRIMARY KEY,Nombre varchar(120) NOT NULL,Descripcion varchar(250) NULL,Precio decimal(10,2) NOT NULL,IdMarca int NOT NULL,IdCategoria int NOT NULL,CONSTRAINT FK_Productos_Marca FOREIGN KEY(IdMarca) REFERENCES dbo.Marca(IdMarca),CONSTRAINT FK_Productos_Categoria FOREIGN KEY(IdCategoria) REFERENCES dbo.Categoria(IdCategoria),CONSTRAINT CK_Productos_Precio CHECK(Precio>=0));
GO
USE PedidosDB;
GO
IF OBJECT_ID(N'dbo.Cliente',N'U') IS NULL CREATE TABLE dbo.Cliente(IdCliente int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Cliente PRIMARY KEY,Nombre varchar(120) NOT NULL,Cedula varchar(20) NULL,Telefono varchar(30) NULL,Correo varchar(120) NULL);
IF OBJECT_ID(N'dbo.Pedido',N'U') IS NULL CREATE TABLE dbo.Pedido(IdPedido int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pedido PRIMARY KEY,IdCliente int NOT NULL,Fecha datetime NULL,Estado varchar(20) NULL,CONSTRAINT FK_Pedido_Cliente FOREIGN KEY(IdCliente) REFERENCES dbo.Cliente(IdCliente));
IF OBJECT_ID(N'dbo.DetallePedido',N'U') IS NULL CREATE TABLE dbo.DetallePedido(IdDetallePedido int IDENTITY(1,1) NOT NULL CONSTRAINT PK_DetallePedido PRIMARY KEY,IdPedido int NOT NULL,IdProducto int NOT NULL,Cantidad int NOT NULL,PrecioUnitario decimal(10,2) NOT NULL,IdBodega int NOT NULL CONSTRAINT DF_DetallePedido_IdBodega DEFAULT(1),CONSTRAINT FK_DetallePedido_Pedido FOREIGN KEY(IdPedido) REFERENCES dbo.Pedido(IdPedido),CONSTRAINT CK_DetallePedido_Cantidad CHECK(Cantidad>0));
IF COL_LENGTH(N'dbo.DetallePedido',N'IdBodega') IS NULL ALTER TABLE dbo.DetallePedido ADD IdBodega int NOT NULL CONSTRAINT DF_DetallePedido_IdBodega DEFAULT(1);
GO
USE InventarioDB;
GO
IF OBJECT_ID(N'dbo.Bodega',N'U') IS NULL CREATE TABLE dbo.Bodega(IdBodega int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Bodega PRIMARY KEY,Nombre nvarchar(100) NOT NULL,Ciudad nvarchar(100) NOT NULL,Direccion nvarchar(250) NOT NULL);
IF COL_LENGTH(N'dbo.Bodega',N'Ciudad') IS NULL ALTER TABLE dbo.Bodega ADD Ciudad nvarchar(100) NOT NULL CONSTRAINT DF_Bodega_Ciudad DEFAULT(N'No especificada');
IF OBJECT_ID(N'dbo.Existencia',N'U') IS NULL CREATE TABLE dbo.Existencia(IdExistencia int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Existencia PRIMARY KEY,IdProducto int NOT NULL,IdBodega int NOT NULL,Stock int NOT NULL,CONSTRAINT AK_Existencia UNIQUE(IdProducto,IdBodega),CONSTRAINT FK_Existencia_Bodega FOREIGN KEY(IdBodega) REFERENCES dbo.Bodega(IdBodega),CONSTRAINT CK_Existencia_Stock CHECK(Stock>=0));
IF OBJECT_ID(N'dbo.MovimientoInventario',N'U') IS NULL CREATE TABLE dbo.MovimientoInventario(IdMovimiento int IDENTITY(1,1) NOT NULL CONSTRAINT PK_MovimientoInventario PRIMARY KEY,IdProducto int NOT NULL,IdBodega int NOT NULL,Tipo nvarchar(20) NOT NULL,Cantidad int NOT NULL,Fecha datetime2 NOT NULL CONSTRAINT DF_Movimiento_Fecha DEFAULT SYSUTCDATETIME(),CONSTRAINT FK_Movimiento_Existencia FOREIGN KEY(IdProducto,IdBodega) REFERENCES dbo.Existencia(IdProducto,IdBodega),CONSTRAINT CK_Movimiento_Tipo CHECK(Tipo IN(N'Entrada',N'Salida')),CONSTRAINT CK_Movimiento_Cantidad CHECK(Cantidad>0));
IF OBJECT_ID(N'dbo.ReservaStock',N'U') IS NULL CREATE TABLE dbo.ReservaStock(IdReserva int IDENTITY(1,1) NOT NULL CONSTRAINT PK_ReservaStock PRIMARY KEY,IdProducto int NOT NULL,IdBodega int NOT NULL,Cantidad int NOT NULL,Estado nvarchar(20) NOT NULL CONSTRAINT DF_Reserva_Estado DEFAULT N'Activa',CONSTRAINT FK_Reserva_Existencia FOREIGN KEY(IdProducto,IdBodega) REFERENCES dbo.Existencia(IdProducto,IdBodega),CONSTRAINT CK_Reserva_Cantidad CHECK(Cantidad>0));
GO
