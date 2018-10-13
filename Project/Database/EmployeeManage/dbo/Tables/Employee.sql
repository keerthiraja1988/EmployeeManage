CREATE TABLE [dbo].[Employee] (
    [EmployeeId]         INT            IDENTITY (300000, 1) NOT NULL,
    [FullName]           NVARCHAR (MAX) NULL,
    [FirstName]          NVARCHAR (MAX) NULL,
    [LastName]           NVARCHAR (MAX) NULL,
    [Initial]            NVARCHAR (200) NULL,
    [Email]              NVARCHAR (MAX) NULL,
    [DateOfBirth]        DATETIME       NULL,
    [DateOfJoining]      DATETIME       NULL,
    [PermenantAddressId] INT            NULL,
    [CurrentAddressId]   INT            NULL,
    [TIN]                NVARCHAR (200) NULL,
    [PASSPORT]           NVARCHAR (200) NULL,
    [WorkLocation]       INT            NULL,
    [IsActive]           BIT            NULL,
    [IsAuthorized]       BIT            NULL,
    [CreatedOn]          DATETIME       NULL,
    [CreatedBy]          BIGINT         NULL,
    [ModifiedOn]         DATETIME       NULL,
    [ModifiedBy]         BIGINT         NULL,
    CONSTRAINT [PK_dbo.Employee] PRIMARY KEY CLUSTERED ([EmployeeId] ASC)
);


GO



CREATE trigger [dbo].[TRG_Employee]
on  [dbo].[Employee]
after UPDATE, INSERT, DELETE
as

declare @EmployeeId int,@UserId int, @activity varchar(20);
if exists(SELECT * from inserted) and exists (SELECT * from deleted)
begin
    SET @activity = 'UPDATE';
   
    SELECT TOP 1 @UserId = ModifiedBy from inserted i;

	INSERT INTO [dbo].[EmployeeHistory]
           ([DMLOperation]
           
           ,[DMLOperationBy]
           ,[EmployeeId]
           ,[FullName]
           ,[FirstName]
           ,[LastName]
           ,[Initial]
           ,[Email]
           ,[DateOfBirth]
           ,[DateOfJoining]
           ,[PermenantAddressId]
           ,[CurrentAddressId]
           ,[TIN]
           ,[PASSPORT]
           ,[WorkLocation]
           ,[IsActive]
		   ,[IsAuthorized]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[ModifiedOn]
           ,[ModifiedBy])
	
	SELECT		@activity
				,@UserId
			  ,[EmployeeId]
			  ,[FullName]
			  ,[FirstName]
			  ,[LastName]
			  ,[Initial]
			  ,[Email]
			  ,[DateOfBirth]
			  ,[DateOfJoining]
			  ,[PermenantAddressId]
			  ,[CurrentAddressId]
			  ,[TIN]
			  ,[PASSPORT]
			  ,[WorkLocation]
			  ,[IsActive]
			  ,[IsAuthorized]
			  ,[CreatedOn]
			  ,[CreatedBy]
			  ,[ModifiedOn]
			  ,[ModifiedBy]
  FROM inserted

   
end

If exists (Select * from inserted) and not exists(Select * from deleted)
begin
   
     SET @activity = 'INSERT';
  
    SELECT TOP 1 @UserId = ModifiedBy from inserted i;

	INSERT INTO [dbo].[EmployeeHistory]
           ([DMLOperation]
           
           ,[DMLOperationBy]
           ,[EmployeeId]
           ,[FullName]
           ,[FirstName]
           ,[LastName]
           ,[Initial]
           ,[Email]
           ,[DateOfBirth]
           ,[DateOfJoining]
           ,[PermenantAddressId]
           ,[CurrentAddressId]
           ,[TIN]
           ,[PASSPORT]
           ,[WorkLocation]
           ,[IsActive]
		   ,[IsAuthorized]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[ModifiedOn]
           ,[ModifiedBy])
	
	SELECT		@activity
				,@UserId
			  ,[EmployeeId]
			  ,[FullName]
			  ,[FirstName]
			  ,[LastName]
			  ,[Initial]
			  ,[Email]
			  ,[DateOfBirth]
			  ,[DateOfJoining]
			  ,[PermenantAddressId]
			  ,[CurrentAddressId]
			  ,[TIN]
			  ,[PASSPORT]
			  ,[WorkLocation]
			  ,[IsActive]
			  ,[IsAuthorized]
			  ,[CreatedOn]
			  ,[CreatedBy]
			  ,[ModifiedOn]
			  ,[ModifiedBy]
  FROM inserted

end

If exists(select * from deleted) and not exists(Select * from inserted)
begin 
    SET @activity = 'DELETE';
    
  
    SELECT TOP 1 @UserId = ModifiedBy from deleted i;

	INSERT INTO [dbo].[EmployeeHistory]
           ([DMLOperation]
           
           ,[DMLOperationBy]
           ,[EmployeeId]
           ,[FullName]
           ,[FirstName]
           ,[LastName]
           ,[Initial]
           ,[Email]
           ,[DateOfBirth]
           ,[DateOfJoining]
           ,[PermenantAddressId]
           ,[CurrentAddressId]
           ,[TIN]
           ,[PASSPORT]
           ,[WorkLocation]
           ,[IsActive]
		   ,[IsAuthorized]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[ModifiedOn]
           ,[ModifiedBy])
	
	SELECT		@activity
				,@UserId
			  ,[EmployeeId]
			  ,[FullName]
			  ,[FirstName]
			  ,[LastName]
			  ,[Initial]
			  ,[Email]
			  ,[DateOfBirth]
			  ,[DateOfJoining]
			  ,[PermenantAddressId]
			  ,[CurrentAddressId]
			  ,[TIN]
			  ,[PASSPORT]
			  ,[WorkLocation]
			  ,[IsActive]
			  ,[IsAuthorized]
			  ,[CreatedOn]
			  ,[CreatedBy]
			  ,[ModifiedOn]
			  ,[ModifiedBy]
  FROM deleted 
end