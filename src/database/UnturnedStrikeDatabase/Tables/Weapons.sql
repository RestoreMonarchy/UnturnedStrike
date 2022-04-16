﻿CREATE TABLE [dbo].[Weapons]
(
	Id INT IDENTITY(1, 1) NOT NULL,
	ItemId INT NOT NULL,
	Name NVARCHAR(255) NOT NULL,
	Description NVARCHAR(500) NOT NULL,
	ImageFileId INT NULL CONSTRAINT FK_Weapons_ImageFileId FOREIGN KEY REFERENCES dbo.Files(Id),
	Category VARCHAR(255) NOT NULL,
	Team VARCHAR(255) NOT NULL CONSTRAINT DF_Weapons_Team DEFAULT 'Both',
	Price INT NOT NULL CONSTRAINT DF_Weapons_Price DEFAULT 0,
	KillRewardMultiplier DECIMAL(9,2) NOT NULL CONSTRAINT DF_Weapons_KillRewardMultiplier DEFAULT 1,
	MagazineId INT NULL,
	MagazineAmount INT NOT NULL CONSTRAINT DF_Weapons_MagazineAmount DEFAULT 0,
	IconUnicode VARCHAR(255) NULL,
	IsEnabled BIT NOT NULL CONSTRAINT DF_Weapons_IsEnabled DEFAULT 1,
	CONSTRAINT PK_Weapons PRIMARY KEY (Id)
)