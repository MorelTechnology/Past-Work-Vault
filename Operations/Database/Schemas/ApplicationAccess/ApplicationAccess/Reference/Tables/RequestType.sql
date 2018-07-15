/* 
 * TABLE: RequestType 
 */

CREATE TABLE Reference.RequestType(
    RequestTypeID      int            IDENTITY(1,1),
    RequestTypeName    varchar(20)    NOT NULL,
    CONSTRAINT PK_RequestType PRIMARY KEY CLUSTERED (RequestTypeID),
    CONSTRAINT UQ_RequestType_RequestTypeName  UNIQUE (RequestTypeName)
) ON Data