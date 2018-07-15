USE STARS 

IF EXISTS (SELECT index_id FROM sys.indexes
	WHERE name = 'idx_sysadduser' AND object_id = OBJECT_ID(N'dbo.Trans'))
BEGIN
	DROP INDEX idx_sysadduser ON dbo.Trans
END

IF EXISTS (SELECT index_id FROM sys.indexes
	WHERE name = 'idx_TransactionID' AND object_id = OBJECT_ID(N'dbo.Trans'))
BEGIN
	DROP INDEX idx_TransactionID ON dbo.Trans
END

CREATE INDEX idx_sysadduser ON dbo.Trans (sysadduser);

CREATE INDEX idx_Transaction ON dbo.Trans (TransactionID);