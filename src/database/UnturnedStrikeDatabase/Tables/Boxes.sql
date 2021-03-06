CREATE TABLE dbo.Boxes
(
	Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Boxes PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL,
	Description NVARCHAR(500) NOT NULL,
	ImageFileId INT NULL CONSTRAINT FK_Boxes_ImageFileId FOREIGN KEY REFERENCES dbo.Files(Id),
	Price DECIMAL(9, 2) NOT NULL,
	IsEnabled BIT NOT NULL CONSTRAINT DF_Boxes_IsEnabled DEFAULT 1
)
