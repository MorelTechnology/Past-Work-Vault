/* 
 * TABLE: Request 
 */

CREATE TABLE Permission.Request(
    RequestID                  int            IDENTITY(1,1),
    RequestTypeID              int            NOT NULL,
    ApplicationUserAccessID    int            NOT NULL,
    RequestTicketNo            varchar(18)    NOT NULL,
    RequestDate                date           DEFAULT CURRENT_TIMESTAMP NOT NULL,
    FulfillmentDate            date           NULL,
    CONSTRAINT PK_Request PRIMARY KEY CLUSTERED (RequestID)
) ON Data
GO
/* 
 * TABLE: Request 
 */

ALTER TABLE Permission.Request ADD CONSTRAINT FK_ApplicationUserAccess_Request 
    FOREIGN KEY (ApplicationUserAccessID)
    REFERENCES Permission.ApplicationUserAccess(ApplicationUserAccessID)
GO
ALTER TABLE Permission.Request ADD CONSTRAINT FK_Request_RequestType 
    FOREIGN KEY (RequestTypeID)
    REFERENCES Reference.RequestType(RequestTypeID)
GO
/* 
 * INDEX: FK_ApplicationUserAccess_Request 
 */

CREATE INDEX FK_ApplicationUserAccess_Request ON Permission.Request(ApplicationUserAccessID)
GO
/* 
 * INDEX: FK_Request_RequestType 
 */

CREATE INDEX FK_Request_RequestType ON Permission.Request(RequestTypeID)