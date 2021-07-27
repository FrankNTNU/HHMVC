use HealthHelper

IF OBJECT_ID('tempdb.dbo.#Members') IS NOT NULL
begin
	DROP TABLE #Members
end

select [ID], [Gender], [Height], [Birthdate], [ActivityLevelID]
into #Members
from [dbo].[Members]

--select * from #Members

Declare @MemberID int
Declare @gender bit
Declare @height float
Declare @birthday datetime
Declare @ac int

Declare @coefficient float

While (Select Count(*) From #Members) > 0
Begin
	Declare @programID int = 0

    Select Top 1 
		@MemberID = [ID], 
		@gender = [Gender], 
		@height = [Height], 
		@birthday = [Birthdate], 
		@ac = [ActivityLevelID] From #Members

	Declare @age int
	set @age = DATEDIFF(year, @birthday, GETDATE())

	Declare @tdee float

	if @MemberID in 
	(
		select [MemberID]
		from [dbo].[Programs] as p
		where p.StatusID = 1 and p.StartDate <= convert(date, GETDATE()) and p.EndDate >=  convert(date, GETDATE())
	)
	begin

		Declare @initWeight int
		Declare @tgtWeight int
		Declare @startDate datetime
		Declare @endDate datetime
		
		select @initWeight = [InitialWeight], @tgtWeight = [TargetWeight], @ac = [ActivityLevelID], @startDate = [StartDate], @endDate = [EndDate], @programID = [ID]
		from [dbo].[Programs] as p
		where p.StatusID = 1 and p.StartDate <= convert(date, GETDATE()) and p.EndDate >=  convert(date, GETDATE()) and p.MemberID = @MemberID

		Declare @pal float
		
		set @pal = case @ac
			when 6 then 1.2
			when 4 then 1.2
			when 1 then 1.4
			when 2 then 1.6
			when 3 then 1.8
		end

		if @gender = 1
			set @tdee = (10 * @initWeight + 6.25 * @height - 5 * @age + 5) * @pal
		else
			set @tdee = (10 * @initWeight + 6.25 * @height - 5 * @age - 161) * @pal

		
		Declare @totalCalToLose float = (@tgtWeight - @initWeight) * 7700
		Declare @totalDays float
		set @totalDays = DATEDIFF(day, @startDate, @endDate) + 1

		Declare @dailyCalToLose float = @totalCalToLose / @totalDays
		set @coefficient = (@tdee - @dailyCalToLose) / @tdee

		if @coefficient < 0.85
			set @coefficient = 0.85
		else if @coefficient > 1
			set @coefficient = 1
	end
	else
	begin
		
		Declare @weight float
		select top 1 @weight = [Weight] from [dbo].[WeightLog]

		set @pal = case @ac
			when 6 then 1.2
			when 4 then 1.2
			when 1 then 1.4
			when 2 then 1.6
			when 3 then 1.8
		end

		if @gender = 1
			set @tdee = (10 * @weight + 6.25 * @height - 5 * @age + 5) * @pal
		else
			set @tdee = (10 * @weight + 6.25 * @height - 5 * @age - 161) * @pal

		set @coefficient = 1

	end

	--select @MemberID as MemberID, @tdee as TDEE

	Declare @strToday nvarchar(10) = convert(nvarchar(10), getdate(), 101)
	set @strToday = REPLACE(@strToday, '/', '')

	--select @strToday

	Declare @totalIngest float

	select @totalIngest = Sum(d.[Portion] * m.[Calories]) from [dbo].[DietLog] as d 
	join [dbo].[MealOptions] as m on d.[MealOptionID] = m.ID
	where d.[Date] = @strToday and d.[MemberID] = @MemberID

	select @MemberID, @totalIngest / @tdee, @coefficient

	if @totalIngest / @tdee <= @coefficient and @totalIngest / @tdee >= 0.8
	begin
		if @programID = 0
			insert into [dbo].[Points] values (@MemberID, 50, getdate(), 8, null)
		else
			insert into [dbo].[Points] values (@MemberID, 50, getdate(), 8, @programID)
	end

	Delete #Members Where [ID] = @MemberID
End