CREATE TABLE [dbo].[EmployeeAddress] (
    [EmployeeAddressId] INT            IDENTITY (100, 1) NOT NULL,
    [EmployeeId]        INT            NOT NULL,
    [Address1]          NVARCHAR (MAX) NULL,
    [Address2]          NVARCHAR (MAX) NULL,
    [Address3]          NVARCHAR (MAX) NULL,
    [City]              NVARCHAR (500) NULL,
    [State]             NVARCHAR (500) NULL,
    [AddressType]       NVARCHAR (50)  NULL,
    [CountryId]         INT            NULL,
    [IsActive]          BIT            NULL,
    [CreatedOn]         DATETIME       NULL,
    [CreatedBy]         BIGINT         NULL,
    [ModifiedOn]        DATETIME       NULL,
    [ModifiedBy]        BIGINT         NULL,
    CONSTRAINT [PK_dbo.EmployeeAddress] PRIMARY KEY CLUSTERED ([EmployeeAddressId] ASC)
);

