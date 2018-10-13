


CREATE PROC [dbo].[P_DeleteEmployee] 
	
			@EmployeeId INT
			
			
		
  AS
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()
	

	DELETE FROM Employee
		WHERE EmployeeId = @EmployeeId


	SELECT @@error
end