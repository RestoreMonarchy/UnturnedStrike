CREATE VIEW dbo.PlayerStatistics
AS
SELECT
	PlayerId,
	SUM(Kills) AS TotalKills,
	SUM(Deaths) AS TotalDeaths,
	SUM(Score) AS TotalScore,
	SUM(BombsPlanted) AS TotalBombsPlanted,
	SUM(BombsDefused) AS TotalBombsDefused,
	MAX(Kills) AS BestKills,
	MAX(Score) AS BestScore,
	MAX(KD) AS BestKD,
	SUM(CASE WHEN (IsWinnerTerrorist = 1 AND IsTerrorist = 1) OR (IsWinnerTerrorist = 0 AND IsTerrorist = 0) THEN 1 ELSE 0 END) AS GamesWon,
	SUM(CASE WHEN (IsWinnerTerrorist = 1 AND IsTerrorist = 0) OR (IsWinnerTerrorist = 0 AND IsTerrorist = 1) THEN 1 ELSE 0 END) AS GamesLost
FROM dbo.PlayerGameSummaries pg
JOIN dbo.GameSummaries g
ON g.Id = pg.GameId
GROUP BY PlayerId;