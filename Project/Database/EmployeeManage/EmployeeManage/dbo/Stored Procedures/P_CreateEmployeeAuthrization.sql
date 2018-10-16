


CREATE PROC [dbo].[P_CreateEmployeeAuthrization] 
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

 	UPDATE [dbo].[Employee] SET       [IsActive]  = @IsAmendmentApproved
										,[IsAuthorized] = @IsAmendmentApproved
							  ,[ModifiedOn] = @CurrentDateTime
							  ,[ModifiedBy] = @UserId
								WHERE [EmployeeId]  = @EmployeeId

RETURN @@Error
end	






