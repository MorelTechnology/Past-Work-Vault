/* 
 * TABLE: Access 
 */

CREATE TABLE Permission.Access(
    AccessID          int             IDENTITY(1,1),
    ParentAccessID    int             NULL,
    ApplicationID     int             NOT NULL,
    AccessTypeID      int             NOT NULL,
    AccessName        varchar(20)     NOT NULL,
    AccessDesc        varchar(100)    NULL,
    CONSTRAINT PK_Access PRIMARY KEY CLUSTERED (AccessID)
) ON Data
GO
/* 
 * TABLE: Access 
 */

ALTER TABLE Permission.Access ADD CONSTRAINT FK_Access_AccessID_Access_ParentAccessID 
    FOREIGN KEY (ParentAccessID)
    REFERENCES Permission.Access(AccessID)
GO
ALTER TABLE Permission.Access ADD CONSTRAINT FK_Access_AccessType_AccessTypeID_ApplicationID 
    FOREIGN KEY (AccessTypeID, ApplicationID)
    REFERENCES Reference.AccessType(AccessTypeID, ApplicationID)
GO
ALTER TABLE Permission.Access ADD CONSTRAINT FK_Access_Application 
    FOREIGN KEY (ApplicationID)
    REFERENCES Reference.Application(ApplicationID)
GO
/* 
 * INDEX: FK_Access_Application 
 */

CREATE INDEX FK_Access_Application ON Permission.Access(ApplicationID)
GO
/* 
 * INDEX: FK_Access_AccessType_AccessTypeID_ApplicationID 
 */

CREATE INDEX FK_Access_AccessType_AccessTypeID_ApplicationID ON Permission.Access(AccessTypeID, ApplicationID)
GO
/* 
 * INDEX: FK_Access_AccessID_Access_ParentAccessID 
 */

CREATE INDEX FK_Access_AccessID_Access_ParentAccessID ON Permission.Access(ParentAccessID)