




CREATE PROC [dbo].[P_GetUserDetailsForLogin] @UserName [NVARCHAR](MAX)

AS
BEGIN
	
	DECLARE @UserId BIGINT  ;

	SELECT
		@UserId =	[UserId]
		
	FROM [dbo].[Users]
	WHERE UserName = @UserName

	SELECT
		[UserId],
		[UserName],
		[FullName],
		[FirstName],
		[LastName],
		[Email],
		[password],
		[PasswordHash],
		[PasswordSalt],
		[IsActive],
		[IsLocked],
		[CreatedOn],
		[CreatedBy],
		[ModifiedOn],
		[ModifiedBy]
	FROM [dbo].[Users]
	WHERE UserName = @UserName

	SELECT
		USRROLE.[UserId],
		USRROLE.[RoleId],
		ROL.[RoleName]
		,[CreatedOn]
      ,[CreatedBy]
      ,[ModifiedOn]
      ,[ModifiedBy]
	FROM [dbo].[UserRoles] USRROLE
	INNER JOIN Roles ROL
		ON ROL.[RoleId] = USRROLE.[RoleId]
   WHERE USRROLE.[UserId] = @UserId

END