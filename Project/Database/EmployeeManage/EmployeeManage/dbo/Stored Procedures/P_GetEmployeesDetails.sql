




CREATE PROC [dbo].[P_GetEmployeesDetails] 
			-- @EmployeeId INT 
		
  AS
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()

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



	RETURN @@Error
end	








