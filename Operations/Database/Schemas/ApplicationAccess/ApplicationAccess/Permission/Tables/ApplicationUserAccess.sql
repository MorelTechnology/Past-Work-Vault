/* 
 * TABLE: ApplicationUserAccess 
 */

CREATE TABLE Permission.ApplicationUserAccess(
    ApplicationUserAccessID        int         IDENTITY(1,1),
    ApplicationID                  int         NOT NULL,
    [ApplicationUserID]      int         NOT NULL,
    AccessID                       int         NOT NULL,
    [ApplicationUserName]    varchar(128)    NOT NULL,
    ModifiedDate                   date        NULL,
    EffectiveDate                  date        DEFAULT CURRENT_TIMESTAMP NOT NULL,
    ExpirationDate                 date        DEFAULT '9999-12-31' NOT NULL,
    IsActive                       AS Utility.SetIsActive(EffectiveDate, ExpirationDate),
    CreatedBy                      varchar(128)    DEFAULT SYSTEM_USER NOT NULL,
    CreatedDate                    date        DEFAULT CURRENT_TIMESTAMP NOT NULL,
    ModifiedBy                     varchar(128)    NULL,
    CONSTRAINT PK_ApplicationUserAccess PRIMARY KEY CLUSTERED (ApplicationUserAccessID)
) ON Data
GO
/* 
 * TABLE: ApplicationUserAccess 
 */

ALTER TABLE Permission.ApplicationUserAccess ADD CONSTRAINT FK_ApplicationUserAccess_Access 
    FOREIGN KEY (AccessID)
    REFERENCES Permission.Access(AccessID)
GO
ALTER TABLE Permission.ApplicationUserAccess ADD CONSTRAINT FK_ApplicationUserAccess_Application 
    FOREIGN KEY (ApplicationID)
    REFERENCES Reference.Application(ApplicationID)
GO
ALTER TABLE Permission.ApplicationUserAccess ADD CONSTRAINT FK_ApplicationUserAccess_ApplicationUser 
    FOREIGN KEY ([ApplicationUserID])
    REFERENCES Permission.ApplicationUser(ExternalApplicationUserID)
GO
/* 
 * INDEX: FK_ApplicationUserAccess_Application 
 */

CREATE INDEX FK_ApplicationUserAccess_Application ON Permission.ApplicationUserAccess(ApplicationID)
GO
/* 
 * INDEX: FK_ApplicationUserAccess_ApplicationUser 
 */

CREATE INDEX FK_ApplicationUserAccess_ApplicationUser ON Permission.ApplicationUserAccess([ApplicationUserID])
GO
/* 
 * INDEX: FK_ApplicationUserAccess_Access 
 */

CREATE INDEX FK_ApplicationUserAccess_Access ON Permission.ApplicationUserAccess(AccessID)