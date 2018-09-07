IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF SCHEMA_ID(N'TimeSheet') IS NULL EXEC(N'CREATE SCHEMA [TimeSheet];');

GO

CREATE TABLE [TimeSheet].[WorkSchedule] (
    [Id] uniqueidentifier NOT NULL,
    [EstimatedWeekPay] decimal(18, 2) NOT NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_WorkSchedule] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Supervisor] (
    [Id] uniqueidentifier NOT NULL,
    [EmployeeId] nvarchar(450) NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_Supervisor] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [TimeSheet].[Departments] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(50) NULL,
    [SupervisorId] uniqueidentifier NOT NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Departments_Supervisor_SupervisorId] FOREIGN KEY ([SupervisorId]) REFERENCES [Supervisor] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Employees] (
    [Id] nvarchar(450) NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [DepartmentId] uniqueidentifier NOT NULL,
    [Email] nvarchar(max) NULL,
    [EmailConfirmed] bit NOT NULL,
    [ExceptFromOvertime] bit NOT NULL,
    [FirstName] nvarchar(50) NULL,
    [HourlyWage] decimal(18, 2) NOT NULL,
    [LastName] nvarchar(50) NULL,
    [LockoutEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [MiddleInitial] nvarchar(1) NULL,
    [NormalizedEmail] nvarchar(max) NULL,
    [NormalizedUserName] nvarchar(max) NULL,
    [PasswordHash] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [UserName] nvarchar(max) NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Employees_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [TimeSheet].[Departments] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [TimeSheet].[WorkDays] (
    [Id] uniqueidentifier NOT NULL,
    [Date] datetime2 NOT NULL,
    [EmployeeId] nvarchar(450) NULL,
    [EstimatedPay] decimal(18, 2) NOT NULL,
    [HoursWorked] int NOT NULL,
    [IsApproved] bit NOT NULL,
    [Reason] nvarchar(50) NULL,
    [SignIn] datetimeoffset NULL,
    [SignOut] datetimeoffset NULL,
    [TimeStamp] rowversion NULL,
    [WorkScheduleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_WorkDays] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WorkDays_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_WorkDays_WorkSchedule_WorkScheduleId] FOREIGN KEY ([WorkScheduleId]) REFERENCES [TimeSheet].[WorkSchedule] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Employees_DepartmentId] ON [Employees] ([DepartmentId]);

GO

CREATE INDEX [IX_Supervisor_EmployeeId] ON [Supervisor] ([EmployeeId]);

GO

CREATE INDEX [IX_Departments_SupervisorId] ON [TimeSheet].[Departments] ([SupervisorId]);

GO

CREATE INDEX [IX_WorkDays_EmployeeId] ON [TimeSheet].[WorkDays] ([EmployeeId]);

GO

CREATE INDEX [IX_WorkDays_WorkScheduleId] ON [TimeSheet].[WorkDays] ([WorkScheduleId]);

GO

ALTER TABLE [Supervisor] ADD CONSTRAINT [FK_Supervisor_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180310190405_Tables', N'2.0.1-rtm-125');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'TimeSheet.WorkDays') AND [c].[name] = N'SignOut');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [TimeSheet].[WorkDays] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [TimeSheet].[WorkDays] ALTER COLUMN [SignOut] datetimeoffset NULL;
ALTER TABLE [TimeSheet].[WorkDays] ADD DEFAULT (getutcdate()) FOR [SignOut];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'TimeSheet.WorkDays') AND [c].[name] = N'SignIn');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [TimeSheet].[WorkDays] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [TimeSheet].[WorkDays] ALTER COLUMN [SignIn] datetimeoffset NULL;
ALTER TABLE [TimeSheet].[WorkDays] ADD DEFAULT (getutcdate()) FOR [SignIn];

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'TimeSheet.WorkDays') AND [c].[name] = N'IsApproved');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [TimeSheet].[WorkDays] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [TimeSheet].[WorkDays] ALTER COLUMN [IsApproved] bit NULL;
ALTER TABLE [TimeSheet].[WorkDays] ADD DEFAULT (0) FOR [IsApproved];

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'TimeSheet.WorkDays') AND [c].[name] = N'Date');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [TimeSheet].[WorkDays] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [TimeSheet].[WorkDays] ALTER COLUMN [Date] datetime2 NOT NULL;
ALTER TABLE [TimeSheet].[WorkDays] ADD DEFAULT (getutcdate()) FOR [Date];

GO

ALTER TABLE [TimeSheet].[WorkDays] ADD [IsVacationDay] bit NULL DEFAULT (0);

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'TimeSheet.WorkDays') AND [c].[name] = N'HoursWorked');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [TimeSheet].[WorkDays] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [TimeSheet].[WorkDays] DROP COLUMN [HoursWorked];
ALTER TABLE [TimeSheet].[WorkDays] ADD [HoursWorked] AS DateDiff(hour,[SignIn], [SignOut]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180310213856_DefaultValues', N'2.0.1-rtm-125');

GO

