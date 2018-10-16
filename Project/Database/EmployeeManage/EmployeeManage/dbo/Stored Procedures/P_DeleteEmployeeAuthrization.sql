


CREATE PROC [dbo].[P_DeleteEmployeeAuthrization] 
			@EmployeeAmendmentId int, 
			@EmployeeId INT ,
			@AmendmentType  [nvarchar](50) ,
	@IsAmendmentApproved [bit] ,	
	@ApproverComment  [nvarchar](max) ,

			@UserId BIGINT 
			
		
  AS
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()

UPDATE [dbo].[EmployeeAmendment]
   SET [EmployeeId] = @EmployeeId
      ,[IsActive] = 0    
      ,[IsAmendmentApproved] = @IsAmendmentApproved      
      ,[ApproverComment] = @ApproverComment
      ,[CreatedOn] = @CurrentDateTime
      ,[CreatedBy] = @UserId
      ,[ModifiedOn] = @CurrentDateTime
      ,[ModifiedBy] = @UserId
 WHERE [EmployeeId] = @EmployeeId and [EmployeeAmendmentId] = @EmployeeAmendmentId

 	UPDATE [dbo].[Employee] SET       [IsActive]  = Case when @IsAmendmentApproved = 1
														then 0
														else 1
														end
									
							  ,[ModifiedOn] = @CurrentDateTime
							  ,[ModifiedBy] = @UserId
								WHERE [EmployeeId]  = @EmployeeId

RETURN @@Error
end	






