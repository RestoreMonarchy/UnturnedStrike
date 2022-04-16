﻿CREATE TABLE [dbo].[Files]
(
	Id INT IDENTITY(1,1) NOT NULL,
	Name NVARCHAR(255) NOT NULL,
	MimeType VARCHAR(255) NOT NULL,
	Data VARBINARY(MAX) NOT NULL,
	Size BIGINT NOT NULL,
	CreateDate DATETIME2(0) NOT NULL CONSTRAINT DF_Files_CreateDate DEFAULT GETUTCDATE(),
	CONSTRAINT PK_Files PRIMARY KEY (Id)
)
