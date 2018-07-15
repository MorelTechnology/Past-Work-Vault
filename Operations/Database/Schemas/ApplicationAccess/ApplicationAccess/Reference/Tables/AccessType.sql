/* 
 * TABLE: AccessType 
 */

CREATE TABLE Reference.AccessType(
    AccessTypeID      int             IDENTITY(1,1),
    ApplicationID     int             NOT NULL,
    AccessTypeName    varchar(20)     NOT NULL,
    AccessTypeDesc    varchar(100)    NULL,
    CONSTRAINT PK_AccessType PRIMARY KEY CLUSTERED (AccessTypeID, ApplicationID),
    CONSTRAINT UQ_AccessType_AccessTypeName_ApplicationID  UNIQUE (AccessTypeName, ApplicationID)
) ON Data
GO
/* 
 * TABLE: AccessType 
 */

ALTER TABLE Reference.AccessType ADD CONSTRAINT FK_AccessType_Application 
    FOREIGN KEY (ApplicationID)
    REFERENCES Reference.Application(ApplicationID)
GO
/* 
 * INDEX: FK_AccessType_Application 
 */

CREATE INDEX FK_AccessType_Application ON Reference.AccessType(ApplicationID)