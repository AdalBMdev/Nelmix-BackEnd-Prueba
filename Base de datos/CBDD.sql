create database Casinoo

use Casinoo

select * from Usuarios
select * from CuentasBancarias
select * from Fichas
select * from ApuestasUsuario


-- Tabla de Usuarios
CREATE TABLE Usuarios (
    user_id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(100),
    edad INT,
    email NVARCHAR(100),
    contraseña NVARCHAR(255), -- Se debe almacenar la contraseña hasheada
    estado_id INT, -- Clave foránea que referencia el estado del usuario
    adulto_asignado_id INT -- Clave foránea que referencia a otro usuario adulto
);

-- Tabla de EstadosUsuario
CREATE TABLE EstadosUsuario (
    estado_id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(50) -- Por ejemplo, 'activo', 'inactivo', 'prohibido', etc.
);

-- Normalizacion Estados de usuario
INSERT INTO EstadosUsuario (nombre)
VALUES
    ('Activo'),
    ('Inactivo'),
    ('Prohibido');

-- Tabla de Monedas
CREATE TABLE Monedas (
    moneda_id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(100),
    símbolo NVARCHAR(3)
);

-- Normalizacion de MOnedas
INSERT INTO Monedas (nombre, símbolo)
VALUES
    ('Dólar estadounidense', '$'),
    ('Euro', '€'),
    ('Peso mexicano', 'MXN'),
    ('Yen japonés', '¥'),
    ('Franco suizo', 'CHF'),
    ('Peso argentino', 'ARS'),
    ('Real brasileño', 'R$'),
    ('Peso dominicano', 'RD$');

-- Tabla de Cuentas Bancarias
CREATE TABLE CuentasBancarias (
    cuenta_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT, -- Clave foránea que referencia al usuario dueño de la cuenta
    moneda_id INT, -- Clave foránea que referencia a la moneda de la cuenta
    saldo DECIMAL(10, 2), 
);

-- Tabla de Tipos de Fichas
CREATE TABLE TiposDeFichas (
    tipo_ficha_id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(100), 
    valor INT -- Valor de la ficha (por ejemplo, 50, 100, 500, 1000)
);

-- Nomalizacion de Fichas
INSERT INTO TiposDeFichas (nombre, valor)
VALUES
    ('Rojas', 50),
    ('Amarillas', 100),
    ('Verdes', 500),
    ('Negras', 1000);

-- Tabla de Fichas
CREATE TABLE Fichas (
    ficha_id INT PRIMARY KEY IDENTITY(1,1),
    tipo_ficha_id INT, -- Clave foránea que referencia el tipo de ficha
    cantidad_disponible INT,
    usuario_id INT, -- Clave foránea que referencia al usuario propietario
);

-- Tabla de TasasDeCambio
CREATE TABLE TasasDeCambio (
    tasa_id INT PRIMARY KEY IDENTITY(1,1),
    moneda_id INT, -- Moneda de origen
    tasa DECIMAL(10, 5), -- Tasa de conversión a dólares
    FOREIGN KEY (moneda_id) REFERENCES Monedas(moneda_id)
);


-- Normalizacion de las Tasas de cambio
INSERT INTO TasasDeCambio (moneda_id, tasa)
VALUES
(1,1),
    (2, 1.06), -- Euro a Dólar
    (3, 0.055), -- Peso Mexicano a Dólar
    (4, 0.0067), -- Yen Japonés a Dólar
    (5, 1.12), -- Franco Suizo a Dólar
    (6, 0.0029), -- Peso Argentino a Dólar
    (7, 0.20), -- Real Brasileño a Dólar
    (8, 0.018); -- Peso Dominicano a Dólar

-- Tabla de ApuestasUsuario
CREATE TABLE ApuestasUsuario (
    apuesta_id INT PRIMARY KEY IDENTITY(1,1),
    usuario_id INT, -- Clave foránea que referencia al usuario que realizó la apuesta
    juego_id INT, -- Clave foránea que referencia el juego en el que se realizó la apuesta
    cantidad_ganada DECIMAL(10, 2),
    cantidad_perdida DECIMAL(10, 2)
);

-- Tabla de Juegos
CREATE TABLE Juegos (
    juego_id INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(100)
);

-- Normalizacion 
INSERT INTO Juegos (nombre)
VALUES
    ('Crops'),
    ('Tragaperras'),
    ('Blackjack');

------------------------------------------------Gestionar juego usuario

CREATE PROCEDURE GestionarJuegoUsuario
    @usuario_id INT,
    @fichas_rojas INT,
    @fichas_amarillas INT,
    @fichas_verdes INT,
    @fichas_negras INT,
    @victoria BIT,
    @juego_id INT
AS
BEGIN
    DECLARE @saldo_usuario DECIMAL(10, 2);
    DECLARE @valor_fichas DECIMAL(10, 2);

    -- Obtener el saldo actual del usuario
    SELECT @saldo_usuario = saldo
    FROM CuentasBancarias
    WHERE user_id = @usuario_id;

    -- Calcular el valor total de las fichas apostadas
    SELECT @valor_fichas = 
        @fichas_rojas * (SELECT valor FROM TiposDeFichas WHERE nombre = 'Rojas') +
        @fichas_amarillas * (SELECT valor FROM TiposDeFichas WHERE nombre = 'Amarillas') +
        @fichas_verdes * (SELECT valor FROM TiposDeFichas WHERE nombre = 'Verdes') +
        @fichas_negras * (SELECT valor FROM TiposDeFichas WHERE nombre = 'Negras');

    -- Actualizar las fichas disponibles si la apuesta no es igual a 0
    IF @victoria = 1
    BEGIN
        IF @fichas_rojas > 0
        BEGIN
            -- Ganó: Sumar las fichas ganadas
          UPDATE Fichas
		SET cantidad_disponible = cantidad_disponible + @fichas_rojas
		WHERE usuario_id = @usuario_id
		AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Rojas');

        END

        IF @fichas_amarillas > 0
        BEGIN
            -- Ganó: Sumar las fichas ganadas
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible + @fichas_amarillas
            WHERE usuario_id = @usuario_id
			AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Amarillas');
        END

        IF @fichas_verdes > 0
        BEGIN
            -- Ganó: Sumar las fichas ganadas
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible + @fichas_verdes
            WHERE usuario_id = @usuario_id
			AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Verdes');
        END

        IF @fichas_negras > 0
        BEGIN
            -- Ganó: Sumar las fichas ganadas
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible + @fichas_negras
            WHERE usuario_id = @usuario_id
			AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Negras');
        END
    END
    ELSE IF @victoria = 0
    BEGIN
        IF @fichas_rojas > 0
        BEGIN
            -- Perdió: Restar las fichas apostadas
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible - @fichas_rojas
            WHERE usuario_id = @usuario_id
			AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Rojas');
        END

        IF @fichas_amarillas > 0
        BEGIN
            -- Perdió: Restar las fichas apostadas
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible - @fichas_amarillas
            WHERE usuario_id = @usuario_id
			AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Amarillas');
        END

        IF @fichas_verdes > 0
        BEGIN
            -- Perdió: Restar las fichas apostadas
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible - @fichas_verdes
            WHERE usuario_id = @usuario_id
			AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Verdes');
        END

        IF @fichas_negras > 0
        BEGIN
            -- Perdió: Restar las fichas apostadas
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible - @fichas_negras
            WHERE usuario_id = @usuario_id
			AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Negras');
        END
    END
	ELSE
	BEGIN
	IF @fichas_rojas > 0
        BEGIN
            -- Perdió: Restar las fichas apostadas
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible - 0
            WHERE usuario_id = @usuario_id
			AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Rojas');
        END

        IF @fichas_amarillas > 0
        BEGIN
            -- Perdió: Restar las fichas apostadas
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible - 0
            WHERE usuario_id = @usuario_id
			AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Amarillas');
        END

        IF @fichas_verdes > 0
        BEGIN
            -- Perdió: Restar las fichas apostadas
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible - 0
            WHERE usuario_id = @usuario_id
			AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Verdes');
        END

        IF @fichas_negras > 0
        BEGIN
            -- Perdió: Restar las fichas apostadas
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible - 0
            WHERE usuario_id = @usuario_id
			AND tipo_ficha_id = (SELECT tipo_ficha_id FROM TiposDeFichas WHERE nombre = 'Negras');
        END
	END
    -- Calcular el valor en dólares de las fichas apostadas
    DECLARE @valor_en_dolares DECIMAL(10, 2);
    SELECT @valor_en_dolares = @valor_fichas * (SELECT tasa FROM TasasDeCambio WHERE moneda_id = 1);

    -- Verificar si ya existe un registro para este usuario en este juego
    DECLARE @apuesta_id INT;
    SELECT @apuesta_id = apuesta_id
    FROM ApuestasUsuario
    WHERE usuario_id = @usuario_id AND juego_id = @juego_id;

    IF @apuesta_id IS NOT NULL
    BEGIN
        -- Actualizar el registro existente
        UPDATE ApuestasUsuario
        SET cantidad_ganada = cantidad_ganada + CASE WHEN @victoria = 1 THEN @valor_en_dolares ELSE 0 END,
            cantidad_perdida = cantidad_perdida + CASE WHEN @victoria = 0 THEN @valor_en_dolares ELSE 0 END
        WHERE apuesta_id = @apuesta_id;
    END
    ELSE
    BEGIN
        -- Insertar un nuevo registro
        INSERT INTO ApuestasUsuario (usuario_id, juego_id, cantidad_ganada, cantidad_perdida)
        VALUES (@usuario_id, @juego_id,
                CASE WHEN @victoria = 1 THEN @valor_en_dolares ELSE 0 END,
                CASE WHEN @victoria = 0 THEN @valor_en_dolares ELSE 0 END);
    END
END;


--------------------------------------------------------------------------------------STORE PROCEDURE DESTINADOS A OBTENER DATOS DE GANANCIAS----------------------------------------------------------------------------------------

------------------------------------------------Obtener estado financiero usuario

CREATE PROCEDURE ObtenerEstadoFinancieroUsuario
    @usuario_id INT,
    @saldo_actual DECIMAL(10, 2) OUTPUT,
    @ganancias DECIMAL(10, 2) OUTPUT,
    @perdidas DECIMAL(10, 2) OUTPUT,
    @fichas INT OUTPUT
AS
BEGIN
    -- Obtener el saldo actual del usuario
    SELECT @saldo_actual = saldo
    FROM CuentasBancarias
    WHERE user_id = @usuario_id;

    -- Obtener las ganancias del usuario
    SELECT @ganancias = ISNULL(SUM(cantidad_ganada), 0)
    FROM ApuestasUsuario
    WHERE usuario_id = @usuario_id;

    -- Obtener las pérdidas del usuario
    SELECT @perdidas = ISNULL(SUM(cantidad_perdida), 0)
    FROM ApuestasUsuario
    WHERE usuario_id = @usuario_id;

    -- Obtener la cantidad de fichas del usuario
    SELECT @fichas = ISNULL(SUM(cantidad_disponible), 0)
    FROM Fichas
    WHERE usuario_id = @usuario_id;
END;

------------------------------------------------Obtener ganancias y pérdidas por juego

CREATE PROCEDURE ObtenerGananciasYPérdidasPorJuego
    @usuario_id INT
AS
BEGIN
    -- Obtener las ganancias y pérdidas del usuario por juego
    SELECT Juegos.nombre AS nombre_juego, ISNULL(SUM(AU.cantidad_ganada), 0) AS ganancias, ISNULL(SUM(AU.cantidad_perdida), 0) AS pérdidas
    FROM Juegos
    LEFT JOIN ApuestasUsuario AU ON Juegos.juego_id = AU.juego_id AND AU.usuario_id = @usuario_id
    GROUP BY Juegos.nombre;
END;