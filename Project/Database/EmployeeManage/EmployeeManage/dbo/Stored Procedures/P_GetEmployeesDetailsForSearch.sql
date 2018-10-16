




CREATE PROC [dbo].[P_GetEmployeesDetailsForSearch] 
			-- @EmployeeId INT 
			@SearchText [nvarchar](max)
			,@SearchColumn [nvarchar](500)
		WITH RECOMPILE  
  AS  
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()

	IF @SearchColumn = 'FullName'
	BEGIN
	SELECT TOP 100 [EmployeeId]
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
  FROM [EmployeeManage].[dbo].[Employee]
  WHERE [FirstName] LIKE '%'  + @SearchText + '%'

  END

  
	IF @SearchColumn = 'EmployeeId'
	BEGIN
	SELECT TOP 100 CAST( EmployeeId AS VARCHAR(500))  AS EmployeeId
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
  FROM [EmployeeManage].[dbo].[Employee]
  WHERE CAST( EmployeeId AS VARCHAR(500)) LIKE '%'  + @SearchText + '%'

  END

  IF @SearchColumn = 'Email'
	BEGIN
	SELECT TOP 100 CAST( EmployeeId AS VARCHAR(500))  AS EmployeeId
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
  FROM [EmployeeManage].[dbo].[Employee]
  WHERE  [Email] LIKE '%'  + @SearchText + '%'

  END

  IF @SearchColumn = 'PASSPORT'
	BEGIN
	SELECT TOP 100 CAST( EmployeeId AS VARCHAR(500))  AS EmployeeId
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
  FROM [EmployeeManage].[dbo].[Employee]
  WHERE  [PASSPORT] LIKE '%'  + @SearchText + '%'

  END

  IF @SearchColumn = 'TIN'
	BEGIN
	SELECT TOP 100 CAST( EmployeeId AS VARCHAR(500))  AS EmployeeId
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
  FROM [EmployeeManage].[dbo].[Employee]
  WHERE  [TIN] LIKE '%'  + @SearchText + '%'

  END
	RETURN @@Error
end	








