use [qmr]
go
set ansi_nulls on
go
set quoted_identifier on
go

--
--QMR45:  drop the temp table created from previous migration to hold the default 2020 display order
--

drop table if exists [qmr].dbo.displayorder2020;

