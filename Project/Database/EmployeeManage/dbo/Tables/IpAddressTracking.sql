CREATE TABLE [dbo].[IpAddressTracking] (
    [IpAddressTrackingId]   BIGINT           IDENTITY (200, 1) NOT NULL,
    [CookieUniqueId]        UNIQUEIDENTIFIER NULL,
    [UserId]                BIGINT           NULL,
    [UserName]              NVARCHAR (500)   NULL,
    [IpAddress]             NVARCHAR (500)   NULL,
    [IsLoginSuccess]        BIT              NULL,
    [Status]                NVARCHAR (500)   NULL,
    [Country]               NVARCHAR (500)   NULL,
    [CountryCode]           NVARCHAR (500)   NULL,
    [Region]                NVARCHAR (500)   NULL,
    [RegionName]            NVARCHAR (500)   NULL,
    [City]                  NVARCHAR (500)   NULL,
    [Zip]                   NVARCHAR (500)   NULL,
    [Lat]                   NVARCHAR (500)   NULL,
    [Lon]                   NVARCHAR (500)   NULL,
    [TimeZone]              NVARCHAR (500)   NULL,
    [ISP]                   NVARCHAR (500)   NULL,
    [ISPDetails]            NVARCHAR (500)   NULL,
    [ORG]                   NVARCHAR (500)   NULL,
    [Query]                 NVARCHAR (500)   NULL,
    [SessionDisconnectedOn] DATETIME         NULL,
    [CreatedOn]             DATETIME         NULL,
    [CreatedBy]             BIGINT           NULL,
    [CreatedByUserName]     NVARCHAR (500)   NULL,
    [ModifiedOn]            DATETIME         NULL,
    [ModifiedBy]            BIGINT           NULL,
    [ModifiedByUserName]    NVARCHAR (500)   NULL,
    CONSTRAINT [PK_IpAddressTrackingId] PRIMARY KEY CLUSTERED ([IpAddressTrackingId] DESC)
);



