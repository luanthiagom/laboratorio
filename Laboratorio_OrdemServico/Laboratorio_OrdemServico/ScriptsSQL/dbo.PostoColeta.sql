CREATE TABLE [dbo].[PostoColeta]
(
	[PostoColetaId] INT NOT NULL PRIMARY KEY, 
    [Descricao] NVARCHAR(100) NOT NULL, 
    [Endereco] NVARCHAR(MAX) NULL
)
