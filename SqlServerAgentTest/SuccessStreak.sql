use HealthHelper

IF OBJECT_ID('tempdb.dbo.#Members') IS NOT NULL
begin
	DROP TABLE #Members
end

select [ID]
into #Members
from [dbo].[Members]

Declare @MemberID int

while (Select Count(*) From #Members) > 0
begin
	Select Top 1 @MemberID = [ID] From #Members

	Declare @startDate datetime
	Declare @programID int = null
	Declare @settlementDay int

	select @startDate = [StartDate], @programID = [ID]
	from [dbo].[Programs] as p
	where p.StatusID = 1 and p.StartDate <= convert(date, GETDATE()) and p.EndDate >=  convert(date, GETDATE()) and p.MemberID = @MemberID

	if @programID is not null
		set @settlementDay = DATEPART(WEEKDAY, DATEADD(day, -1, @startDate))
	else
		set @settlementDay = 7
	
	if DATEPART(WEEKDAY, GETDATE()) =  @settlementDay
	begin
		IF OBJECT_ID('tempdb.dbo.#Points') IS NOT NULL
		begin
			DROP TABLE #Points
		end

		select * into #Points
		from [dbo].[Points] as pts
		where  pts.MemberID = @MemberID 
		and pts.GetPointsDateTime <= convert(date, DATEADD(day, 1, GETDATE())) and pts.GetPointsDateTime >= convert(date, DATEADD(day, -6, GETDATE()))
		and pts.StatusID = 8
		
		Declare @streak int = 0
		Declare @i int = 0

		if (select COUNT(*) from #Points) >= 5
		begin
			while (select Count(*) From #Points) > 0
			begin
				Declare @PointID int
				Declare @getPointDate datetime

				Select Top 1 @PointID = [ID], @getPointDate = [GetPointsDateTime] From #Points order by GetPointsDateTime asc

				if ((@settlementDay - 1) + @i + 1) % 7 + 1 = DATEPART(WEEKDAY, @getPointDate)
				begin
					set @streak = @streak + 1
					Delete #Points Where [ID] = @PointID

					if @streak = 5
						break

				end
				else
				begin
					set @streak = 0
				end
				
				set @i = @i + 1
			end

			if @streak >= 5
				insert into [dbo].[Points] values (@MemberID, 50, getdate(), 9, @programID)
		end
	end

	Delete #Members Where [ID] = @MemberID
end
