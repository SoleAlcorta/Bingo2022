CREATE TABLE [dbo].[HistorialCartones](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[fecha] DATETIME NOT NULL,
	[carton1] [int] NULL,
	[carton2] [int] NULL,
	[carton3] [int] NULL,
	[carton4] [int] NULL,
 CONSTRAINT [PK_HistorialCartones] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
