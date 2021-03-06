CREATE TABLE [dbo].[VIPPurchases]
(
	Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_VIPPurchases PRIMARY KEY,
	PlayerId CHAR(17) NOT NULL CONSTRAINT FK_VIPPurchases_PlayerId FOREIGN KEY REFERENCES dbo.Players(Id),
	Amount DECIMAL(9, 2) NOT NULL,
	CreateDate DATETIME2 NOT NULL CONSTRAINT DF_VIPPurhcases_CreateDate DEFAULT GETUTCDATE()
)
