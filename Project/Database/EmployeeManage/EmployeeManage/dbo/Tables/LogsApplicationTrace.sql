CREATE TABLE [dbo].[LogsApplicationTrace] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Message]         NVARCHAR (MAX) NULL,
    [MessageTemplate] NVARCHAR (MAX) NULL,
    [Level]           NVARCHAR (128) NULL,
    [TimeStamp]       DATETIME       NULL,
    [Exception]       NVARCHAR (MAX) NULL,
    [Properties]      XML            NULL,
    CONSTRAINT [PK_ApplicationLogs] PRIMARY KEY CLUSTERED ([Id] DESC)
);

