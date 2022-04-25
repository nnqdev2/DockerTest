use [qmr]
go
set ansi_nulls on
go
set quoted_identifier on
go

--
--QMR45:  adding a new column to quarterlyreviewdetails table to handle charts display order
--step 7: update quarterlyreviewdetails display order for 2020
--

--
-- after seeding the displayorder2020 table, run the below to update the quarterlyreviewdetail display order for 2020
--
merge [QMR].[dbo].quarterlyreviewdetails as target
using 
	(select quarterid, measureid, displayorder 
	from [QMR].[dbo].displayorder2020 
	where reporting = 1) as source 
on (target.quarterid = source.quarterid and target.measureid = source.measureid) 
when matched then update set target.displayorder = source.displayorder
;


--select *
--from [QMR].[dbo].quarterlyreviewdetails 
--where quarterid in (24,25,26,56) and displayorder > 0
--order by quarterid, displayorder;