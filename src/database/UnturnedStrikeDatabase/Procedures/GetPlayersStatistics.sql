CREATE PROCEDURE dbo.GetPlayersStatistics @DateFrom DATE, @DateTo DATE, @Top INT, @OrderBy VARCHAR(255)
AS
BEGIN

	DECLARE @stmt NVARCHAR(max);
	DECLARE @params NVARCHAR(max) = N'@DateFrom DATE, @DateTo DATE, @Top INT';

	SET @stmt = 
	N'SELECT
		TOP (@Top)
		p.Id,
		p.SteamName,
		p.SteamIconUrl,
		p.Country,
		p.IsVip,
		SUM(pg.Kills) AS TotalKills,
		SUM(pg.Deaths) AS TotalDeaths,
		SUM(pg.Score) AS TotalScore,
		SUM(pg.BombsPlanted) AS TotalBombsPlanted,
		SUM(pg.BombsDefused) AS TotalBombsDefused,
		SUM(pg.HostagesRescued) AS TotalHostagesRescued,
		MAX(pg.Kills) AS BestKills,
		MAX(pg.Score) AS BestScore,
		MAX(pg.KD) AS BestKD,
		SUM(CASE WHEN (g.IsWinnerTerrorist = 1 AND pg.IsTerrorist = 1) OR (g.IsWinnerTerrorist = 0 AND pg.IsTerrorist = 0) THEN 1 ELSE 0 END) AS GamesWon,
		SUM(CASE WHEN (g.IsWinnerTerrorist = 1 AND pg.IsTerrorist = 0) OR (g.IsWinnerTerrorist = 0 AND pg.IsTerrorist = 1) THEN 1 ELSE 0 END) AS GamesLost
	FROM dbo.Players p 
	JOIN dbo.PlayerGameSummaries pg ON p.Id = pg.PlayerId
	JOIN dbo.GameSummaries g
	ON g.Id = pg.GameId
	WHERE g.StartDate BETWEEN @DateFrom AND @DateTo
	GROUP BY p.Id, p.SteamName, p.SteamIconUrl, p.Country, p.IsVip ORDER BY ' + @OrderBy + ' DESC';

	EXEC sys.sp_executesql @stmt, @params, @DateFrom = @DateFrom, @DateTo = @DateTo, @Top = @Top;
	
END;
GO
-- EXEC dbo.GetPlayersStatistics @DateFrom = '2020-01-01', @DateTo = '2020-12-01', @Top = 1, @OrderBy = 'TotalKills'
