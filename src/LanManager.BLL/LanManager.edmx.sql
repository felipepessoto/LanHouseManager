
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 08/18/2012 19:58:19
-- Generated from EDMX file: E:\Meus Arquivos\Desenvolvimento\Projetos\LanManager\src\LanManager.BLL\LanManager.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [LanManager];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ClientSession_AccessPoint]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientSession] DROP CONSTRAINT [FK_ClientSession_AccessPoint];
GO
IF OBJECT_ID(N'[dbo].[FK_Application_Application]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Application] DROP CONSTRAINT [FK_Application_Application];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientLog_Application]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientApplicationLog] DROP CONSTRAINT [FK_ClientLog_Application];
GO
IF OBJECT_ID(N'[dbo].[FK_Client_Client]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Client] DROP CONSTRAINT [FK_Client_Client];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientLog_Client]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientApplicationLog] DROP CONSTRAINT [FK_ClientLog_Client];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientSession_Client]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientSession] DROP CONSTRAINT [FK_ClientSession_Client];
GO
IF OBJECT_ID(N'[dbo].[FK_Imagem_Client]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Image] DROP CONSTRAINT [FK_Imagem_Client];
GO
IF OBJECT_ID(N'[dbo].[FK_PeriodAllowed_DependentClient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PeriodAllowed] DROP CONSTRAINT [FK_PeriodAllowed_DependentClient];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientLog_ClientSession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientApplicationLog] DROP CONSTRAINT [FK_ClientLog_ClientSession];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationDependentPermission_Application]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationDependentPermission] DROP CONSTRAINT [FK_ApplicationDependentPermission_Application];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationDependentPermission_Client]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationDependentPermission] DROP CONSTRAINT [FK_ApplicationDependentPermission_Client];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupDependentPermission_ApplicationGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupDependentPermission] DROP CONSTRAINT [FK_GroupDependentPermission_ApplicationGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupDependentPermission_Client]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupDependentPermission] DROP CONSTRAINT [FK_GroupDependentPermission_Client];
GO
IF OBJECT_ID(N'[dbo].[FK_MinutesPurchased_Client]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MinutesPurchased] DROP CONSTRAINT [FK_MinutesPurchased_Client];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AccessPoint]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccessPoint];
GO
IF OBJECT_ID(N'[dbo].[Application]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Application];
GO
IF OBJECT_ID(N'[dbo].[ApplicationGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationGroup];
GO
IF OBJECT_ID(N'[dbo].[Client]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Client];
GO
IF OBJECT_ID(N'[dbo].[ClientApplicationLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientApplicationLog];
GO
IF OBJECT_ID(N'[dbo].[ClientSession]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientSession];
GO
IF OBJECT_ID(N'[dbo].[Image]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Image];
GO
IF OBJECT_ID(N'[dbo].[PeriodAllowed]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PeriodAllowed];
GO
IF OBJECT_ID(N'[dbo].[Admin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Admin];
GO
IF OBJECT_ID(N'[dbo].[MinutesPurchased]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MinutesPurchased];
GO
IF OBJECT_ID(N'[dbo].[Config]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Config];
GO
IF OBJECT_ID(N'[dbo].[ApplicationDependentPermission]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationDependentPermission];
GO
IF OBJECT_ID(N'[dbo].[GroupDependentPermission]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupDependentPermission];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AccessPoint'
CREATE TABLE [dbo].[AccessPoint] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] varchar(100)  NOT NULL,
    [NextCommandToExecute] varchar(50)  NULL
);
GO

-- Creating table 'Application'
CREATE TABLE [dbo].[Application] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [DefaultPath] varchar(max)  NOT NULL,
    [RunArguments] varchar(50)  NULL,
    [Icon] varbinary(max)  NULL,
    [Active] bit  NOT NULL,
    [MinimumAge] int  NOT NULL,
    [InstalledAt] nvarchar(1000)  NULL,
    [ApplicationGroup_Id] uniqueidentifier  NULL
);
GO

-- Creating table 'ApplicationGroup'
CREATE TABLE [dbo].[ApplicationGroup] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'Client'
CREATE TABLE [dbo].[Client] (
    [Id] uniqueidentifier  NOT NULL,
    [UserName] varchar(20)  NOT NULL,
    [Password] varchar(20)  NOT NULL,
    [MaxDebit] int  NOT NULL,
    [CanAccessAnyApplication] bit  NOT NULL,
    [CanAccessAnyTime] bit  NOT NULL,
    [FullName] varchar(100)  NOT NULL,
    [BirthDate] datetime  NOT NULL,
    [DocumentID] varchar(30)  NOT NULL,
    [Nickname] varchar(50)  NULL,
    [Active] bit  NOT NULL,
    [RegisterDate] datetime  NOT NULL,
    [FatherName] varchar(100)  NULL,
    [MotherName] varchar(100)  NULL,
    [StreetAddress] varchar(200)  NULL,
    [City] varchar(50)  NULL,
    [State] char(2)  NULL,
    [Country] varchar(50)  NULL,
    [ZipCode] char(8)  NULL,
    [Phone] bigint  NULL,
    [MobilePhone] bigint  NULL,
    [Email] varchar(50)  NULL,
    [School] varchar(100)  NULL,
    [SchoolTime] int  NULL,
    [HasMidnightPermission] bit  NOT NULL,
    [LastUpdateDate] datetime  NULL,
    [MinutesLeft] int  NOT NULL,
    [MinutesBonus] int  NOT NULL,
    [CPF] nvarchar(11)  NULL,
    [Neighborhood] nvarchar(200)  NULL,
    [PasswordExpired] bit  NOT NULL,
    [Parent_Id] uniqueidentifier  NULL
);
GO

-- Creating table 'ClientApplicationLog'
CREATE TABLE [dbo].[ClientApplicationLog] (
    [Id] uniqueidentifier  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NULL,
    [Application_Id] uniqueidentifier  NOT NULL,
    [Client_Id] uniqueidentifier  NOT NULL,
    [ClientSession_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'ClientSession'
CREATE TABLE [dbo].[ClientSession] (
    [Id] uniqueidentifier  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NULL,
    [LastClientPing] datetime  NOT NULL,
    [MinutesPaid] int  NULL,
    [MinutesBonusUsed] int  NULL,
    [AccessPoint_Id] uniqueidentifier  NOT NULL,
    [Client_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Image'
CREATE TABLE [dbo].[Image] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] varchar(50)  NULL,
    [Description] varchar(200)  NULL,
    [CreationDate] datetime  NOT NULL,
    [ImageBinary] varbinary(max)  NOT NULL,
    [Client_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'PeriodAllowed'
CREATE TABLE [dbo].[PeriodAllowed] (
    [Id] uniqueidentifier  NOT NULL,
    [DayOfWeek] int  NULL,
    [StartHour] int  NOT NULL,
    [EndHour] int  NOT NULL,
    [Client_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Admin'
CREATE TABLE [dbo].[Admin] (
    [Id] uniqueidentifier  NOT NULL,
    [UserName] nvarchar(20)  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [FullName] nvarchar(100)  NOT NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'MinutesPurchased'
CREATE TABLE [dbo].[MinutesPurchased] (
    [Id] uniqueidentifier  NOT NULL,
    [Amount] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Client_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Config'
CREATE TABLE [dbo].[Config] (
    [ShortName] nvarchar(20)  NOT NULL,
    [Value] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'ApplicationDependentPermission'
CREATE TABLE [dbo].[ApplicationDependentPermission] (
    [ApplicationsAllowed_Id] uniqueidentifier  NOT NULL,
    [Client_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GroupDependentPermission'
CREATE TABLE [dbo].[GroupDependentPermission] (
    [ApplicationGroupsAllowed_Id] uniqueidentifier  NOT NULL,
    [Client_Id] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AccessPoint'
ALTER TABLE [dbo].[AccessPoint]
ADD CONSTRAINT [PK_AccessPoint]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Application'
ALTER TABLE [dbo].[Application]
ADD CONSTRAINT [PK_Application]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApplicationGroup'
ALTER TABLE [dbo].[ApplicationGroup]
ADD CONSTRAINT [PK_ApplicationGroup]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Client'
ALTER TABLE [dbo].[Client]
ADD CONSTRAINT [PK_Client]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClientApplicationLog'
ALTER TABLE [dbo].[ClientApplicationLog]
ADD CONSTRAINT [PK_ClientApplicationLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClientSession'
ALTER TABLE [dbo].[ClientSession]
ADD CONSTRAINT [PK_ClientSession]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Image'
ALTER TABLE [dbo].[Image]
ADD CONSTRAINT [PK_Image]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PeriodAllowed'
ALTER TABLE [dbo].[PeriodAllowed]
ADD CONSTRAINT [PK_PeriodAllowed]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Admin'
ALTER TABLE [dbo].[Admin]
ADD CONSTRAINT [PK_Admin]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MinutesPurchased'
ALTER TABLE [dbo].[MinutesPurchased]
ADD CONSTRAINT [PK_MinutesPurchased]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ShortName] in table 'Config'
ALTER TABLE [dbo].[Config]
ADD CONSTRAINT [PK_Config]
    PRIMARY KEY CLUSTERED ([ShortName] ASC);
GO

-- Creating primary key on [ApplicationsAllowed_Id], [Client_Id] in table 'ApplicationDependentPermission'
ALTER TABLE [dbo].[ApplicationDependentPermission]
ADD CONSTRAINT [PK_ApplicationDependentPermission]
    PRIMARY KEY NONCLUSTERED ([ApplicationsAllowed_Id], [Client_Id] ASC);
GO

-- Creating primary key on [ApplicationGroupsAllowed_Id], [Client_Id] in table 'GroupDependentPermission'
ALTER TABLE [dbo].[GroupDependentPermission]
ADD CONSTRAINT [PK_GroupDependentPermission]
    PRIMARY KEY NONCLUSTERED ([ApplicationGroupsAllowed_Id], [Client_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AccessPoint_Id] in table 'ClientSession'
ALTER TABLE [dbo].[ClientSession]
ADD CONSTRAINT [FK_ClientSession_AccessPoint]
    FOREIGN KEY ([AccessPoint_Id])
    REFERENCES [dbo].[AccessPoint]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientSession_AccessPoint'
CREATE INDEX [IX_FK_ClientSession_AccessPoint]
ON [dbo].[ClientSession]
    ([AccessPoint_Id]);
GO

-- Creating foreign key on [ApplicationGroup_Id] in table 'Application'
ALTER TABLE [dbo].[Application]
ADD CONSTRAINT [FK_Application_Application]
    FOREIGN KEY ([ApplicationGroup_Id])
    REFERENCES [dbo].[ApplicationGroup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Application_Application'
CREATE INDEX [IX_FK_Application_Application]
ON [dbo].[Application]
    ([ApplicationGroup_Id]);
GO

-- Creating foreign key on [Application_Id] in table 'ClientApplicationLog'
ALTER TABLE [dbo].[ClientApplicationLog]
ADD CONSTRAINT [FK_ClientLog_Application]
    FOREIGN KEY ([Application_Id])
    REFERENCES [dbo].[Application]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientLog_Application'
CREATE INDEX [IX_FK_ClientLog_Application]
ON [dbo].[ClientApplicationLog]
    ([Application_Id]);
GO

-- Creating foreign key on [Parent_Id] in table 'Client'
ALTER TABLE [dbo].[Client]
ADD CONSTRAINT [FK_Client_Client]
    FOREIGN KEY ([Parent_Id])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Client_Client'
CREATE INDEX [IX_FK_Client_Client]
ON [dbo].[Client]
    ([Parent_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'ClientApplicationLog'
ALTER TABLE [dbo].[ClientApplicationLog]
ADD CONSTRAINT [FK_ClientLog_Client]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientLog_Client'
CREATE INDEX [IX_FK_ClientLog_Client]
ON [dbo].[ClientApplicationLog]
    ([Client_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'ClientSession'
ALTER TABLE [dbo].[ClientSession]
ADD CONSTRAINT [FK_ClientSession_Client]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientSession_Client'
CREATE INDEX [IX_FK_ClientSession_Client]
ON [dbo].[ClientSession]
    ([Client_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'Image'
ALTER TABLE [dbo].[Image]
ADD CONSTRAINT [FK_Imagem_Client]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Imagem_Client'
CREATE INDEX [IX_FK_Imagem_Client]
ON [dbo].[Image]
    ([Client_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'PeriodAllowed'
ALTER TABLE [dbo].[PeriodAllowed]
ADD CONSTRAINT [FK_PeriodAllowed_DependentClient]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PeriodAllowed_DependentClient'
CREATE INDEX [IX_FK_PeriodAllowed_DependentClient]
ON [dbo].[PeriodAllowed]
    ([Client_Id]);
GO

-- Creating foreign key on [ClientSession_Id] in table 'ClientApplicationLog'
ALTER TABLE [dbo].[ClientApplicationLog]
ADD CONSTRAINT [FK_ClientLog_ClientSession]
    FOREIGN KEY ([ClientSession_Id])
    REFERENCES [dbo].[ClientSession]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientLog_ClientSession'
CREATE INDEX [IX_FK_ClientLog_ClientSession]
ON [dbo].[ClientApplicationLog]
    ([ClientSession_Id]);
GO

-- Creating foreign key on [ApplicationsAllowed_Id] in table 'ApplicationDependentPermission'
ALTER TABLE [dbo].[ApplicationDependentPermission]
ADD CONSTRAINT [FK_ApplicationDependentPermission_Application]
    FOREIGN KEY ([ApplicationsAllowed_Id])
    REFERENCES [dbo].[Application]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Client_Id] in table 'ApplicationDependentPermission'
ALTER TABLE [dbo].[ApplicationDependentPermission]
ADD CONSTRAINT [FK_ApplicationDependentPermission_Client]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ApplicationDependentPermission_Client'
CREATE INDEX [IX_FK_ApplicationDependentPermission_Client]
ON [dbo].[ApplicationDependentPermission]
    ([Client_Id]);
GO

-- Creating foreign key on [ApplicationGroupsAllowed_Id] in table 'GroupDependentPermission'
ALTER TABLE [dbo].[GroupDependentPermission]
ADD CONSTRAINT [FK_GroupDependentPermission_ApplicationGroup]
    FOREIGN KEY ([ApplicationGroupsAllowed_Id])
    REFERENCES [dbo].[ApplicationGroup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Client_Id] in table 'GroupDependentPermission'
ALTER TABLE [dbo].[GroupDependentPermission]
ADD CONSTRAINT [FK_GroupDependentPermission_Client]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupDependentPermission_Client'
CREATE INDEX [IX_FK_GroupDependentPermission_Client]
ON [dbo].[GroupDependentPermission]
    ([Client_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'MinutesPurchased'
ALTER TABLE [dbo].[MinutesPurchased]
ADD CONSTRAINT [FK_MinutesPurchased_Client]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[Client]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MinutesPurchased_Client'
CREATE INDEX [IX_FK_MinutesPurchased_Client]
ON [dbo].[MinutesPurchased]
    ([Client_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------