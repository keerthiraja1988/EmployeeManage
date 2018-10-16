CREATE TABLE [dbo].[Roles] (
    [RoleId]   INT            IDENTITY (1, 1) NOT NULL,
    [RoleName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Roles] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);

