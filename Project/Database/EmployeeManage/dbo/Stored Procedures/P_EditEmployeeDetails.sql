
CREATE PROC [dbo].[P_EditEmployeeDetails] 
			-- @EmployeeId INT 
			@EmployeeId int	,
			@FirstName [nvarchar](max) ,
			@LastName [nvarchar](max) ,
			@Email [nvarchar](max) 	,
			@DateOfBirth DATETIME ,	
			@DateOfJoining DATETIME ,
			@TIN  [nvarchar](200) ,
			@PASSPORT  [nvarchar](200), 	
			@WorkLocation INT ,
			@UserId BIGINT 
			,@EmployeeAddresses [dbo].[T_EmployeeAddress] READONLY
  AS
begin
	DECLARE @CurrentDateTime DATETIME = GETDATE()

	UPDATE [dbo].[Employee] SET  FirstName = @FirstName
								,LastName  = @LastName
								,Email  = @Email
								,DateOfBirth  = @DateOfBirth
								,DateOfJoining  = @DateOfJoining
								,TIN = @TIN
								,PASSPORT  = @PASSPORT
								,WorkLocation  = @WorkLocation
								,ModifiedOn = @CurrentDateTime
								,ModifiedBy = @UserId
							WHERE [EmployeeId]  = @EmployeeId

	
	SELECT @@error
end