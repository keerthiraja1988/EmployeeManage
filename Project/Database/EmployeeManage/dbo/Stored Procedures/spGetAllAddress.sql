		CREATE PROCEDURE [dbo].[spGetAllAddress]  
--Add the parameters  

AS  
BEGIN  
--SET NOCOUNT ON added to prevent extra result sets from  
--interfering with SELECT statements.  
SET NOCOUNT ON;  
--Insert statements  

SELECT * FROM Address  
END