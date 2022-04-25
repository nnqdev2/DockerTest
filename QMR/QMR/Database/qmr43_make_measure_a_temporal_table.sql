USE [QMR]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--
--QMR-43 Add history to Measure for changes
--

-- add last updated by column
alter table [qmr].[dbo].[Measure]  
add LastUpdatedBy nvarchar(50);

-- add comment column
alter table [qmr].[dbo].[Measure]  
add Comment nvarchar(4000) null;

-- change title from nvarchar(max) to nvarchar(500)
alter table [qmr].[dbo].[Measure]  
alter column  title nvarchar(500);

-- change description from nvarchar(max) to nvarchar(1000)
alter table [qmr].[dbo].[Measure]  
alter column  description nvarchar(1000);

-- make existing Measure table a temporal table
alter table [qmr].[dbo].[Measure]  
add 
	SysStartTime datetime2 generated always as row start hidden default getutcdate(),
	SysEndTime datetime2 generated always as row end hidden default convert(datetime2, '9999-12-31 23:59:59.9999999'),
	period for system_time (SysStartTime, SysEndTime)
go
alter table [qmr].[dbo].[Measure] set (system_versioning = on (history_table=[dbo].MeasureHistory))
go

