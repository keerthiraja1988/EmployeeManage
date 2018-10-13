





CREATE PROC [dbo].[P_GetEmployeesDetailsSummary] 
			
		
  AS
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()

	DECLARE @TotalNoOfActiveEmployee INT  = 50
	DECLARE @TotalNoOfInActiveEmployee INT = 100
	DECLARE @TotalNoOfNewStarter INT = 100
	DECLARE @TotalNoOfResignation INT = 100

	



	RETURN @@Error
end