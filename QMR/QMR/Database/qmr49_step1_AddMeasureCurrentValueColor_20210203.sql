--QMR-49

use [qmr]
go
set ansi_nulls on
go
set quoted_identifier on
go

--
--QMR49 - add configurable option for current value color
-- create look up table, populate lookup values and add new column to the measure table
--

----drop table if exists [dbo].[CurrentValueOption];
--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CurrentValueOption]') AND type in (N'U'))
--DROP TABLE [dbo].[CurrentValueOption]
--GO

-- step1 create table
create table [QMR].[dbo].[CurrentValueOption](
	[CurrentValueOptionId] [int] identity(1,1) NOT NULL,
	[CurrentValueOptionDescription] [nvarchar](255) NULL,
 constraint [CurrentValueOption$PrimaryKey] primary key clustered 
(
	[CurrentValueOptionId] asc
)with (pad_index = off, statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on, optimize_for_sequential_key = off) on [primary]
) on [primary]
go

-- step 2 seed table
insert into [QMR].[dbo].[CurrentValueOption] 
([CurrentValueOptionDescription]) values ('Automatic');
insert into [QMR].[dbo].[CurrentValueOption] 
([CurrentValueOptionDescription]) values ('Red');
insert into [QMR].[dbo].[CurrentValueOption] 
([CurrentValueOptionDescription]) values ('Green');
insert into [QMR].[dbo].[CurrentValueOption] 
([CurrentValueOptionDescription]) values ('Yellow');



-- step 3 alter the measure table
--ALTER TABLE [dbo].[Measure] DROP CONSTRAINT [DF__Measure__Current__531856C7]
--GO
--alter table [QMR].[dbo].[Measure] 
--drop column CurrentValueOptionId;

--step 3 add new column to measure table and default value to 1 (Automatic)
alter table [QMR].[dbo].[Measure] 
add CurrentValueOptionId int not null default 1
;


  --update [QMR].[dbo].[Measure]
  --set CurrentValueOptionId = 1
  --where (MeasureId % 2 ) <> 0;

  --select * 
  --FROM [QMR].[dbo].[Measure]
  --where (MeasureId % 2 ) <> 0;
