CREATE PROCEDURE spEmpleado 
(  
    @EmpleadoId INT = NULL,  
    @Nombre VARCHAR(20) = NULL,  
    @Ciudad VARCHAR(20) = NULL,
    @ActionType VARCHAR(25)  
)  
AS  
BEGIN  
    IF @ActionType = 'GuardarDato'  
    BEGIN  
        IF NOT EXISTS (SELECT * FROM tblEmpleado WHERE EmpleadoId=@EmpleadoId)  
        BEGIN  
            INSERT INTO tblEmpleado (Nombre,Ciudad)  
            VALUES (@Nombre,@Ciudad)  
        END  
        ELSE  
        BEGIN  
            UPDATE tblEmpleado SET Nombre=@Nombre,Ciudad=@Ciudad WHERE EmpleadoId=@EmpleadoId  
        END  
    END  
    IF @ActionType = 'EliminarDato'  
    BEGIN  
        DELETE tblEmpleado WHERE EmpleadoId=@EmpleadoId  
    END  
    IF @ActionType = 'MostrarDato'  
    BEGIN  
        SELECT EmpleadoId AS EmpId,Nombre,Ciudad FROM tblEmpleado  
    END  
    IF @ActionType = 'MostrarRegistro'  
    BEGIN  
        SELECT EmpleadoId AS EmpId,Nombre,Ciudad FROM tblEmpleado   
        WHERE EmpleadoId=@EmpleadoId  
    END  
END  
GO
