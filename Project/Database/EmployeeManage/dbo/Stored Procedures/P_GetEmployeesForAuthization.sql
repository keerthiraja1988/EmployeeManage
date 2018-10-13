



CREATE PROC [dbo].[P_GetEmployeesForAuthization] 
			 @EmployeeId INT 
		
  AS
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()

SELECT TOP 1000 [EmployeeAmendmentId]
      ,[EmployeeId]
      ,[IsActive]
      ,[AmendmentType]
      ,[IsAmendmentApproved]
      ,[AmendmentComment]
      ,[ApproverComment]
      ,[CreatedOn]
      ,[CreatedBy]
      ,[ModifiedOn]
      ,[ModifiedBy]
  FROM [dbo].[EmployeeAmendment]


	RETURN @@Error
end