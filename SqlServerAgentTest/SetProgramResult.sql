use HealthHelper

IF OBJECT_ID('tempdb.dbo.#Programs') IS NOT NULL
begin
	DROP TABLE #Programs
end

select [ID], [StartDate], [EndDate]
into #Programs
from [dbo].[Programs]
where [StatusID] = 1 and [EndDate] = convert(date, GETDATE())

Declare @ProgramID int
Declare @startDate date
Declare @endDate date
Declare @successDays float

while (Select Count(*) From #Programs) > 0
begin
	Select Top 1 @ProgramID = [ID], @startDate = [StartDate], @endDate = [EndDate] From #Programs

	select @successDays = count(*) from [dbo].[Points]
	where convert(date, [GetPointsDateTime]) >= @startDate
		and convert(date, [GetPointsDateTime]) <= @endDate
		and [StatusID] = 8
		and [ProgramID] = @ProgramID

	Declare @totalDays float
	set @totalDays = DATEDIFF(day, @startDate, @endDate) + 1

	if @successDays / @totalDays >= 0.8
		update [dbo].[Programs] set StatusID = 3 where [ID] = @ProgramID
	else
		update [dbo].[Programs] set StatusID = 6 where [ID] = @ProgramID

	Delete #Programs Where [ID] = @ProgramID
end

-------------------------------------------------------------------------

IF OBJECT_ID('tempdb.dbo.#failPrograms') IS NOT NULL
begin
	DROP TABLE #failPrograms
end

select [ID]
into #failPrograms
from [dbo].[Programs]
where [StatusID] = 3 and DATEADD(day, 7, [EndDate]) = convert(date, GETDATE())


while (Select Count(*) From #failPrograms) > 0
begin
	Select Top 1 @ProgramID = [ID] From #failPrograms

	update [dbo].[Programs] set StatusID = 6 where [ID] = @ProgramID

	Delete #failPrograms Where [ID] = @ProgramID
end