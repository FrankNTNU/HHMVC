use HealthHelper

IF OBJECT_ID('tempdb.dbo.#Members') IS NOT NULL
begin
	DROP TABLE #Members
end

Declare @strToday nvarchar(10) = convert(nvarchar(10), getdate(), 101)
	set @strToday = REPLACE(@strToday, '/', '')

select [MemberID]
into #Members
from [dbo].[DietLog] as d
where d.Date = @strToday
group by d.[MemberID]
having count(*) >=
(
	select [MealCount] from [dbo].[Members] as m
	where d.MemberID = m.ID
)

Declare @MemberID int

While (Select Count(*) From #Members) > 0
Begin
    Select Top 1 @MemberID = [MemberID] From #Members

	insert into [dbo].[Points] values (@MemberID, 50, getdate(), 7, null)

	Delete #Members Where [MemberID] = @MemberID
End