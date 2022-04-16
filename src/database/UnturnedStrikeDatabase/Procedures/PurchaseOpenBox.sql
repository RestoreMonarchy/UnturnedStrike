CREATE PROCEDURE dbo.PurchaseOpenBox
	@BoxId INT,
	@PlayerId CHAR(17)
AS
BEGIN
	
	DECLARE @boxWeaponSkins TABLE (WeaponSkinId INT, StartNum INT, EndNum INT);
	DECLARE @combinations INT, @rand INT, @selectedWeaponSkinId INT;
	DECLARE @playerWeaponSkinId INT, @price DECIMAL(9,2);

	IF NOT EXISTS (SELECT * FROM dbo.Players WHERE Id = @PlayerId)
		RETURN 1;

	SELECT TOP (1) @price = Price FROM dbo.Boxes b JOIN dbo.BoxWeaponSkins w ON w.BoxId = b.Id WHERE b.Id = @BoxId

	IF @price IS NULL
		RETURN 2;

	WITH CTE_BoxWeaponSkins AS (
		SELECT *, rownum = ROW_NUMBER() OVER(ORDER BY WeaponSkinId) FROM dbo.BoxWeaponSkins WHERE BoxId = @BoxId
	),
	CTE_recursive AS (
		SELECT rownum, WeaponSkinId, StartNum = 1, EndNum = Weight FROM CTE_BoxWeaponSkins WHERE rownum = 1
		UNION ALL 
		SELECT b.rownum, b.WeaponSkinId, c.EndNum + 1, c.EndNum + 1 + b.Weight
		FROM CTE_recursive c
			JOIN CTE_BoxWeaponSkins b ON b.rownum = c.rownum + 1
	)
	INSERT INTO @boxWeaponSkins
	SELECT WeaponSkinId, StartNum, EndNum 
	FROM CTE_recursive;
	
	SELECT @combinations = MAX(EndNum) - 1 FROM @boxWeaponSkins;

	SET @rand = FLOOR(RAND() * @combinations + 1);

	SELECT @selectedWeaponSkinId = WeaponSkinId FROM @boxWeaponSkins WHERE @rand BETWEEN StartNum AND EndNum;
		
	BEGIN TRAN;
		
		UPDATE dbo.Players 
		SET Balance -= @price
		WHERE Id = @PlayerId 
		AND Balance >= @price;

		IF @@ROWCOUNT = 0 
		BEGIN
			ROLLBACK;
			RETURN 3;
		END;

		INSERT INTO dbo.PlayerWeaponSkins (PlayerId, WeaponSkinId) VALUES (@PlayerId, @selectedWeaponSkinId);

		SELECT @playerWeaponSkinId = SCOPE_IDENTITY();
		INSERT INTO dbo.BoxPurchases (BoxId, PlayerId, PlayerWeaponSkinId, Amount) 
		OUTPUT inserted.Id 
		VALUES (@BoxId, @PlayerId, @playerWeaponSkinId, @price);
		
	COMMIT;

	RETURN 0
END
