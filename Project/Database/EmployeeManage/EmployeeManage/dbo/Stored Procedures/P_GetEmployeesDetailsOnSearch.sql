


CREATE PROC [dbo].[P_GetEmployeesDetailsOnSearch] 		
			@EmployeeId [nvarchar](500)		
			,@FullName [nvarchar](max)
			,@Email [nvarchar](max)
			,@TIN [nvarchar](max)
			,@Passport [nvarchar](max)
			,@DateOfBirthStart DATETIME
			,@DateOfBirthEnd DATETIME
			,@DateOfJoiningStart DATETIME
			,@DateOfJoiningEnd DATETIME
		WITH RECOMPILE  
  AS  
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()

	


SELECT  [EmployeeId]
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
  FROM [dbo].[Employee]
  WHERE	
	  (CAST([EmployeeId] AS NVARCHAR(500)) LIKE '%' + @EmployeeId + '%'  or @EmployeeId is null)
  AND (FullName = @FullName or @FullName is null)
  AND ([Email] = @Email or @Email is null)
  AND ([TIN] = @TIN or @TIN is null)
  AND ([PASSPORT] = @Passport or @Passport is null)

  AND (([DateOfBirth] >= @DateOfBirthStart or @DateOfBirthStart is null) 
			AND ([DateOfBirth] <= @DateOfBirthEnd or @DateOfBirthEnd is null) )
 
  AND (([DateOfJoining] >= @DateOfJoiningStart or @DateOfJoiningStart is null) 
			AND ([DateOfJoining] <= @DateOfJoiningEnd or @DateOfJoiningEnd is null) ) 

	RETURN @@Error
end	








