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

--------------------------------------------------------------------------------------STORE PROCEDURE DESTINADOS AL USUARIO----------------------------------------------------------------------------------------

--------------------------------------------------------------------Register
CREATE PROCEDURE sp_RegistrarUsuario
    @nombre NVARCHAR(100),
    @edad INT,
    @email NVARCHAR(100),
    @contraseña NVARCHAR(255),
    @estado_id INT,
    @adulto_asignado_id INT,
    @Registrado BIT OUTPUT,
    @Mensaje NVARCHAR(100) OUTPUT
AS
BEGIN
    -- Verificar si el correo es único
    IF (NOT EXISTS (SELECT * FROM Usuarios WHERE email = @email))
    BEGIN
        -- Insertar el nuevo usuario
        INSERT INTO Usuarios (nombre, edad, email, contraseña, estado_id, adulto_asignado_id)
        VALUES (@nombre, @edad, @email, @contraseña, @estado_id, @adulto_asignado_id);

        SET @Registrado = 1;
        SET @Mensaje = 'Usuario registrado exitosamente.';
    END
    ELSE
    BEGIN
        SET @Registrado = 0;
        SET @Mensaje = 'El correo ya existe en la base de datos.';
    END
END;


--------------------------------------------------------------------Login
CREATE PROCEDURE sp_IniciarSesionUsuario
    @email NVARCHAR(100),
    @contraseña NVARCHAR(255),
    @UsuarioEncontrado BIT OUTPUT,
    @Mensaje NVARCHAR(100) OUTPUT
AS
BEGIN
    -- Verificar si el usuario existe y la contraseña es correcta
    IF (EXISTS (SELECT * FROM Usuarios WHERE email = @email AND contraseña = @contraseña))
    BEGIN
        SET @UsuarioEncontrado = 1;
        SET @Mensaje = 'Inicio de sesión exitoso.';
    END
    ELSE
    BEGIN
        SET @UsuarioEncontrado = 0;
        SET @Mensaje = 'Correo o contraseña incorrectos. Por favor, inténtalo de nuevo.';
    END
END;

--------------------------------------------------------------------Asignar adulto

CREATE PROCEDURE sp_AsignarAdultoResponsable
    @CorreoUsuarioMenor NVARCHAR(100),
    @NombreAdulto NVARCHAR(100),
    @Registrado BIT OUTPUT,
    @Mensaje NVARCHAR(100) OUTPUT
AS
BEGIN
    -- Verificar la edad del usuario menor
    DECLARE @EdadMenor INT
    SELECT @EdadMenor = edad FROM Usuarios WHERE email = @CorreoUsuarioMenor

    IF @EdadMenor IS NULL
    BEGIN
        SET @Registrado = 0;
        SET @Mensaje = 'No se encontró el usuario menor con ese correo electrónico.';
    END
    ELSE IF @EdadMenor >= 21
    BEGIN
        SET @Registrado = 0;
        SET @Mensaje = 'El usuario menor no cumple con la edad máxima de 20 años.';
    END
    ELSE
    BEGIN
        -- Verificar la edad del adulto
        DECLARE @EdadAdulto INT
        SELECT @EdadAdulto = edad FROM Usuarios WHERE nombre = @NombreAdulto

        IF @EdadAdulto IS NULL
        BEGIN
            SET @Registrado = 0;
            SET @Mensaje = 'No se encontró un adulto con ese nombre.';
        END
        ELSE IF @EdadAdulto < 21
        BEGIN
            SET @Registrado = 0;
            SET @Mensaje = 'El adulto no cumple con la edad mínima de 21 años.';
        END
        ELSE
        BEGIN
            -- Asignar el ID del adulto responsable al usuario menor
            DECLARE @IdAdulto INT
            SELECT @IdAdulto = user_id FROM Usuarios WHERE nombre = @NombreAdulto

            UPDATE Usuarios
            SET adulto_asignado_id = @IdAdulto
            WHERE email = @CorreoUsuarioMenor;

            SET @Registrado = 1;
            SET @Mensaje = 'Asignación de adulto responsable exitosa.';
        END
    END
END;

--------------------------------------------------------------------Cambio de contraseña 
CREATE PROCEDURE sp_CambiarContraseña
    @email NVARCHAR(100),
    @contraseñaActual NVARCHAR(255),
    @nuevaContraseña NVARCHAR(255),
    @ContraseñaCambiada BIT OUTPUT,
    @Mensaje NVARCHAR(100) OUTPUT
AS
BEGIN
    -- Verificar si el usuario existe y la contraseña actual es correcta
    IF (EXISTS (SELECT * FROM Usuarios WHERE email = @email AND contraseña = @contraseñaActual))
    BEGIN
        -- Actualizar la contraseña
        UPDATE Usuarios
        SET contraseña = @nuevaContraseña
        WHERE email = @email;

        SET @ContraseñaCambiada = 1;
        SET @Mensaje = 'Contraseña cambiada exitosamente.';
    END
    ELSE
    BEGIN
        SET @ContraseñaCambiada = 0;
        SET @Mensaje = 'La contraseña actual es incorrecta. Por favor, inténtalo de nuevo.';
    END
END;

--------------------------------------------------------------------Inhabilitar usuario

CREATE PROCEDURE CambiarEstadoUsuarioAInactivo
    @usuario_id INT
AS
BEGIN
    UPDATE Usuarios
    SET estado_id = (SELECT estado_id FROM EstadosUsuario WHERE nombre = 'Inactivo')
    WHERE user_id = @usuario_id;
END;

--------------------------------------------------------------------------------------STORE PROCEDURE DESTINADOS A CUENTAS BANCARIAS----------------------------------------------------------------------------------------

------------------------------------------------Crear cuenta bancaria

CREATE PROCEDURE sp_CrearCuentaBancaria
    @user_id INT,
    @moneda_id INT,
    @saldo DECIMAL(10, 2)
AS
BEGIN
    INSERT INTO CuentasBancarias (user_id, moneda_id, saldo)
    VALUES (@user_id, @moneda_id, @saldo);
END;


------------------------------------------------Eliminar cuenta bancaria

CREATE PROCEDURE sp_EliminarCuentaBancaria
    @cuenta_id INT,
    @user_id INT
AS
BEGIN
    -- Verificar que la cuenta bancaria pertenezca al usuario que intenta eliminarla
    IF NOT EXISTS (SELECT 1 FROM CuentasBancarias WHERE cuenta_id = @cuenta_id AND user_id = @user_id)
    BEGIN
        -- Generar un error si la cuenta bancaria no pertenece al usuario
        THROW 51000, 'La cuenta bancaria no pertenece al usuario especificado o no tiene cuenta bancaria.', 1;
    END

    -- Realizar la eliminación
    DELETE FROM CuentasBancarias
    WHERE cuenta_id = @cuenta_id;
END;

--------------------------------------------------------------------------------------STORE PROCEDURE DESTINADOS A DIVISAS----------------------------------------------------------------------------------------

------------------------------------------------Convertir Moneda A Dolares
CREATE PROCEDURE ConvertirMonedaADolares
    @cuenta_id INT,
    @nuevoSaldo DECIMAL(18, 4) OUTPUT -- Aumenta la precisión
AS
BEGIN
    -- Declarar variables para almacenar información
    DECLARE @saldoActual DECIMAL(18, 4); -- Aumenta la precisión
    DECLARE @tasaConversion DECIMAL(18, 4); -- Aumenta la precisión
    DECLARE @moneda_id INT;

    -- Obtener el saldo actual y el moneda_id de la cuenta bancaria
    SELECT @saldoActual = saldo, @moneda_id = moneda_id
    FROM CuentasBancarias
    WHERE cuenta_id = @cuenta_id;

    -- Obtener la tasa de conversión a dólares desde la tabla TasasDeCambio
    SELECT @tasaConversion = tasa
    FROM TasasDeCambio
    WHERE moneda_id = @moneda_id;

    -- Calcular el nuevo saldo en dólares
    SET @nuevoSaldo = @saldoActual * @tasaConversion;

    -- Actualizar el moneda_id a la moneda que representa el dólar
    UPDATE CuentasBancarias
    SET moneda_id = (SELECT moneda_id FROM Monedas WHERE nombre = 'Dólar estadounidense')
    WHERE cuenta_id = @cuenta_id;

    -- Actualizar el saldo con el nuevo saldo en dólares
    UPDATE CuentasBancarias
    SET saldo = @nuevoSaldo
    WHERE cuenta_id = @cuenta_id;
END;

------------------------------------------------Comprar fichas en dolares

CREATE PROCEDURE ComprarFichasEnDolares
    @usuario_id INT,
    @tipo_ficha_id INT,
    @cantidad INT
AS
BEGIN
    -- Verificar si ya existe un registro del usuario con el tipo de ficha seleccionado
    DECLARE @registroExistente INT;
    SELECT @registroExistente = COUNT(*)
    FROM Fichas
    WHERE usuario_id = @usuario_id AND tipo_ficha_id = @tipo_ficha_id;

    -- Declarar variables para almacenar información
    DECLARE @valorFichaEnDolares DECIMAL(10, 5);
    DECLARE @costoTotalEnDolares DECIMAL(10, 2);

    -- Obtener el valor de la ficha en dólares desde la tabla TiposDeFichas
    SELECT @valorFichaEnDolares = valor
    FROM TiposDeFichas
    WHERE tipo_ficha_id = @tipo_ficha_id;

    -- Calcular el costo total en dólares
    SET @costoTotalEnDolares = @valorFichaEnDolares * @cantidad;

    -- Verificar si el usuario tiene suficiente saldo en su cuenta en dólares
    DECLARE @saldoUsuarioDolares DECIMAL(10, 2);
    SELECT @saldoUsuarioDolares = saldo
    FROM CuentasBancarias
    WHERE user_id = @usuario_id AND moneda_id = (SELECT moneda_id FROM Monedas WHERE nombre = 'Dólar estadounidense');

    IF @saldoUsuarioDolares >= @costoTotalEnDolares
    BEGIN
        -- Restar el costo de las fichas al saldo del usuario en dólares
        UPDATE CuentasBancarias
        SET saldo = saldo - @costoTotalEnDolares
        WHERE user_id = @usuario_id AND moneda_id = (SELECT moneda_id FROM Monedas WHERE nombre = 'Dólar estadounidense');

        -- Si ya existe un registro, actualizar la cantidad de fichas
        IF @registroExistente > 0
        BEGIN
            UPDATE Fichas
            SET cantidad_disponible = cantidad_disponible + @cantidad
            WHERE usuario_id = @usuario_id AND tipo_ficha_id = @tipo_ficha_id;
        END
        ELSE
        BEGIN
            -- Si no existe un registro, insertar uno nuevo
            INSERT INTO Fichas (tipo_ficha_id, cantidad_disponible, usuario_id)
            VALUES (@tipo_ficha_id, @cantidad, @usuario_id);
        END

        -- Devolver un mensaje de éxito
        SELECT 'Compra de fichas exitosa.';

    END
    ELSE
    BEGIN
        -- Devolver un mensaje de error si el usuario no tiene suficiente saldo en dólares
        SELECT 'Saldo insuficiente en dólares para comprar las fichas.';
    END
END;

------------------------------------------------Cambiar fichas a moneda usuario

CREATE PROCEDURE CambiarFichasAMonedaUsuario
    @usuario_id INT,
    @tipo_ficha_id INT,
    @moneda_destino NVARCHAR(100),
    @cantidad_fichas_a_convertir INT
AS
BEGIN
    -- Declarar variables para almacenar información
    DECLARE @valor_ficha DECIMAL(10, 2);
    DECLARE @saldo_usuario DECIMAL(10, 2);
    DECLARE @tasa_conversion DECIMAL(10, 5);
    DECLARE @cantidad_fichas_disponibles INT;
    DECLARE @moneda_actual NVARCHAR(100);

    -- Obtener el valor de la ficha desde la tabla TiposDeFichas
    SELECT @valor_ficha = valor
    FROM TiposDeFichas
    WHERE tipo_ficha_id = @tipo_ficha_id;

    -- Obtener la moneda actual de la cuenta bancaria del usuario
    SELECT @moneda_actual = m.nombre
    FROM Monedas m
    INNER JOIN CuentasBancarias cb ON m.moneda_id = cb.moneda_id
    WHERE cb.user_id = @usuario_id;

    -- Obtener el saldo actual de la cuenta bancaria del usuario en dólares
    SELECT @saldo_usuario = saldo
    FROM CuentasBancarias
    WHERE user_id = @usuario_id;

    -- Obtener la tasa de conversión para la moneda de destino
    SELECT @tasa_conversion = tasa
    FROM TasasDeCambio
    WHERE moneda_id = (SELECT moneda_id FROM Monedas WHERE nombre = @moneda_destino);

    -- Obtener la cantidad de fichas disponibles del tipo especificado
    SELECT @cantidad_fichas_disponibles = cantidad_disponible
    FROM Fichas
    WHERE usuario_id = @usuario_id AND tipo_ficha_id = @tipo_ficha_id;

    -- Verificar si el usuario tiene suficientes fichas para el cambio y que la moneda de destino no sea dólares
    IF @cantidad_fichas_disponibles >= @cantidad_fichas_a_convertir AND @moneda_destino <> 'Dólares'
    BEGIN
        -- Calcular el valor total en la moneda de destino
        DECLARE @valor_total_destino DECIMAL(10, 2);

        -- Calcular el valor total de las fichas en la moneda de destino
        SET @valor_total_destino = @valor_ficha * @cantidad_fichas_a_convertir;

        -- Realizar la conversión de moneda, si es necesario
        IF @moneda_actual <> @moneda_destino
        BEGIN
            SET @valor_total_destino = (@valor_ficha * @cantidad_fichas_a_convertir + @saldo_usuario) / @tasa_conversion;
        END

        -- Actualizar el saldo de la cuenta bancaria del usuario y cambiar el tipo de moneda
        UPDATE CuentasBancarias
        SET saldo = @valor_total_destino,
            moneda_id = (SELECT moneda_id FROM Monedas WHERE nombre = @moneda_destino)
        WHERE user_id = @usuario_id;

        -- Restar la cantidad de fichas convertidas al usuario
        UPDATE Fichas
        SET cantidad_disponible = @cantidad_fichas_disponibles - @cantidad_fichas_a_convertir
        WHERE usuario_id = @usuario_id AND tipo_ficha_id = @tipo_ficha_id;

        -- Devolver un mensaje de éxito
        SELECT 'Cambio de fichas exitoso. Nuevo saldo en ' + @moneda_destino + ': ' + CAST(@valor_total_destino AS NVARCHAR);
    END
    ELSE
    BEGIN
        -- Devolver un mensaje de error si el usuario no tiene suficientes fichas o si la moneda de destino es dólares.
        SELECT 'No tienes suficientes fichas para realizar el cambio o no puedes cambiar a dólares.';
    END
END;

------------------------------------------------Añadir saldo a cuenta bancaria

CREATE PROCEDURE AñadirSaldoACuentaBancaria
    @usuario_id INT,
    @moneda_id INT,
    @monto DECIMAL(10, 2)
AS
BEGIN
    -- Actualizar el saldo de la cuenta bancaria del usuario
    UPDATE CuentasBancarias
    SET saldo = saldo + @monto
    WHERE user_id = @usuario_id AND moneda_id = @moneda_id;

    -- Devolver un mensaje de éxito
    SELECT 'Saldo añadido con éxito. Nuevo saldo: ' + CAST(saldo AS NVARCHAR)
    FROM CuentasBancarias
    WHERE user_id = @usuario_id AND moneda_id = @moneda_id;
END;

--------------------------------------------------------------------------------------STORE PROCEDURE DESTINADOS A VALIDACION----------------------------------------------------------------------------------------

-- Procedimiento para verificar la elegibilidad para jugar

CREATE PROCEDURE VerificarElegibilidadParaJugar
    @usuario_id INT,
    @esElegible BIT OUTPUT
AS
BEGIN
    DECLARE @edad INT;
    DECLARE @adulto_asignado_id INT;

    -- Obtener la edad del usuario y el ID del adulto asignado
    SELECT @edad = edad, @adulto_asignado_id = adulto_asignado_id
    FROM Usuarios
    WHERE user_id = @usuario_id;

    -- Verificar si el usuario es mayor de edad (mayor o igual a 21 años) o si tiene un adulto asignado
    IF @edad >= 21 or @adulto_asignado_id != 0
    BEGIN
        -- Usuario elegible para jugar
        SET @esElegible = 1;
    END
    ELSE
    BEGIN
        -- Usuario no elegible para jugar
        SET @esElegible = 0;
    END;
END;
;

------------------------------------------------Verificar disponibilidad de fichas del usuario

CREATE PROCEDURE VerificarDisponibilidadFichas
    @usuario_id INT,
    @fichas_rojas INT,
    @fichas_amarillas INT,
    @fichas_verdes INT,
    @fichas_negras INT,
    @fichasEncontradas BIT OUTPUT
AS
BEGIN
    DECLARE @fichas_disponibles INT;
    SET @fichasEncontradas = 1;  -- Suponemos que hay suficientes fichas hasta que se demuestre lo contrario

    -- Verificar la disponibilidad de fichas rojas
    SELECT @fichas_disponibles = ISNULL(SUM(cantidad_disponible), 0)
    FROM Fichas
    WHERE usuario_id = @usuario_id AND tipo_ficha_id = 1;
    
    IF @fichas_rojas > @fichas_disponibles
    BEGIN
        -- No tienes suficientes fichas rojas en tu cuenta
        SET @fichasEncontradas = 0;
        RETURN;
    END

    -- Verificar la disponibilidad de fichas amarillas
    SELECT @fichas_disponibles = ISNULL(SUM(cantidad_disponible), 0)
    FROM Fichas
    WHERE usuario_id = @usuario_id AND tipo_ficha_id = 2;

    IF @fichas_amarillas > @fichas_disponibles
    BEGIN
        -- No tienes suficientes fichas amarillas en tu cuenta
        SET @fichasEncontradas = 0;
        RETURN;
    END

    -- Verificar la disponibilidad de fichas verdes
    SELECT @fichas_disponibles = ISNULL(SUM(cantidad_disponible), 0)
    FROM Fichas
    WHERE usuario_id = @usuario_id AND tipo_ficha_id = 3;

    IF @fichas_verdes > @fichas_disponibles
    BEGIN
        -- No tienes suficientes fichas verdes en tu cuenta
        SET @fichasEncontradas = 0;
        RETURN;
    END

    -- Verificar la disponibilidad de fichas negras
    SELECT @fichas_disponibles = ISNULL(SUM(cantidad_disponible), 0)
    FROM Fichas
    WHERE usuario_id = @usuario_id AND tipo_ficha_id = 4;

    IF @fichas_negras > @fichas_disponibles
    BEGIN
        -- No tienes suficientes fichas negras en tu cuenta
        SET @fichasEncontradas = 0;
        RETURN;
    END
END;

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

------------------------------------------------Verificar limite perdida en juego
CREATE PROCEDURE VerificarLimitePerdidaEnJuego
    @usuario_id INT,
    @juego_id INT,
    @limite_perdida DECIMAL(10, 2),
    @excedido BIT OUTPUT
AS
BEGIN
    -- Calcular la suma de las pérdidas del usuario en un juego específico
    DECLARE @perdidas DECIMAL(10, 2);
    SELECT @perdidas = ISNULL(SUM(cantidad_perdida), 0)
    FROM ApuestasUsuario
    WHERE usuario_id = @usuario_id AND juego_id = @juego_id;

    -- Verificar si se ha excedido el límite de pérdida
    IF @perdidas >= @limite_perdida
    BEGIN
        SET @excedido = 1; -- Límite de pérdida en el juego específico excedido
    END
    ELSE
    BEGIN
        IF @perdidas IS NULL
        BEGIN
            SET @excedido = 0; -- El usuario nunca ha jugado, el límite no se ha excedido
        END
        ELSE
        BEGIN
            SET @excedido = 0; -- Límite de pérdida en el juego específico no excedido
        END
    END
END;

------------------------------------------------Verificar limite ganancia en juego

CREATE PROCEDURE VerificarLimiteGanancia
    @usuario_id INT,
    @limite_ganancia DECIMAL(10, 2),
    @excedido BIT OUTPUT
AS
BEGIN
    -- Calcular la suma de las ganancias del usuario en todos los juegos
    DECLARE @ganancias DECIMAL(10, 2);
    SELECT @ganancias = ISNULL(SUM(cantidad_ganada), 0)
    FROM ApuestasUsuario
    WHERE usuario_id = @usuario_id;

    -- Verificar si se ha excedido el límite de ganancia
    IF @ganancias >= @limite_ganancia
    BEGIN
        SET @excedido = 1; -- Límite de ganancia excedido
    END
    ELSE
    BEGIN
        SET @excedido = 0; -- Límite de ganancia no excedido
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