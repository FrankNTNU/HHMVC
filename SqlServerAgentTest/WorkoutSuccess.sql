use HealthHelper

IF OBJECT_ID('tempdb.dbo.#Members') IS NOT NULL
begin
	DROP TABLE #Members
end

select [ID]
into #Members
from [dbo].[Members]

Declare @MemberID int
Declare @successCount int

while (select count(*) from #Members) > 0
begin
	select top 1 @MemberID = [ID] from #Members

	select @successCount = count(*) from [dbo].[WorkoutLog]
	where [MemberID] = @MemberID and [StatusID] = 5 and convert(date, [WorkoutTime]) = convert(date, GETDATE())

	if @successCount > 0
		insert [dbo].[Points] values (@MemberID, 20 * @successCount, GETDATE(), 11, null)

	delete #Members where [ID] = @MemberID
end