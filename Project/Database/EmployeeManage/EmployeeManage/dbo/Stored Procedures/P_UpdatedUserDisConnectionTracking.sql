





CREATE PROC [dbo].[P_UpdatedUserDisConnectionTracking] 
						 @UserId bigint
						,@CookieUniqueId [uniqueidentifier] 
						,@IpAddress [nvarchar](500) 
					
						,@SessionDisconnectedOn datetime
						
  AS
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()

	UPDATE [dbo].[IpAddressTracking]
		 SET [UserId] = @UserId      
		,[SessionDisconnectedOn] = @SessionDisconnectedOn
     
 WHERE [UserId] = @UserId AND IpAddress = @IpAddress
 AND CookieUniqueId = @CookieUniqueId


		SELECT @@Error
end	








