CREATE TABLE [dbo].[PlayerGameSummaries]
(
	GameId INT NOT NULL CONSTRAINT FK_PlayerGameSummaries_GameId FOREIGN KEY REFERENCES dbo.GameSummaries(Id),
	PlayerId CHAR(17) NOT NULL CONSTRAINT FK_PlayerGameSummaries_PlayerId FOREIGN KEY REFERENCES dbo.Players(Id),
	IsTerrorist BIT NOT NULL,
	Kills SMALLINT NOT NULL,
	Deaths SMALLINT NOT NULL,
	BombsPlanted TINYINT NOT NULL,
	BombsDefused TINYINT NOT NULL,
	HostagesRescued TINYINT NOT NULL,
	MVPs TINYINT NOT NULL,
	KD AS CAST(Kills / CAST(ISNULL(NULLIF(Deaths, 0), 1) AS DECIMAL) AS DECIMAL(9,2)),
	Score SMALLINT NOT NULL,
	CONSTRAINT PK_PlayerGameSummaries PRIMARY KEY (GameId, PlayerId)
)