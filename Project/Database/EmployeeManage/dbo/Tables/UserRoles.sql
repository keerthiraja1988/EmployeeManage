CREATE TABLE [dbo].[UserRoles] (
    [UserId]     INT      NOT NULL,
    [RoleId]     INT      NOT NULL,
    [CreatedOn]  DATETIME NULL,
    [CreatedBy]  BIGINT   NULL,
    [ModifiedOn] DATETIME NULL,
    [ModifiedBy] BIGINT   NULL,
    CONSTRAINT [PK_dbo.UserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.UserRoles_dbo.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([RoleId]) ON DELETE CASCADE
);

