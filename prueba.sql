USE [Pruebas]
GO
/****** Object:  Table [dbo].[Pruebas_EMP]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pruebas_EMP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NombreEmpleado] [nvarchar](50) NOT NULL,
	[EdadEmpleado] [int] NOT NULL,
	[SexoEmpleado] [nvarchar](50) NOT NULL,
	[IdPuesto] [int] NOT NULL,
	[FotografiaEmpleado] [varbinary](max) NULL,
	[Extension] [nvarchar](50) NULL,
	[IdEstado] [int] NULL,
	[NumTelefono] [int] NULL,
	[Intentos] [int] NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [varbinary](max) NULL,
 CONSTRAINT [PK_Pruebas_EMP] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[Pruebas_TodosEmpleados]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <Create Date,,>
-- Description:	<Extraer la lista de todos los empleados>
-- =============================================
CREATE FUNCTION [dbo].[Pruebas_TodosEmpleados] 
(	
	
	
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT [Id]
      ,[NombreEmpleado]
      ,[EdadEmpleado]
      ,[SexoEmpleado]
      ,[IdPuesto]
      ,[FotografiaEmpleado]
      ,[Extension]
      ,[IdEstado]
      ,[NumTelefono]
      ,[Intentos]
      ,[Email]
  FROM [Pruebas].[dbo].[Pruebas_EMP]
  
)
GO
/****** Object:  Table [dbo].[Pruebas_PRS]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pruebas_PRS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NombrePuesto] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Prueba_PRS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pruebas_EstadoUsuario]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pruebas_EstadoUsuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Estado] [nvarchar](10) NULL,
 CONSTRAINT [PK_Pruebas_EstadoUsuario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[Pruebas_SoloUnEmpleado]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Pruebas_SoloUnEmpleado] 
(	
	@Id int
)
RETURNS TABLE 
AS
RETURN 
(
  SELECT [Id]
      ,[NombreEmpleado]
      ,[EdadEmpleado]
      ,[SexoEmpleado]
      ,[IdPuesto]
	  ,(Select [NombrePuesto] from [dbo].[Pruebas_PRS] Where [Id] = [IdPuesto]) Puesto
      ,[FotografiaEmpleado]
      ,[Extension]
      ,[IdEstado]
	  ,(Select [Estado] from [dbo].Pruebas_EstadoUsuario Where [Id] = [IdEstado]) Estado
      ,[NumTelefono]
      ,[Intentos]
      ,[Email]   
  FROM [Pruebas].[dbo].[Pruebas_EMP]
  WHERE [Id] = @Id

)
GO
/****** Object:  Table [dbo].[Pruebas_EmpleadosControlHoras]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pruebas_EmpleadosControlHoras](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RefIdEmpleado] [int] NOT NULL,
	[HoraEntrada] [time](7) NOT NULL,
	[HoraSalida] [time](7) NULL,
	[Fecha] [date] NOT NULL,
 CONSTRAINT [PK_Pruebas_EmpleadosControlHoras] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[Pruebas_CotrolHoras]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[Pruebas_CotrolHoras] 
(	
	@IdEmpleado int,
	@Desde date,
	@Hasta date
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT [Id]
      ,[RefIdEmpleado]
      ,[HoraEntrada] 
      ,[HoraSalida]
      ,[Fecha]
  FROM [dbo].[Pruebas_EmpleadosControlHoras]
  WHERE [RefIdEmpleado] = @IdEmpleado and [Fecha] BETWEEN @Desde and @Hasta
)
GO
/****** Object:  UserDefinedFunction [dbo].[Pruebas_TraerPuestos]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <Create Date,,>
-- Description:	<Traer todos los puestos>
-- =============================================
CREATE FUNCTION [dbo].[Pruebas_TraerPuestos] 
(	

)
RETURNS TABLE 
AS
RETURN 
(
	SELECT [Id]
      ,[NombrePuesto]
  FROM [dbo].[Pruebas_PRS]
)
GO
/****** Object:  UserDefinedFunction [dbo].[Pruebas_FN_TodosEmpleados]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[Pruebas_FN_TodosEmpleados]
(	

)
RETURNS TABLE 
AS
RETURN 
(
		SELECT [Id]
      ,[NombreEmpleado]
      ,[EdadEmpleado]
      ,[SexoEmpleado]
      ,[IdPuesto]
	  ,(Select [NombrePuesto] from [dbo].[Pruebas_PRS] Where [Id] = [IdPuesto]) Puesto
      ,[FotografiaEmpleado]
      ,[Extension]
      ,[IdEstado]

      ,[NumTelefono]
      ,[Intentos]
      ,[Email]   
  FROM [Pruebas].[dbo].[Pruebas_EMP]
  
)
GO
/****** Object:  UserDefinedFunction [dbo].[Pruebas_FN_ImagenUnEmpleado]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[Pruebas_FN_ImagenUnEmpleado] 
(	
	@Id int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
      [FotografiaEmpleado],
	  [Extension]
  FROM [Pruebas].[dbo].[Pruebas_EMP] 
  WHERE [Id] = @Id
)
GO
/****** Object:  UserDefinedFunction [dbo].[Pruebas_FN_ControlHoras]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[Pruebas_FN_ControlHoras]
(	
	@IdEmpleado int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT [Id]
      ,[RefIdEmpleado]
      ,[HoraEntrada] 
      ,[HoraSalida]
      ,[Fecha]
  FROM [dbo].[Pruebas_EmpleadosControlHoras]
  WHERE [RefIdEmpleado] = @IdEmpleado
)
GO
/****** Object:  UserDefinedFunction [dbo].[Pruebas_FN_Login_User]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <28-02-2024>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[Pruebas_FN_Login_User] 
(	
	@Email nvarchar(50),
	@Password text
)
RETURNS TABLE 
AS
RETURN 
(
  SELECT [Password], [Email] FROM [dbo].[Pruebas_EMP] WHERE [Email]=@Email
)
GO
/****** Object:  UserDefinedFunction [dbo].[Pruebas_FN_TraerEstado]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <01-03-2024>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[Pruebas_FN_TraerEstado] 
(	

)
RETURNS TABLE 
AS
RETURN 
(
	SELECT [Id]
      ,[Estado]
	FROM [dbo].[Pruebas_EstadoUsuario]
)
GO
/****** Object:  Table [dbo].[Pruebas_EmailsLog]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pruebas_EmailsLog](
	[Fecha] [datetime] NOT NULL,
	[Destinatario] [nvarchar](50) NOT NULL,
	[Asunto] [nvarchar](250) NOT NULL,
	[Mensaje] [nvarchar](max) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pruebas_SolicitarToken_CambioPassword]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pruebas_SolicitarToken_CambioPassword](
	[RefUsuarios] [int] NOT NULL,
	[Dispositivo] [nvarchar](50) NULL,
	[Token] [nvarchar](max) NULL,
	[Key] [nvarchar](50) NULL,
	[FechaSolicitud] [datetime] NOT NULL,
	[FechaExpiracion] [datetime] NOT NULL,
	[Estado] [int] NULL,
	[EmailSend] [int] NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Pruebas_EMP] ADD  CONSTRAINT [DF_Pruebas_EMP_Intentos]  DEFAULT ((0)) FOR [Intentos]
GO
ALTER TABLE [dbo].[Pruebas_EMP]  WITH CHECK ADD  CONSTRAINT [FK_Pruebas_EMP_Pruebas_EstadoUsuario] FOREIGN KEY([IdEstado])
REFERENCES [dbo].[Pruebas_EstadoUsuario] ([Id])
GO
ALTER TABLE [dbo].[Pruebas_EMP] CHECK CONSTRAINT [FK_Pruebas_EMP_Pruebas_EstadoUsuario]
GO
ALTER TABLE [dbo].[Pruebas_EMP]  WITH CHECK ADD  CONSTRAINT [FK_Pruebas_EMP_Pruebas_PRS] FOREIGN KEY([IdPuesto])
REFERENCES [dbo].[Pruebas_PRS] ([Id])
GO
ALTER TABLE [dbo].[Pruebas_EMP] CHECK CONSTRAINT [FK_Pruebas_EMP_Pruebas_PRS]
GO
ALTER TABLE [dbo].[Pruebas_EmpleadosControlHoras]  WITH CHECK ADD  CONSTRAINT [FK_Pruebas_EmpleadosControlHoras_Pruebas_EMP] FOREIGN KEY([RefIdEmpleado])
REFERENCES [dbo].[Pruebas_EMP] ([Id])
GO
ALTER TABLE [dbo].[Pruebas_EmpleadosControlHoras] CHECK CONSTRAINT [FK_Pruebas_EmpleadosControlHoras_Pruebas_EMP]
GO
ALTER TABLE [dbo].[Pruebas_SolicitarToken_CambioPassword]  WITH CHECK ADD  CONSTRAINT [FK_Pruebas_SolicitarToken_CambioPassword_Pruebas_EMP] FOREIGN KEY([RefUsuarios])
REFERENCES [dbo].[Pruebas_EMP] ([Id])
GO
ALTER TABLE [dbo].[Pruebas_SolicitarToken_CambioPassword] CHECK CONSTRAINT [FK_Pruebas_SolicitarToken_CambioPassword_Pruebas_EMP]
GO
/****** Object:  StoredProcedure [dbo].[Pruebas_CrearEmpleado]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <Create Date,,>
-- Description:	<Para crear nuevos usuarios>
-- =============================================
CREATE PROCEDURE [dbo].[Pruebas_CrearEmpleado] 
	@NombreEmpleado nvarchar(50),
	@EdadEmpleado int,
	@SexoEmpleado nvarchar(50),
	@IdPuesto int,
	@FotografiaEmpleado varbinary(max)
AS
BEGIN
	
	INSERT INTO [dbo].[Pruebas_EMP]
           ([NombreEmpleado]
           ,[EdadEmpleado]
           ,[SexoEmpleado]
           ,[IdPuesto]
		   ,[FotografiaEmpleado])
     VALUES
           (@NombreEmpleado,
            @EdadEmpleado,
            @SexoEmpleado,
            @IdPuesto,
			@FotografiaEmpleado)

END
GO
/****** Object:  StoredProcedure [dbo].[Pruebas_EditarEmpleado]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <Create Date,,>
-- Description:	<Editar empleados>
-- =============================================
CREATE PROCEDURE [dbo].[Pruebas_EditarEmpleado] 
	@Id int,
	@NombreEmpleado nvarchar(50),
	@EdadEmpleado int,
	@NumTelefono int,
	@SexoEmpleado nvarchar(50),
	@Email nvarchar(50),
	@IdPuesto int,
	@IdEstado int,
	@FotografiaEmpleado varbinary(max),
	@Extension nvarchar(50),
	@Mensaje nvarchar(50) OUTPUT,
	@Status bit OUTPUT
AS
BEGIN
	UPDATE [dbo].[Pruebas_EMP]
			SET [NombreEmpleado] = @NombreEmpleado,
				[EdadEmpleado] = @EdadEmpleado,
				[SexoEmpleado] = @SexoEmpleado,
				[IdPuesto] = @IdPuesto,
				[FotografiaEmpleado] = @FotografiaEmpleado,
				[Extension] = @Extension,
				[IdEstado] = @IdEstado,
				[NumTelefono] = @NumTelefono,
				[Email] = @Email
			WHERE [Id] = @Id
		SET @Status = 1
		SET @Mensaje = 'Usuario actualizado con éxito.'
	
END
GO
/****** Object:  StoredProcedure [dbo].[Pruebas_EliminarEmpleado]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <Create Date,,>
-- Description:	<Eliminar a un empleado>
-- =============================================
CREATE PROCEDURE [dbo].[Pruebas_EliminarEmpleado] 
	@Id int 
AS
BEGIN
	DELETE FROM [dbo].[Pruebas_EMP]
      WHERE [Id] = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[Pruebas_P_Actualizar_Estado_Token]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Pruebas_P_Actualizar_Estado_Token] 
	@Email nvarchar(50)
AS
BEGIN
   Declare @IdUser int = (Select [Id] From [dbo].[Pruebas_EMP] Where [Email] = @Email)
	UPDATE [dbo].[Pruebas_SolicitarToken_CambioPassword]
   SET 
      [Estado] = 1
 WHERE [RefUsuarios] = @IdUser
END
GO
/****** Object:  StoredProcedure [dbo].[Pruebas_P_CrearEmpleado]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <01-03-2024>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Pruebas_P_CrearEmpleado] 
	@NombreEmpleado		nvarchar(50),
    @EdadEmpleado		int,
    @SexoEmpleado		nvarchar(50),
    @Password			varbinary(max),
    @NumTelefono		int,
    @Email				nvarchar(50),
	@IdPuesto			int,
	@IdEstado			int,
	@FotografiaEmpleado varbinary(max),
	@Mensaje			nvarchar(50) OUTPUT,
	@Status				bit OUTPUT
AS
BEGIN
	IF(EXISTS(SELECT * FROM [dbo].[Pruebas_EMP] WHERE [NombreEmpleado] = @NombreEmpleado or [Email]=@Email or [NumTelefono]=@NumTelefono))
	BEGIN
		SET @Status = 0
		SET @Mensaje = 'Ya existe un usuario con estas características.'
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[Pruebas_EMP]
           ([NombreEmpleado]
           ,[EdadEmpleado]
           ,[SexoEmpleado]
           ,[IdPuesto]
           ,[IdEstado]
           ,[Password]
           ,[NumTelefono]
           ,[Intentos]
           ,[Email]
		   ,[FotografiaEmpleado])
     VALUES
           (@NombreEmpleado,		
			@EdadEmpleado,		
			@SexoEmpleado,		
			@IdPuesto,					
			@IdEstado,			
			@Password,			
			@NumTelefono,		
			0,			
			@Email,
			@FotografiaEmpleado)	
			SET @Status = 1
			SET @Mensaje = 'Usuario creado con éxito.'
	END	
END
GO
/****** Object:  StoredProcedure [dbo].[Pruebas_P_Intentos_Usuario]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <28-02-2024>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Pruebas_P_Intentos_Usuario]
	@Intentos int,
	@Email nvarchar(50),
	@Estado int
AS
BEGIN
	IF(@Intentos != 0)
	BEGIN
	UPDATE [dbo].[Pruebas_EMP]
		SET 
			  [Intentos] = @Intentos,
			  [IdEstado] = @Estado
		WHERE [Email] = @Email
	END
	ELSE
	BEGIN
		UPDATE [dbo].[Pruebas_EMP]
		SET 
			  [Intentos] = 0,
			  [IdEstado] = @Estado
		WHERE [Email] = @Email
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Pruebas_P_Recuperar_Cuenta]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <28-02-2024>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Pruebas_P_Recuperar_Cuenta] 
	@Email nvarchar(50),
	@Password varbinary(MAX),
	@NumTelefono int,
	@Mensaje nvarchar(50) OUTPUT,
	@Status bit OUTPUT
AS
BEGIN
	IF (EXISTS((SELECT [Email] FROM [dbo].[Pruebas_EMP] WHERE [Email] = @Email)) and @Password is not null )
	BEGIN
	UPDATE [dbo].[Pruebas_EMP]
		   SET 
				[IdEstado] = 1
			  ,[Password] = @Password
			  ,[Intentos] = 0
			WHERE [Email] = @Email
			SET @Status = 1
			SET @Mensaje = 'Correo recuperado con éxito.'
	END
	ELSE IF (EXISTS((SELECT [NumTelefono] FROM [dbo].[Pruebas_EMP] WHERE [NumTelefono] = @NumTelefono)) and @Password is not null)
	BEGIN
			UPDATE [dbo].[Pruebas_EMP]
	   SET 
			[IdEstado] = 1
		  ,[Password] = @Password
		  ,[Intentos] = 0
		WHERE [NumTelefono] = @NumTelefono
		SET @Status = 1
		SET @Mensaje = 'Correo recuperado con éxito.'
	END
	ELSE
	BEGIN
		SET @Status = 0
		SET @Mensaje = 'No se pudo recuperar su correo.'
	END
	
END
GO
/****** Object:  StoredProcedure [dbo].[Pruebas_P_Regristrar_Usuario_Login]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <29-02-2024>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Pruebas_P_Regristrar_Usuario_Login] 
	@NombreEmpleado		nvarchar(50),
    @EdadEmpleado		int,
    @SexoEmpleado		nvarchar(50),
    @Password			varbinary(MAX),
    @NumTelefono		int,
    @Email				nvarchar(50),
	@Mensaje			nvarchar(50) OUTPUT,
	@Status				bit OUTPUT
AS
BEGIN
	IF(EXISTS(SELECT * FROM [dbo].[Pruebas_EMP] WHERE [NombreEmpleado] = @NombreEmpleado or [Email]=@Email or [NumTelefono]=@NumTelefono))
	BEGIN
		SET @Status = 0
		SET @Mensaje = 'Ya existe un usuario con estas características.'
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[Pruebas_EMP]
           ([NombreEmpleado]
           ,[EdadEmpleado]
           ,[SexoEmpleado]
           ,[IdPuesto]
           ,[IdEstado]
           ,[Password]
           ,[NumTelefono]
           ,[Intentos]
           ,[Email])
     VALUES
           (@NombreEmpleado,		
			@EdadEmpleado,		
			@SexoEmpleado,		
			(SELECT [Id] FROM [dbo].[Pruebas_PRS] WHERE [NombrePuesto] = 'Nuevo Empleado'),					
			1,			
			@Password,			
			@NumTelefono,		
			0,			
			@Email)	
			SET @Status = 1
			SET @Mensaje = 'Usuario creado con éxito.'
	END	
	END			
GO
/****** Object:  StoredProcedure [dbo].[Pruebas_P_Set_EmailsLog]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <06-03-2024>
-- Description:	<Inserta los datos a la tabla EmailsLog>
-- =============================================
CREATE PROCEDURE [dbo].[Pruebas_P_Set_EmailsLog] 
	@Destinatario nvarchar(50),
	@Asunto nvarchar(250),
	@Mensaje nvarchar(MAX)
AS
BEGIN
	INSERT INTO [dbo].[Pruebas_EmailsLog]
           ([Fecha]
           ,[Destinatario]
           ,[Asunto]
		   ,[Mensaje])
     VALUES
           (GETDATE(),
		    @Destinatario,
			@Asunto,
			@Mensaje)
END
GO
/****** Object:  StoredProcedure [dbo].[Pruebas_P_SolicitarToken_CambioPassword]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac Luque>
-- Create date: <06-03-2024>
-- Description:	<Inserta el token del usuario que quiere hacer el cambio de contraseña.>
-- =============================================
CREATE PROCEDURE [dbo].[Pruebas_P_SolicitarToken_CambioPassword] 
	@Email nvarchar(50),
	@Dispositivo nvarchar(50),
	@Token text,
	@Key nvarchar(50),
	@FechaSolicitud datetime,
	@FechaExpiracion datetime,
	@EmailSend int,
	@Mensaje nvarchar(50) OUTPUT,
	@Status bit OUTPUT
AS
BEGIN
	

	IF (EXISTS(SELECT [Email] FROM [dbo].[Pruebas_EMP] WHERE [Email] = @Email))
	BEGIN
		Declare @id int = (SELECT [Id] FROM [dbo].[Pruebas_EMP] WHERE [Email] = @Email)
		INSERT INTO [dbo].[Pruebas_SolicitarToken_CambioPassword]
           ([RefUsuarios]
           ,[Dispositivo]
           ,[Token]
           ,[Key]
           ,[FechaSolicitud]
           ,[FechaExpiracion]
           ,[Estado]
           ,[EmailSend])
     VALUES
           (
		   @id,
		   @Dispositivo,
		   @Token,
		   @Key,
		   @FechaSolicitud,
		   @FechaSolicitud,
		   0,
		   @EmailSend
		   )
		   SET @Status = 1
		   SET @Mensaje = 'Token enviado a su correo electronico.'
	END
	ELSE
	BEGIN
	 SET @Status = 0
     SET @Mensaje = 'Sucedio un problema al enviar el Token.'
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Pruebas_SP_EditarEmpleado]    Script Date: 07/03/2024 16:45:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Isaac David Luque Medina>
-- Create date: <2024-01-06>
-- Description:	<Actualiza los datos de un solo empleado.>
-- =============================================
CREATE PROCEDURE [dbo].[Pruebas_SP_EditarEmpleado] 
	@Id int,
	@NombreEmpleado nvarchar(50),
	@EdadEmpleado int,
	@NumTelefono int,
	@SexoEmpleado nvarchar(50),
	@Email nvarchar(50),
	@IdPuesto int,
	@IdEstado int,
	@FotografiaEmpleado varbinary(max),
	@Extension nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

 UPDATE [dbo].[Pruebas_EMP]
			SET [NombreEmpleado] = @NombreEmpleado,
				[EdadEmpleado] = @EdadEmpleado,
				[SexoEmpleado] = @SexoEmpleado,
				[IdPuesto] = @IdPuesto,
				[FotografiaEmpleado] = @FotografiaEmpleado,
				[Extension] = @Extension,
				[IdEstado] = @IdEstado,
				[NumTelefono] = @NumTelefono,
				[Email] = @Email
			WHERE [Id] = @Id
END
GO
