USE [DatabaseAgenti]
GO
/****** Object:  Table [dbo].[TabellaIntermedia]    Script Date: 01/03/2024 17:55:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TabellaIntermedia](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDAnagrafica] [int] NOT NULL,
	[IDVerbale] [int] NOT NULL,
	[IDViolazione] [int] NOT NULL,
 CONSTRAINT [PK_TabellaIntermedia] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TabellaIntermedia]  WITH CHECK ADD  CONSTRAINT [FK_TabellaIntermedia_ANAGRAFE1] FOREIGN KEY([IDAnagrafica])
REFERENCES [dbo].[ANAGRAFE] ([IDAnagrafica])
GO
ALTER TABLE [dbo].[TabellaIntermedia] CHECK CONSTRAINT [FK_TabellaIntermedia_ANAGRAFE1]
GO
ALTER TABLE [dbo].[TabellaIntermedia]  WITH CHECK ADD  CONSTRAINT [FK_TabellaIntermedia_VERBALE1] FOREIGN KEY([IDVerbale])
REFERENCES [dbo].[VERBALE] ([IDVerbale])
GO
ALTER TABLE [dbo].[TabellaIntermedia] CHECK CONSTRAINT [FK_TabellaIntermedia_VERBALE1]
GO
ALTER TABLE [dbo].[TabellaIntermedia]  WITH CHECK ADD  CONSTRAINT [FK_TabellaIntermedia_VIOLAZIONE1] FOREIGN KEY([IDViolazione])
REFERENCES [dbo].[VIOLAZIONE] ([IDViolazione])
GO
ALTER TABLE [dbo].[TabellaIntermedia] CHECK CONSTRAINT [FK_TabellaIntermedia_VIOLAZIONE1]
GO
