CREATE TABLE [dbo].[Country] (
    [CountryId]   INT            IDENTITY (200, 1) NOT NULL,
    [CountryCode] NVARCHAR (500) NULL,
    [CountryName] NVARCHAR (500) NULL,
    [IsActive]    BIT            NULL,
    [CreatedOn]   DATETIME       NULL,
    [CreatedBy]   BIGINT         NULL,
    [ModifiedOn]  DATETIME       NULL,
    [ModifiedBy]  BIGINT         NULL,
    CONSTRAINT [PK_dbo.Country] PRIMARY KEY CLUSTERED ([CountryId] ASC)
);

