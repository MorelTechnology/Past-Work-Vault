/* 
 * TABLE: Application 
 */

CREATE TABLE Reference.Application(
    ApplicationID      int             NOT NULL,
    ApplicationName    varchar(128)    NOT NULL,
    HostName           varchar(128)    NOT NULL,
    ContactName        varchar(256)    NULL,
    ContactEmail       varchar(256)    NULL,
    ContactPhone       varchar(20)     NULL,
    Notes              text            NULL,
    EffectiveDate      date            DEFAULT CURRENT_TIMESTAMP NOT NULL,
    ExpirationDate     date            DEFAULT '9999-12-31' NOT NULL,
    IsActive           AS Utility.SetIsActive(EffectiveDate, ExpirationDate),
    CreatedBy          varchar(128)    DEFAULT SYSTEM_USER NOT NULL,
    CreatedDate        datetime        DEFAULT CURRENT_TIMESTAMP NOT NULL,
    ModifiedBy         varchar(128)    NULL,
    ModifiedDate       datetime        NULL,
    CONSTRAINT PK_ExternalApplication PRIMARY KEY CLUSTERED (ApplicationID)
) ON Data