


CREATE PROC [dbo].[P_CreateEmployee] 
	
			@FullName [nvarchar](max) ,
			@FirstName [nvarchar](max) ,
			@LastName [nvarchar](max) ,
			@Initial [nvarchar](200) ,	
			@Email [nvarchar](max) 	,
			@DateOfBirth DATETIME ,	
			@DateOfJoining DATETIME ,	
			--@PermenanatAddress INT ,
			--@TempAddress  INT 	,
			@TIN  [nvarchar](200) ,
			@PASSPORT  [nvarchar](200), 	
			@WorkLocation INT ,
			@UserId BIGINT 
			,@EmployeeAddress [dbo].[T_EmployeeAddress] READONLY
		
  AS
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()
	DECLARE @PermenantAddressId INT 
	DECLARE @CurrentAddressId INT 
	DECLARE @EmployeeId INT 

	BEGIN TRANSACTION
	INSERT INTO [dbo].[Employee]
           ([FullName]
           ,[FirstName]
           ,[LastName]
           ,[Initial]
           ,[Email]
           ,[DateOfBirth]
           ,[DateOfJoining]
           --,PermenantAddressId
           --,[TempAddress]
           ,[TIN]
           ,[PASSPORT]
           ,[WorkLocation]
           ,[IsActive]
		   ,[IsAuthorized]
		   ,[CreatedOn]
           ,[CreatedBy]
           ,[ModifiedOn]
           ,[ModifiedBy]
         )
		SELECT @FirstName 	+ ' ' + @LastName 			
			  , @FirstName 			
			  , @LastName 			
			  , @Initial 			
			  , @Email 				
			  , @DateOfBirth 		
			  , @DateOfJoining 		
			 --,1
			  --, @TempAddress  		
			  , @TIN  				
			  , @PASSPORT  			
			  , @WorkLocation 	
			  , 0
			  ,0
			   ,@CurrentDateTime
			   ,@UserId
			   ,@CurrentDateTime
			   ,@UserId
			   
	SET @EmployeeId = SCOPE_IDENTITY()
	
INSERT INTO [dbo].[EmployeeAmendment]
           ([EmployeeId]
           ,[IsActive]
           ,[AmendmentType]
           ,[IsAmendmentApproved]
          
           ,[CreatedOn]
           ,[CreatedBy]
           ,[ModifiedOn]
           ,[ModifiedBy])

		SELECT @EmployeeId 			
			  , 1 			
			  , 'New Starter' 			
			  , 0 			
			   ,@CurrentDateTime
			   ,@UserId
			   ,@CurrentDateTime
			   ,@UserId


	INSERT INTO [dbo].[EmployeeAddress]
			   ([EmployeeId]
			   ,[Address1]
			   ,[Address2]
			   ,[Address3]
			   ,[City]
			   ,[State]
			   ,[CountryId]
			   ,[AddressType]
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
			   ,[AddressType]
			   ,1
			   ,@CurrentDateTime
			   ,@UserId
			   ,@CurrentDateTime
			   ,@UserId
		FROM @EmployeeAddress 	
	
	
	COMMIT TRANSACTION



	UPDATE [dbo].[Employee] SET  [PermenantAddressId] = (SELECT [EmployeeAddressId] FROM [dbo].[EmployeeAddress] PerAddress
																WHERE PerAddress.EmployeeId =  @EmployeeId AND 
																		PerAddress.AddressType = 'P')
										,[CurrentAddressId]  = (SELECT [EmployeeAddressId] FROM [dbo].[EmployeeAddress] PerAddress
																WHERE PerAddress.EmployeeId =  @EmployeeId AND 
																		PerAddress.AddressType = 'c')
								WHERE [EmployeeId]  = @EmployeeId

		SELECT @@Error
end