use HealthHelper

Update [dbo].[WorkoutLog]
set [StatusID] = 6
where [StatusID] = 4 and CONVERT(date, WorkoutTime) = CONVERT(date, GETDATE())