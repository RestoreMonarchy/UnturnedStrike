CREATE PROCEDURE [dbo].[ToggleEquipWeaponSkin]
	@playerWeaponSkinId INT,
	@playerId CHAR(17)
AS
BEGIN
	DECLARE @weaponId INT;
	DECLARE @isEquiped BIT;
	DECLARE @ownerId CHAR(17);

	SELECT
		@weaponId = ws.WeaponId,
		@isEquiped = pws.IsEquiped,
		@ownerId = pws.PlayerId 
	FROM dbo.PlayerWeaponSkins pws 
	JOIN dbo.WeaponSkins ws ON ws.Id = pws.WeaponSkinId
	WHERE pws.Id = @playerWeaponSkinId;

	-- Skin does not exist
	IF (@weaponId = NULL)
		RETURN 1;

	-- Player is not owner of a skin
	IF (@ownerId != @playerId)
		RETURN 2;
			
	BEGIN TRAN;

	IF (@isEquiped = 0) 
	BEGIN
		UPDATE pws
		SET pws.IsEquiped = 0 
		FROM dbo.PlayerWeaponSkins pws 
			JOIN dbo.WeaponSkins ws ON ws.Id = pws.WeaponSkinId
		WHERE ws.WeaponId = @weaponId AND pws.PlayerId = @playerId
		AND pws.IsEquiped = 1;
	END

	UPDATE dbo.PlayerWeaponSkins SET IsEquiped = 1 - IsEquiped WHERE Id = @playerWeaponSkinId;
	
	COMMIT;

	RETURN 0;
END