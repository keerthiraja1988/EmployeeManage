
CREATE PROC [dbo].[P_UpdateEmployeeDetails] 
			 @EmployeeId INT,
			@FullName [nvarchar](max) ,
			@FirstName [nvarchar](max) ,
			@LastName [nvarchar](max) ,
			@Initial [nvarchar](200) ,	
			@Email [nvarchar](max) 	,
			@DateOfBirth DATETIME ,	
			@DateOfJoining DATETIME ,		
			@TIN  [nvarchar](200) ,
			@PASSPORT  [nvarchar](200), 	
			@WorkLocation INT ,
			@UserId BIGINT ,
			@T_EmployeeAddress [dbo].[T_EmployeeAddress] READONLY
		
  AS
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()
	DECLARE @PermanentAddressId INT 
	DECLARE @CurrentAddressId INT 
	
	IF EXISTS(SELECT  * FROM @T_EmployeeAddress WHERE [AddressType]  = 'P')  
	BEGIN
	INSERT INTO [dbo].[EmployeeAddress]
			   ([EmployeeId]
			   ,[Address1]
			   ,[Address2]
			   ,[Address3]
			   ,[City]
			   ,[State]
			   ,[CountryId]
			   ,[IsActive]
			   ,[CreatedOn]
			   ,[CreatedBy]
			   ,[ModifiedOn]
			   ,[ModifiedBy])
		SELECT @EmployeeId
				,[Address1]
			   ,[Address2]
			   ,[Address3]
			   ,[City]
			   ,[State]
			     ,[CountryId]
			   ,1
			   ,@CurrentDateTime
			   ,@UserId
			   ,@CurrentDateTime
			   ,@UserId
		FROM @T_EmployeeAddress WHERE [AddressType]  = 'P'
	
	select @PermanentAddressId	= @@IDENTITY
	END
	ELSE
		BEGIN

		SELECT @PermanentAddressId = [PermenantAddressId]  
		FROM
		[dbo].[Employee] WHERE EmployeeId = @EmployeeId

		END

	IF EXISTS(SELECT  * FROM @T_EmployeeAddress WHERE [AddressType]  = 'C')  
	BEGIN
	INSERT INTO [dbo].[EmployeeAddress]
			   ([EmployeeId]
			   ,[Address1]
			   ,[Address2]
			   ,[Address3]
			   ,[City]
			   ,[State]
			     ,[CountryId]
			   ,[IsActive]
			   ,[CreatedOn]
			   ,[CreatedBy]
			   ,[ModifiedOn]
			   ,[ModifiedBy])
		SELECT @EmployeeId
				,[Address1]
			   ,[Address2]
			   ,[Address3]
			   ,[City]
			   ,[State]
			      ,[CountryId]
			   ,1
			   ,@CurrentDateTime
			   ,@UserId
			   ,@CurrentDateTime
			   ,@UserId
		FROM @T_EmployeeAddress WHERE [AddressType]  = 'C'
	
	select @CurrentAddressId	= @@IDENTITY
	END
	ELSE
		BEGIN

		SELECT @CurrentAddressId = [CurrentAddressId]
		FROM
		[dbo].[Employee] WHERE EmployeeId = @EmployeeId

		END

	UPDATE [dbo].[Employee]
	   SET
			 [FullName] = 	@FullName
			,[FirstName] = 	@FirstName
			,[LastName] = 	@LastName
			,[Initial] = 	@Initial
			,[Email] = 		@Email
			,[DateOfBirth] = @DateOfBirth
			,[DateOfJoining] = @DateOfJoining
			,[PermenantAddressId] =@PermanentAddressId
			,[CurrentAddressId] = @CurrentAddressId
			,[TIN] = 		@TIN
			,[PASSPORT] = 	@PASSPORT
			,[WorkLocation] = @WorkLocation
			,[IsActive] = 	1
			--,[IsAuthorized] = @IsAuthorized
			,[CreatedOn] = 	@CurrentDateTime
			,[CreatedBy] = 	@UserId
			,[ModifiedOn] = @CurrentDateTime
			,[ModifiedBy] =@UserId	

	WHERE EmployeeId = @EmployeeId







	RETURN @@Error
end	





