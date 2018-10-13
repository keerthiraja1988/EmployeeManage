CREATE TABLE [dbo].[LogsAspNetCoreError] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Application] NVARCHAR (50)   NULL,
    [Logged]      DATETIME        NULL,
    [Level]       NVARCHAR (50)   NULL,
    [UserId]      NVARCHAR (50)   NULL,
    [UserName]    NVARCHAR (MAX)  NULL,
    [Message]     NVARCHAR (MAX)  NULL,
    [IpAddress]   NVARCHAR (50)   NULL,
    [Url]         NVARCHAR (1000) NULL,
    [Host]        NVARCHAR (1000) NULL,
    [QueryString] NVARCHAR (1000) NULL,
    [Browser]     NVARCHAR (1000) NULL,
    [Cookie]      NVARCHAR (1000) NULL,
    [Referrer]    NVARCHAR (1000) NULL,
    [Machinename] NVARCHAR (1000) NULL,
    [Logger]      NVARCHAR (250)  NULL,
    [Callsite]    NVARCHAR (MAX)  NULL,
    [Exception]   NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.AspNetCoreLog] PRIMARY KEY CLUSTERED ([Id] DESC)
);

