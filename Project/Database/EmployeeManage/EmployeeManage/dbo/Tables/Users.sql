CREATE TABLE [dbo].[Users] (
    [UserId]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserName]     NVARCHAR (MAX) NULL,
    [FullName]     NVARCHAR (MAX) NULL,
    [FirstName]    NVARCHAR (MAX) NULL,
    [LastName]     NVARCHAR (MAX) NULL,
    [Email]        NVARCHAR (MAX) NULL,
    [Password]     NVARCHAR (MAX) NULL,
    [PasswordHash] NVARCHAR (500) NULL,
    [PasswordSalt] NVARCHAR (100) NULL,
    [IsActive]     BIT            NULL,
    [IsLocked]     BIT            NULL,
    [CreatedOn]    DATETIME       NULL,
    [CreatedBy]    BIGINT         NULL,
    [ModifiedOn]   DATETIME       NULL,
    [ModifiedBy]   BIGINT         NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

