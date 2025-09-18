
-- Crear la base de datos
CREATE DATABASE TechStore;
GO

USE TechStore;
GO

-- Tabla BaseEntity (no se crea directamente, pero se incluye en las demás tablas)
-- Tabla Cliente
CREATE TABLE Cliente (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
    Nombre NVARCHAR(255) NOT NULL,
    Apellido NVARCHAR(255) NOT NULL,
	Documento NVARCHAR(10) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Direccion NVARCHAR(500),
    Telefono NVARCHAR(50)
);
GO

-- Tabla Producto
CREATE TABLE Producto (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
    Nombre NVARCHAR(255) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Precio DECIMAL(18, 2) NOT NULL,
    Categoria NVARCHAR(255),
    ImagenUrl NVARCHAR(MAX),
    Stock INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1
);
GO

-- Tabla Orden
CREATE TABLE Orden (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
    ClienteId UNIQUEIDENTIFIER NOT NULL,
    FechaOrden DATETIME NOT NULL DEFAULT GETUTCDATE(),
    Total DECIMAL(18, 2) NOT NULL,
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Pendiente',
    DireccionEnvio NVARCHAR(500),
    FOREIGN KEY (ClienteId) REFERENCES Cliente(Id)
);
GO

-- Tabla DetalleOrden
CREATE TABLE DetalleOrden (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
    OrdenId UNIQUEIDENTIFIER NOT NULL,
    ProductoId UNIQUEIDENTIFIER NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (OrdenId) REFERENCES Orden(Id),
    FOREIGN KEY (ProductoId) REFERENCES Producto(Id)
);
GO

-- Tabla CarritoItem
CREATE TABLE CarritoItem (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FechaCreacion DATETIME NOT NULL DEFAULT GETUTCDATE(),
    ClienteId UNIQUEIDENTIFIER NOT NULL,
    ProductoId UNIQUEIDENTIFIER NOT NULL,
    Cantidad INT NOT NULL DEFAULT 1,
    FOREIGN KEY (ClienteId) REFERENCES Cliente(Id),
    FOREIGN KEY (ProductoId) REFERENCES Producto(Id)
);
GO


INSERT INTO Cliente (Nombre, Apellido,Documento, Email, PasswordHash, Direccion, Telefono)
VALUES 
('Juan', 'Pérez', '40589874', 'juan.perez@example.com', 'hashedpassword1', 'Calle Falsa 123', '123456789'),
('Ana', 'Gómez', '40558715', 'ana.gomez@example.com', 'hashedpassword2', 'Avenida Siempre Viva 456', '987654321'),
('Carlos', 'López', '07898456', 'carlos.lopez@example.com', 'hashedpassword3', 'Calle Luna 789', '456123789');

-- Insertar datos de prueba en la tabla Producto
INSERT INTO Producto (Nombre, Descripcion, Precio, Categoria, ImagenUrl, Stock, Activo)
VALUES 
('Laptop HP', 'Laptop HP con procesador Intel i5 y 8GB RAM', 750.00, 'Laptops', 'https://example.com/laptop-hp.jpg', 10, 1),
('iPhone 14', 'Teléfono móvil Apple iPhone 14 con 128GB', 999.00, 'Móviles', 'https://example.com/iphone-14.jpg', 15, 1),
('Audífonos Sony', 'Audífonos inalámbricos Sony con cancelación de ruido', 199.00, 'Accesorios', 'https://example.com/audifonos-sony.jpg', 20, 1),
('Tablet Samsung', 'Tablet Samsung Galaxy Tab con pantalla de 10 pulgadas', 350.00, 'Tablets', 'https://example.com/tablet-samsung.jpg', 12, 1),
('Monitor LG', 'Monitor LG UltraWide de 34 pulgadas', 450.00, 'Monitores', 'https://example.com/monitor-lg.jpg', 8, 1);

-- Insertar datos de prueba en la tabla Orden
INSERT INTO Orden (ClienteId, Total, DireccionEnvio)
VALUES 
((SELECT Id FROM Cliente WHERE Email = 'juan.perez@example.com'), 1749.00, 'Calle Falsa 123'),
((SELECT Id FROM Cliente WHERE Email = 'ana.gomez@example.com'), 350.00, 'Avenida Siempre Viva 456');

-- Insertar datos de prueba en la tabla DetalleOrden
INSERT INTO DetalleOrden (OrdenId, ProductoId, Cantidad, PrecioUnitario)
VALUES 
((SELECT Id FROM Orden WHERE Total = 1749.00), (SELECT Id FROM Producto WHERE Nombre = 'Laptop HP'), 1, 750.00),
((SELECT Id FROM Orden WHERE Total = 1749.00), (SELECT Id FROM Producto WHERE Nombre = 'iPhone 14'), 1, 999.00),
((SELECT Id FROM Orden WHERE Total = 350.00), (SELECT Id FROM Producto WHERE Nombre = 'Tablet Samsung'), 1, 350.00);

-- Insertar datos de prueba en la tabla CarritoItem
INSERT INTO CarritoItem (ClienteId, ProductoId, Cantidad)
VALUES 
((SELECT Id FROM Cliente WHERE Email = 'carlos.lopez@example.com'), (SELECT Id FROM Producto WHERE Nombre = 'Audífonos Sony'), 2),
((SELECT Id FROM Cliente WHERE Email = 'ana.gomez@example.com'), (SELECT Id FROM Producto WHERE Nombre = 'Monitor LG'), 1);