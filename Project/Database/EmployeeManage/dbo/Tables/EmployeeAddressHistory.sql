CREATE TABLE [dbo].[EmployeeAddressHistory] (
    [EmployeeHistoryId] INT            IDENTITY (1, 1) NOT NULL,
    [DMLOperation]      NVARCHAR (200) NOT NULL,
    [DMLOperationOn]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [DMLOperationBy]    BIGINT         DEFAULT ((1)) NULL,
    [EmployeeAddressId] INT            NOT NULL,
    [EmployeeId]        INT            NOT NULL,
    [Address1]          NVARCHAR (MAX) NULL,
    [Address2]          NVARCHAR (MAX) NULL,
    [Address3]          NVARCHAR (MAX) NULL,
    [City]              NVARCHAR (500) NULL,
    [State]             NVARCHAR (500) NULL,
    [CountryId]         INT            NULL,
    [IsActive]          BIT            NULL,
    [CreatedOn]         DATETIME       NULL,
    [CreatedBy]         BIGINT         NULL,
    [ModifiedOn]        DATETIME       NULL,
    [ModifiedBy]        BIGINT         NULL,
    CONSTRAINT [PK_dbo.EmployeeAddressHistory] PRIMARY KEY CLUSTERED ([EmployeeHistoryId] ASC)
);

