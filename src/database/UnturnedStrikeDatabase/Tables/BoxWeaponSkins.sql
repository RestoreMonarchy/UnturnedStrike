CREATE TABLE [dbo].[BoxWeaponSkins]
(
	BoxId INT NOT NULL CONSTRAINT FK_BoxWeaponSkins_BoxId FOREIGN KEY REFERENCES dbo.Boxes(Id),
	WeaponSkinId INT NOT NULL CONSTRAINT FK_BoxWeaponSkins_WeaponSkinId FOREIGN KEY REFERENCES dbo.WeaponSkins(Id),
	Weight INT NOT NULL,
	CONSTRAINT PK_BoxWeaponSkins PRIMARY KEY (BoxId, WeaponSkinId)
);
