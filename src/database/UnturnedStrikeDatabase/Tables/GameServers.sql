﻿CREATE TABLE [dbo].[GameServers]
(
	Id INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_GameServers PRIMARY KEY,
	Name VARCHAR(55) NOT NULL,
	Address VARCHAR(255) NOT NULL,
	Port INT NOT NULL,
	CreateDate DATETIME2(0) NOT NULL CONSTRAINT DF_GameServers_CreateDate DEFAULT GETUTCDATE()
)
