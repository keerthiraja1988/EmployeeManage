CREATE TABLE [dbo].[EmployeeAmendment] (
    [EmployeeAmendmentId] INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeId]          INT            NULL,
    [IsActive]            BIT            DEFAULT ((1)) NULL,
    [AmendmentType]       NVARCHAR (50)  NULL,
    [IsAmendmentApproved] BIT            NULL,
    [AmendmentComment]    NVARCHAR (MAX) NULL,
    [ApproverComment]     NVARCHAR (MAX) NULL,
    [CreatedOn]           DATETIME       NULL,
    [CreatedBy]           BIGINT         NULL,
    [ModifiedOn]          DATETIME       NULL,
    [ModifiedBy]          BIGINT         NULL,
    CONSTRAINT [PK_dbo.EmployeeAmendment] PRIMARY KEY CLUSTERED ([EmployeeAmendmentId] ASC)
);

