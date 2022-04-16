CREATE TABLE [dbo].[PlayerWeaponSkins]
(
	Id INT IDENTITY(1,1) NOT NULL,
	PlayerId CHAR(17) NOT NULL CONSTRAINT FK_PlayerWeaponSkins_PlayerId FOREIGN KEY REFERENCES dbo.Players(Id),
	WeaponSkinId INT NOT NULL CONSTRAINT FK_PlayerWeaponSkins_SkinId FOREIGN KEY REFERENCES dbo.WeaponSkins(Id),
	IsEquiped BIT NOT NULL CONSTRAINT DF_PlayerWeaponSkins_IsEquiped DEFAULT 0,
	CreateDate DATETIME2(0) NOT NULL CONSTRAINT DF_PlayerWeaponSkins_CreateDate DEFAULT GETUTCDATE(),
	CONSTRAINT PK_PlayerWeaponSkins PRIMARY KEY (Id)
)
