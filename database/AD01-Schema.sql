-- Ejecutar en SQL Server Management Studio con permiso para crear bases y tablas.
-- El script conserva las tablas y los datos existentes.

IF DB_ID(N'LibroDB') IS NULL EXEC(N'CREATE DATABASE LibroDB');
IF DB_ID(N'InventarioDB') IS NULL EXEC(N'CREATE DATABASE InventarioDB');
GO

USE LibroDB;
GO
IF OBJECT_ID(N'dbo.Libros', N'U') IS NULL
    CREATE TABLE dbo.Libros (
        IdLibro int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Libros PRIMARY KEY,
        Titulo varchar(100) NOT NULL,
        Autor nvarchar(100) NOT NULL,
        Isbn nvarchar(20) NOT NULL,
        Precio decimal(10,2) NOT NULL,
        Estado bit NOT NULL CONSTRAINT DF_Libros_Estado DEFAULT (1),
        CONSTRAINT CK_Libros_Precio CHECK (Precio >= 0)
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
        IdProducto int NOT NULL, -- Referencia lógica al IdLibro de LibroDB; no hay FK entre servicios.
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
