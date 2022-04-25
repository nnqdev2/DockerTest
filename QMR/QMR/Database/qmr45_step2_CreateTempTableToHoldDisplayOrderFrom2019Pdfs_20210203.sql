use [qmr]
go
set ansi_nulls on
go
set quoted_identifier on
go

--
--QMR45:  adding a new column to quarterlyreviewdetails table to handle charts display order
--step 2: create temp table to store the default display order for 2020
--

--drop table if exists [qmr].dbo.displayorder2020;

--create temp table to store default display order
select qm.[quarterid]
      ,qm.[quarter]
      ,qm.[year]
      ,qm.[locked]
	  ,m.Reporting
	  ,qrd.MeasureId
	  ,qrd.displayorder
	  ,m.title
	  ,mg.MeasureGroupName
	  ,mt.MeasureTypeDescription
into [qmr].dbo.displayorder2020
from [qmr].[dbo].[quarterlymeasure]  qm
inner join [qmr].[dbo].[quarterlyreviewdetails]  qrd on qm.quarterid = qrd.quarterid
inner join [qmr].[dbo].[measure] m on qrd.measureid = m.measureid 
inner join [qmr].[dbo].[measuregroups] mg on mg.measuregroupid = m.measuregroupid
inner join [qmr].[dbo].[measuretype] mt on mt.measuretypeid = mg.measuretypeid
where qm.year = 2020
;


--select * from [qmr].dbo.displayorder2020  do
--order by do.quarter asc, do.Reporting desc, do.MeasureTypeDescription, do.MeasureGroupName, do.title;

