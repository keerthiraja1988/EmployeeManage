


CREATE PROC [dbo].[P_RegisterNewUser] 
	@UserId 		[bigint] ,
	@UserName 		[nvarchar](max) ,
	@FullName 		[nvarchar](max) ,
	@FirstName		[nvarchar](max) ,
	@LastName 		[nvarchar](max) ,
	@Email 			[nvarchar](max) ,
	@Password 		[nvarchar](max) ,
	@PasswordHash		 [nvarchar](500) ,
	@PasswordSalt	[nvarchar](100) ,
	@IsActive 		[bit] ,
	@IsLocked 		[bit] ,
	@CreatedBy 		[bigint] 
		
  AS
begin

DECLARE @TodaysDate DATETIME = GETDATE()

INSERT INTO [dbo].[Users]
           ([UserName]
           ,[FullName]
           ,[FirstName]
           ,[LastName]
           ,[Email]
           ,[Password]
           ,[PasswordHash]
           ,[PasswordSalt]
           ,[IsActive]
           ,[IsLocked]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[ModifiedOn]
           ,[ModifiedBy])
		SELECT
			@UserName 		
			,@FirstName	+ ' ' + @LastName	
			,@FirstName		
			,@LastName 		
			,@Email 			
			,@Password 		
			,@PasswordHash	
			,@PasswordSalt	
			,@IsActive 		
			,@IsLocked 
			,@TodaysDate			
			,@CreatedBy 		
			,@TodaysDate
			,@CreatedBy
	
	
	DECLARE @ID BIGINT = SCOPE_IDENTITY()

INSERT INTO [dbo].[UserRoles]
           ([UserId]
           ,[RoleId]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[ModifiedOn]
           ,[ModifiedBy])
	SELECT @ID
      ,[RoleId]
	  			,@TodaysDate			
			,@CreatedBy 		
			,@TodaysDate
			,@CreatedBy
		 FROM [dbo].[Roles]		
			
SELECT CAST(1 AS bit)
end