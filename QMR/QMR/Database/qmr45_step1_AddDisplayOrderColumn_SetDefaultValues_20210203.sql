--qmr-45
use [qmr]
go
set ansi_nulls on
go
set quoted_identifier on
go
--
--QMR45:  adding a new column to quarterlyreviewdetails table to handle charts display order
--step 1: add new column and default display order
--

--ALTER TABLE [qmr].dbo.quarterlyreviewdetails
--drop  column DisplayOrder ;
--go

-- add new display order column
alter table [qmr].dbo.quarterlyreviewdetails
add displayorder int null;
go

-- set display order to 0 for all measures in series including reporting and non-reporting.
update [qmr].dbo.quarterlyreviewdetails
set displayorder = 0;
go

-- seed default display order reporting measure for year 2019 and older by measureid asc sequence
merge [QMR].[dbo].[quarterlyreviewdetails] as target
using (
	select row_number() over (partition by qrd.quarterid order by qrd.measureid asc ) as seq
		  ,qrd.[quarterid]
		  ,qrd.[measureid]
		  ,qrd.[id]
		  ,qrd.[currentvalue]
		  ,qrd.[actionid]
		  ,qrd.[trendid]
		  ,qrd.[displayorder]
	from [qmr].dbo.quarterlyreviewdetails qrd
	inner join [qmr].dbo.measure m on qrd.MeasureId = m.MeasureId and m.reporting = 1
	inner join [qmr].dbo.quarterlymeasure qm on qm.QuarterId = qrd.QuarterId and qm.year != 2020
) as source 
on (target.quarterid = source.quarterid and target.measureid = source.measureid 
and target.id = source.id) 
--when records are matched, update the records if there is any change
when matched and isnull(target.displayorder,0) <> source.seq  
then update set target.displayorder  = source.seq
;

-- set not null constraint on the display order column
ALTER TABLE [qmr].dbo.quarterlyreviewdetails
alter column DisplayOrder int not null;





