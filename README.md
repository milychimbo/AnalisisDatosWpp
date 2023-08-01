# AnalisisDatosWpp

Este proyecto API con .NET 7.0 sirve para transformar los chats de whatsapp a datos para SQL Server para que posterior a ello puedas realizar análisis de datos. 

## Prerequisitos 

1. Clona este repositorio.
2. Descarga los chats de whatsapp a través del botón Exportar Chat, debes guardarlo sin archivos y esto te devuelve un .txt
3. Crea una base datos y configurala en los appsettings.json

## Uso

Debes ejecutar el proyecto, lo que te abrirá el Swagger y hay un único request donde puedes subir tu archivo txt.

## Estructura de Chat

La tabla que necesitas en tu base de datos es la siguiente:

CREATE TABLE [dbo].[Chat](
	[Fecha] [DATETIME2](7) NULL,
	[Persona] [NVARCHAR](30) NULL,
	[Mensaje] [NVARCHAR](MAX) NULL
)

## Contacto

Puedes contactarme en chimboemily@gmail.com

