CREATE PROCEDURE dbo.PurchaseVIP
	@PlayerId CHAR(17),
	@Amount DECIMAL(9,2)
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	IF NOT EXISTS (SELECT * FROM dbo.Players WHERE Id = @PlayerId)
		RETURN 1;

	BEGIN TRAN;
	
		UPDATE dbo.Players 
		SET Balance -= @Amount
		WHERE Id = @PlayerId 
		AND Balance >= @Amount;

		IF @@ROWCOUNT = 0 
		BEGIN
			ROLLBACK;
			RETURN 2;
		END;

		UPDATE dbo.Players SET IsVIP = 1 WHERE Id = @PlayerId;

		INSERT INTO dbo.VIPPurchases (PlayerId, Amount) 
		OUTPUT inserted.Id, inserted.PlayerId, inserted.Amount, inserted.CreateDate
		VALUES (@PlayerId, @Amount);

	COMMIT; 

	RETURN 0;
END;
