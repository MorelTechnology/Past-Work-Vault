/* 
 * TABLE: ApplicationUser 
 */

CREATE TABLE Permission.ApplicationUser(
    ExternalApplicationUserID    int             IDENTITY(1,1),
    ActiveDirectoryUserName      varchar(256)    NOT NULL,
    LastName                     varchar(30)     NOT NULL,
    FirstName                    varchar(30)     NOT NULL,
    EffectiveDate                date            DEFAULT CURRENT_TIMESTAMP NOT NULL,
    ExpirationDate               date            DEFAULT '9999-12-31' NOT NULL,
    IsActive                     AS Utility.SetIsActive(EffectiveDate, ExpirationDate),
    CreatedBy                    varchar(128)    DEFAULT SYSTEM_USER NOT NULL,
    CreatedDate                  datetime        DEFAULT CURRENT_TIMESTAMP NOT NULL,
    ModifiedBy                   varchar(128)    NULL,
    ModifiedDate                 datetime        DEFAULT CURRENT_TIMESTAMP NULL,
    CONSTRAINT PK_ExternalApplicationUser PRIMARY KEY CLUSTERED (ExternalApplicationUserID),
    CONSTRAINT UQ_ApplicationUser_RiverStoneUserName_EffectiveDate_ExpirationDate  UNIQUE (ActiveDirectoryUserName, EffectiveDate, ExpirationDate)
) ON Data