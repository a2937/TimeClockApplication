Create Table Employees
(
[Id] INT IDENTITY (1, 1) NOT NULL,
   [FirstName]    NVARCHAR (50) NULL,
   [LastName]     NVARCHAR (50) NULL,
   [ExceptFromOvertime] bit Null,
   [HourlyWage] money Null,
   [HoursWorked] decimal Null,
   [EmailAddress] NVARCHAR (50) NOT NULL,
   [Password]     NVARCHAR (50) NULL,

)
