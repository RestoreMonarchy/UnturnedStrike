CREATE TABLE [dbo].[Players]
(
	Id CHAR(17) NOT NULL CONSTRAINT PK_Players PRIMARY KEY,
	SteamName NVARCHAR(32) NULL,
	SteamIconUrl VARCHAR(255) NULL,
	Country CHAR(2) NULL,
	LastActivity DATETIME2(0) NOT NULL CONSTRAINT DF_Players_LastActivity DEFAULT GETUTCDATE(),
	CreateDate DATETIME2(0) NOT NULL CONSTRAINT DF_Players_CreateDate DEFAULT GETUTCDATE(),
	IsVIP BIT NOT NULL CONSTRAINT DF_Players_IsVIP DEFAULT 0,
	Balance DECIMAL(9, 2) NOT NULL CONSTRAINT DF_Players_Balance DEFAULT 0
)
