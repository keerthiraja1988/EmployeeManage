





CREATE PROC [dbo].[P_GetEmployeeDetails] 
			-- @EmployeeId INT 
		@EmployeeId int	
  AS
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()

	SELECT TOP 1 [EmployeeId]
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
  WHERE EmployeeId = @EmployeeId


	RETURN @@Error
end