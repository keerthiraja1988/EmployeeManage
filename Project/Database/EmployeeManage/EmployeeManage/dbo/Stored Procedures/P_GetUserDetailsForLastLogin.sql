





CREATE PROC [dbo].[P_GetUserDetailsForLastLogin] 
	@UserId int, 
	@CookieUniqueId [uniqueidentifier]
AS
BEGIN
	
	SELECT TOP 1 [IpAddressTrackingId]
      ,[UserId]  
	  ,[CookieUniqueId]
      ,[IpAddress]  
      ,[Country]
      ,[CountryCode]     
      ,[City]     
      ,[ISPDetails]    
      ,[SessionDisconnectedOn]
      ,[CreatedOn]
  FROM [EmployeeManage].[dbo].[IpAddressTracking]
  WHERE [UserId] = @UserId 
  AND [CookieUniqueId] <>  @CookieUniqueId
  ORDER BY [IpAddressTrackingId] DESC

  	SELECT TOP 1 [IpAddressTrackingId]
      ,[UserId]  
	  ,[CookieUniqueId]
      ,[IpAddress]  
      ,[Country]
      ,[CountryCode]     
      ,[City]     
      ,[ISPDetails]    
      ,[SessionDisconnectedOn]
      ,[CreatedOn]
  FROM [EmployeeManage].[dbo].[IpAddressTracking]
  WHERE [UserId] = @UserId 
  AND [CookieUniqueId] =  @CookieUniqueId


END


