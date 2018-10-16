CREATE TYPE [dbo].[T_EmployeeAddress] AS TABLE (
    [EmployeeId]  INT            NULL,
    [Address1]    NVARCHAR (MAX) NULL,
    [Address2]    NVARCHAR (MAX) NULL,
    [Address3]    NVARCHAR (MAX) NULL,
    [City]        NVARCHAR (500) NULL,
    [State]       NVARCHAR (500) NULL,
    [CountryId]   INT            NULL,
    [AddressType] NVARCHAR (500) NULL);

