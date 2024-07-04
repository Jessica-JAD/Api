USE IHSecurity
GO

DECLARE
    /*************************************************** OJO **************************************************/
    /*      Especificar BIEN el ID_DATOS segun sea el cliente. Se asume q sea uno asociado a Stocks           */
    /*      Ese se usará como plantilla para crear uno nuevo, pero con el nuevo modulo                        */
    @id_datos INT = 61
    /**********************************************************************************************************/

DECLARE
    @id_api_modulo INT = 30,
    @api_modulo VARCHAR(50) = 'Zun API Integracion',
    @id_api_operador INT,
    @api_operador VARCHAR(30) = 'APIUser',
    @api_operador_desc VARCHAR(50) = 'Usuario generico API de integración',
    @fecha DATETIME,
    @error_msg VARCHAR(100),
    @crlf VARCHAR(1) = CHAR(13),
    @error_number INT,
    @error_sql NVARCHAR(4000),
    @api_db VARCHAR(30) = 'ZunAPI'
BEGIN TRANSACTION
BEGIN TRY
    /******************************************
        Adicionar nuevo Módulo
    ******************************************/
    SET @error_msg = N'Error al añadir modulo ' + @api_modulo
    IF NOT EXISTS (SELECT * FROM modulo WHERE id_modulo = @id_api_modulo) BEGIN
        INSERT INTO modulo (id_modulo, desc_modulo, base_datos, conexion)
            VALUES (@id_api_modulo, @api_modulo, null, '')
    END
    ELSE BEGIN
        UPDATE modulo
            SET desc_modulo = @api_modulo
            WHERE id_modulo = @id_api_modulo
    END
    PRINT('Nuevo modulo actualizado' + @crlf)

    /******************************************
        Adicionar nuevo Usuario
    ******************************************/
    SET @error_msg = N'Error al añadir usuario ' + @api_operador
    IF NOT EXISTS (SELECT * FROM operador WHERE (UPPER(LTRIM(RTRIM(nombre))) = UPPER(LTRIM(RTRIM(@api_operador))))) BEGIN
        SET @Fecha = GETDATE()
        INSERT INTO operador (nombre, descripcion, fecha_creac, fecha_clave, cambio_clave, admin, activo)
            VALUES (@api_operador, @api_operador_desc, @Fecha, @Fecha, 0, 0, 1)
    END
    ELSE BEGIN
        UPDATE operador
            SET
                descripcion = @api_operador_desc,
                cambio_clave = 0,
                admin = 0,
                activo = 1
            WHERE 
                UPPER(LTRIM(RTRIM(nombre))) = UPPER(LTRIM(RTRIM(@api_operador)))
    END
    PRINT('Nuevo operador actualizado' + @crlf)

    /******************************************
        Adicionar Acceso
    ******************************************/
    SET @error_msg = N'Error al añadir acceso'
    SET @id_api_operador = (SELECT MAX(id_oper) FROM operador WHERE (UPPER(LTRIM(RTRIM(nombre))) = UPPER(LTRIM(RTRIM(@api_operador)))))
    IF NOT EXISTS(SELECT * FROM datos WHERE id_modulo = @id_api_modulo)
        IF EXISTS(SELECT * FROM datos WHERE id_datos = @id_datos AND id_modulo = 11) BEGIN
            INSERT INTO datos (id_juego, id_modulo, base_datos, origen_datos, password, login, servidor, descripcion, backup_antes, backup_desp, bloqueo)
                SELECT D.id_juego, @id_api_modulo, @api_db, D.origen_datos, D.password, D.login, D.servidor, @api_db + ' hacia ' + D.descripcion, NULL, NULL, 0
                    FROM datos D
                    WHERE D.id_datos = @id_datos;
        
        END
        ELSE BEGIN
            RAISERROR(N'El ID de Datos especificado no está relacionado con el módulo Stocks', 15, 1);
        END

    -- Busco el ID_datos existente
    SET @id_datos = (SELECT MAX(id_datos) FROM datos WHERE id_modulo = @id_api_modulo)

    INSERT INTO acceso (id_datos, id_oper)
        VALUES (@id_datos, @id_api_operador)

    PRINT('Nuevo juego de datos actualizado' + CHAR(13))

    COMMIT
END TRY

BEGIN CATCH
    ROLLBACK
    SET @error_number = ERROR_NUMBER()
    SET @error_sql = ERROR_MESSAGE()
    RAISERROR(N'%s. Se aborta la ejecución del script.%sError %d - %s', 10, 1, @error_msg, @crlf, @error_number, @error_sql);
END CATCH
GO
