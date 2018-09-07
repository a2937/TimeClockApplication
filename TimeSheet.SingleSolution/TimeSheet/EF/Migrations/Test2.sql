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

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [TimeSheet].[Companies] (
    [Id] uniqueidentifier NOT NULL,
    [City] nvarchar(50) NULL,
    [CompanyName] nvarchar(50) NULL,
    [StateAbbreviated] nvarchar(2) NULL,
    [StreetAddress] nvarchar(50) NULL,
    [TimeStamp] rowversion NULL,
    [WagesPaid] decimal(18, 2) NOT NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [TimeSheet].[WorkSchedule] (
    [Id] uniqueidentifier NOT NULL,
    [EstimatedWeekPay] decimal(18, 2) NOT NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_WorkSchedule] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey])
);

GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name])
);

GO

CREATE TABLE [Supervisors] (
    [Id] uniqueidentifier NOT NULL,
    [EmployeeId] nvarchar(450) NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_Supervisors] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [TimeSheet].[Departments] (
    [Id] uniqueidentifier NOT NULL,
    [CompanyId] uniqueidentifier NULL,
    [Name] nvarchar(50) NULL,
    [SupervisorId] uniqueidentifier NULL,
    [TimeStamp] rowversion NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Departments_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [TimeSheet].[Companies] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Departments_Supervisors_SupervisorId] FOREIGN KEY ([SupervisorId]) REFERENCES [Supervisors] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [DepartmentId] uniqueidentifier NULL,
    [Email] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [ExceptFromOvertime] bit NOT NULL,
    [FirstName] nvarchar(50) NULL,
    [HourlyWage] decimal(18, 2) NOT NULL,
    [LastName] nvarchar(50) NULL,
    [LockoutEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [MiddleInitial] nvarchar(1) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [PasswordHash] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [UserName] nvarchar(256) NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUsers_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [TimeSheet].[Departments] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [TimeSheet].[WorkDays] (
    [Id] uniqueidentifier NOT NULL,
    [Date] datetime2 NOT NULL DEFAULT (getutcdate()),
    [EmployeeId] nvarchar(450) NULL,
    [EstimatedPay] decimal(18, 2) NOT NULL,
    [HoursWorked] AS DateDiff(hour,[SignIn], [SignOut]),
    [IsApproved] bit NULL DEFAULT (0),
    [IsVacationDay] bit NULL DEFAULT (0),
    [Reason] nvarchar(50) NULL,
    [SignIn] datetimeoffset NULL DEFAULT (getutcdate()),
    [SignOut] datetimeoffset NULL DEFAULT (getutcdate()),
    [TimeStamp] rowversion NULL,
    [WorkScheduleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_WorkDays] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WorkDays_AspNetUsers_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_WorkDays_WorkSchedule_WorkScheduleId] FOREIGN KEY ([WorkScheduleId]) REFERENCES [TimeSheet].[WorkSchedule] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

GO

CREATE INDEX [IX_AspNetUsers_DepartmentId] ON [AspNetUsers] ([DepartmentId]);

GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

CREATE INDEX [IX_Supervisors_EmployeeId] ON [Supervisors] ([EmployeeId]);

GO

CREATE INDEX [IX_Departments_CompanyId] ON [TimeSheet].[Departments] ([CompanyId]);

GO

CREATE INDEX [IX_Departments_SupervisorId] ON [TimeSheet].[Departments] ([SupervisorId]);

GO

CREATE INDEX [IX_WorkDays_EmployeeId] ON [TimeSheet].[WorkDays] ([EmployeeId]);

GO

CREATE INDEX [IX_WorkDays_WorkScheduleId] ON [TimeSheet].[WorkDays] ([WorkScheduleId]);

GO

ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [AspNetUserClaims] ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [AspNetUserLogins] ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [AspNetUserTokens] ADD CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [Supervisors] ADD CONSTRAINT [FK_Supervisors_AspNetUsers_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180315221121_RebuildTablesAfterReplace', N'2.0.1-rtm-125');

GO

ALTER TABLE [TimeSheet].[WorkDays] DROP CONSTRAINT [FK_WorkDays_WorkSchedule_WorkScheduleId];

GO

DROP TABLE [TimeSheet].[WorkSchedule];

GO

DROP INDEX [IX_WorkDays_WorkScheduleId] ON [TimeSheet].[WorkDays];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'TimeSheet.WorkDays') AND [c].[name] = N'WorkScheduleId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [TimeSheet].[WorkDays] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [TimeSheet].[WorkDays] DROP COLUMN [WorkScheduleId];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180316154937_RemoveWorkSchedule', N'2.0.1-rtm-125');

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'TimeSheet.WorkDays') AND [c].[name] = N'IsVacationDay');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [TimeSheet].[WorkDays] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [TimeSheet].[WorkDays] ALTER COLUMN [IsVacationDay] bit NOT NULL;

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'TimeSheet.WorkDays') AND [c].[name] = N'IsApproved');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [TimeSheet].[WorkDays] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [TimeSheet].[WorkDays] ALTER COLUMN [IsApproved] bit NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180316175537_HtmlEditorFor', N'2.0.1-rtm-125');

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'TimeSheet.WorkDays') AND [c].[name] = N'Reason');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [TimeSheet].[WorkDays] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [TimeSheet].[WorkDays] ALTER COLUMN [Reason] nvarchar(100) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180316215540_FixNotLongEnoughReason', N'2.0.1-rtm-125');

GO

