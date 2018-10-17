



CREATE PROC [dbo].[P_SaveIpAddressDetailsOnLogin] 
						 @UserId bigint
						,@UserName  nvarchar(500)
						,@CookieUniqueId [UNiqueidentifier] 
						 ,@IsLoginSuccess BIT
						, @Status nvarchar(500)
						,@IpAddress [nvarchar](500) 
						,@Country nvarchar(500)
						,@CountryCode nvarchar(500)
						,@Region nvarchar(500)
						,@RegionName nvarchar(500)
						,@City nvarchar(500)
						,@Zip nvarchar(500)
						,@Lat nvarchar(500)
						,@Lon nvarchar(500)
						,@TimeZone nvarchar(500)
						,@ISP nvarchar(500)
						,@ISPDetails nvarchar(500)
						,@ORG nvarchar(500)
						,@Query nvarchar(500)
						,@CreatedOn datetime
						,@CreatedBy bigint
						,@CreatedByUserName nvarchar(500)
						,@ModifiedOn datetime
						,@ModifiedBy bigint
						,@ModifiedByUserName nvarchar(500)		
  AS
begin
	
	DECLARE @CurrentDateTime DATETIME = GETDATE()

	INSERT INTO [dbo].[IpAddressTracking]
           (
		   		    [UserId]
           ,[UserName]
           ,[IsLoginSuccess]
		   ,
		   [Status]
		   ,CookieUniqueId
		   ,IpAddress
           ,[Country]
           ,[CountryCode]
           ,[Region]
           ,[RegionName]
           ,[City]
           ,[Zip]
           ,[Lat]
           ,[Lon]
           ,[TimeZone]
           ,[ISP]
           ,[ISPDetails]
           ,[ORG]
           ,[Query]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[CreatedByUserName]
           ,[ModifiedOn]
           ,[ModifiedBy]
           ,[ModifiedByUserName])
	  SELECT
	  						 @UserId 
						,@UserName  
						 ,@IsLoginSuccess 
		,@Status 
		,@CookieUniqueId
		,@IpAddress
		,@Country 
		,@CountryCode 
		,@Region 
		,@RegionName 
		,@City 
		,@Zip 
		,@Lat 
		,@Lon 
		,@TimeZone 
		,@ISP 
		,@ISPDetails 
		,@ORG 
		,@Query 
		,@CurrentDateTime
		,@CreatedBy 
		,@CreatedByUserName 
		,@CurrentDateTime
		,@ModifiedBy 
		,@ModifiedByUserName 

		SELECT @@Error
end