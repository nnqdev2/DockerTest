use [qmr]
go
set ansi_nulls on
go
set quoted_identifier on
go

--
--QMR45:  adding a new column to quarterlyreviewdetails table to handle charts display order
--step 6: update display order for 4th quarter 2020
--

update [qmr].dbo.displayorder2020
set displayorder = 10
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'VIP Customer Service';

update [qmr].dbo.displayorder2020
set displayorder = 20
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Process measures in the Green';

update [qmr].dbo.displayorder2020
set displayorder = 30
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Outcome measures in the Green';

update [qmr].dbo.displayorder2020
set displayorder = 40
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Percent of measures in red or yellow involved in process improvement';

update [qmr].dbo.displayorder2020
set displayorder = 50
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Workplace Safety';

update [qmr].dbo.displayorder2020
set displayorder = 60
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'VIP Wait time';


update [qmr].dbo.displayorder2020
set displayorder = 70
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'GHG Emissions from Fleet Vehicle Fuel Use';

update [qmr].dbo.displayorder2020
set displayorder = 80
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Analytical Turnaround Time';

update [qmr].dbo.displayorder2020
set displayorder = 90
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'LEAD Quality Systems Measure';


update [qmr].dbo.displayorder2020
set displayorder = 100
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Completeness';

update [qmr].dbo.displayorder2020
set displayorder = 110
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Data Integrity Training';


update [qmr].dbo.displayorder2020
set displayorder = 120
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'LEAD Quality Manual';


update [qmr].dbo.displayorder2020
set displayorder = 130
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Number of Data Corrections Past Due';


update [qmr].dbo.displayorder2020
set displayorder = 140
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Number of Open Corrective Actions Past Due';

update [qmr].dbo.displayorder2020
set displayorder = 150
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title like 'Percentage of Current SOP%s';

update [qmr].dbo.displayorder2020
set displayorder = 160
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Proficiency Testing Performance';


update [qmr].dbo.displayorder2020
set displayorder = 170
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Proficiency Testing Performance -Regulatory Compliance';


update [qmr].dbo.displayorder2020
set displayorder = 180
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Quality Management Review';


update [qmr].dbo.displayorder2020
set displayorder = 190
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Solid Waste Industrial Permits Current';

update [qmr].dbo.displayorder2020
set displayorder = 200
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Solid Waste Composting Permits Current';


update [qmr].dbo.displayorder2020
set displayorder = 210
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and title = 'Solid Waste Municipal Permits Current';

--------------------------

update [qmr].dbo.displayorder2020
set displayorder = 220
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Individual WPCF Permits Current');

--------------------------

update [qmr].dbo.displayorder2020
set displayorder = 230
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Individual ACDP Permits Current');

--------------------------

update [qmr].dbo.displayorder2020
set displayorder = 240
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Individual Title V Permits Current');

--------------------------

update [qmr].dbo.displayorder2020
set displayorder = 250
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Individual NPDES Permits Current');

--------------------------


update [qmr].dbo.displayorder2020
set displayorder = 260
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Solid Waste Tire permits Current');

--------------------------

update [qmr].dbo.displayorder2020
set displayorder = 270
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'WQ Permits Issued to Plan');


update [qmr].dbo.displayorder2020
set displayorder = 280
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Compliance - Tanks - UST');

update [qmr].dbo.displayorder2020
set displayorder = 290
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Timely closure of complaints');

update [qmr].dbo.displayorder2020
set displayorder = 300
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - ACDP Standard Permits - Western Region');


update [qmr].dbo.displayorder2020
set displayorder = 310
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - ACDP Standard Permits - Northwest Region');

update [qmr].dbo.displayorder2020
set displayorder = 320
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - ACDP Standard Permits - Eastern Region');


update [qmr].dbo.displayorder2020
set displayorder = 330
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - ACDP Simple Permits - Western Region');


update [qmr].dbo.displayorder2020
set displayorder = 340
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - ACDP Simple Permits - Eastern Region');



update [qmr].dbo.displayorder2020
set displayorder = 350
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - ACDP Simple Permits - Eastern Region');


update [qmr].dbo.displayorder2020
set displayorder = 360
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - ACDP General Permits - Western Region');



update [qmr].dbo.displayorder2020
set displayorder = 370
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - ACDP General Permits - Eastern Region');



update [qmr].dbo.displayorder2020
set displayorder = 380
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - ACDP Basic Permits - Western Region');



update [qmr].dbo.displayorder2020
set displayorder = 390
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - ACDP Basic Permits - Northwest Region');


update [qmr].dbo.displayorder2020
set displayorder = 400
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - ACDP Basic Permits - Eastern Region');

update [qmr].dbo.displayorder2020
set displayorder = 410
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - Title V Permits - Western Region');


update [qmr].dbo.displayorder2020
set displayorder = 420
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - Title V Permits - Northwest Region');

update [qmr].dbo.displayorder2020
set displayorder = 430
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - Title V Permits - Eastern Region');

update [qmr].dbo.displayorder2020
set displayorder = 440
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - HW LQG facilities - Western Region');


update [qmr].dbo.displayorder2020
set displayorder = 450
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - HW LQG facilities - Northwest Region');

update [qmr].dbo.displayorder2020
set displayorder = 460
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - HW LQG facilities - Eastern Region');


update [qmr].dbo.displayorder2020
set displayorder = 470
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - HW LQG facilities - Northwest Region');


update [qmr].dbo.displayorder2020
set displayorder = 480
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - HW LQG facilities - Eastern Region');


update [qmr].dbo.displayorder2020
set displayorder = 490
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - HW SQG facilities - Western Region');


update [qmr].dbo.displayorder2020
set displayorder = 500
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - HW SQG facilities - Northwest Region');

update [qmr].dbo.displayorder2020
set displayorder = 510
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - HW SQG facilities - Eastern Region');

update [qmr].dbo.displayorder2020
set displayorder = 520
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - Solid Waste Permits - Western Region');

update [qmr].dbo.displayorder2020
set displayorder = 530
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - Solid Waste Permits - Northwest Region');


update [qmr].dbo.displayorder2020
set displayorder = 540
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - Solid Waste Permits - Eastern Region');

update [qmr].dbo.displayorder2020
set displayorder = 550
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - WQ Major Individual Permits - Western Region');

update [qmr].dbo.displayorder2020
set displayorder = 560
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - WQ Major Individual Permits - Northwest Region');

update [qmr].dbo.displayorder2020
set displayorder = 570
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - WQ Major Individual Permits - Eastern Region');


update [qmr].dbo.displayorder2020
set displayorder = 580
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - WQ Minor Individual Permits - Western Region');

update [qmr].dbo.displayorder2020
set displayorder = 590
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - WQ Minor Individual Permits - Northwest Region');


update [qmr].dbo.displayorder2020
set displayorder = 600
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - WQ Minor Individual Permits - Eastern Region');


update [qmr].dbo.displayorder2020
set displayorder = 610
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title = 'Inspections conducted on schedule - WQ Minor Individual Permits - Eastern Region');

----------------------------------------------

-- not in dev
update [qmr].dbo.displayorder2020
set displayorder = 620
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Construction Stormwater > 5 Acres%Western Region');

update [qmr].dbo.displayorder2020
set displayorder = 630
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Construction Stormwater > 5 Acres%Northwest Region');

update [qmr].dbo.displayorder2020
set displayorder = 640
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Construction Stormwater > 5 Acres%Eastern Region');

--------------------------------

-- not in dev

update [qmr].dbo.displayorder2020
set displayorder = 650
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Construction Stormwater < 5 Acres%Western Region');

update [qmr].dbo.displayorder2020
set displayorder = 660
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Construction Stormwater < 5 Acres%Northwest Region');



update [qmr].dbo.displayorder2020
set displayorder = 670
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Construction Stormwater < 5 Acres%Eastern Region');

--------------------------

--title like 'Inspections conducted on schedule - Industrial Stormwater - %'
-- in 2019 but not 2020
update [qmr].dbo.displayorder2020
set displayorder = 680
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Industrial Stormwater%Western Region');

update [qmr].dbo.displayorder2020
set displayorder = 690
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Industrial Stormwater%Northwest Region');

------

update [qmr].dbo.displayorder2020
set displayorder = 700
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Air Quality');

update [qmr].dbo.displayorder2020
set displayorder = 710
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Water Quality');

----
update [qmr].dbo.displayorder2020
set displayorder = 720
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Hazardous & Solid Waste');

----
update [qmr].dbo.displayorder2020
set displayorder = 730
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'UST SOC Inspections conducted on schedule');

----
update [qmr].dbo.displayorder2020
set displayorder = 740
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule - ACDP Simple Permits - Northwest Region');

update [qmr].dbo.displayorder2020
set displayorder = 750
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule - ACDP General Permits - Northwest Region');

update [qmr].dbo.displayorder2020
set displayorder = 760
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Inspections conducted on schedule%Air Quality');

----
update [qmr].dbo.displayorder2020
set displayorder = 770
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Timeliness of issuing formal enforcement actions');


----
update [qmr].dbo.displayorder2020
set displayorder = 780
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Resolved compliance orders');


----
update [qmr].dbo.displayorder2020
set displayorder = 790
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Resolved compliance orders ("other" orders)');

----
update [qmr].dbo.displayorder2020
set displayorder = 800
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Resolved compliance orders (default final orders)');
----
update [qmr].dbo.displayorder2020
set displayorder = 810
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Resolved compliance orders (MAOs)');
----
update [qmr].dbo.displayorder2020
set displayorder = 815
where (year=2020) and (quarter in ('4th quarter'))
and (title like 'Proposed Orders in Contested Case Hearings that ALJ upheld all violations alleged.');
-----
update [qmr].dbo.displayorder2020
set displayorder = 820
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Timely completion of records requests');

update [qmr].dbo.displayorder2020
set displayorder = 830
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Records requests in compliance');

update [qmr].dbo.displayorder2020
set displayorder = 831
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Records requests over 15 business days - percent in compliance');


update [qmr].dbo.displayorder2020
set displayorder = 832
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Records requests under 15 business days');

----
update [qmr].dbo.displayorder2020
set displayorder = 835
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Cost of time Lost');


update [qmr].dbo.displayorder2020
set displayorder = 840
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Cost of medical expenses');
--
update [qmr].dbo.displayorder2020
set displayorder = 850
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Facility/site inspections completed');
--
update [qmr].dbo.displayorder2020
set displayorder = 860
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Safety hazards corrected by deadline');
--
update [qmr].dbo.displayorder2020
set displayorder = 870
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Number of accidents per miles driven statewide');
--
update [qmr].dbo.displayorder2020
set displayorder = 880
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Days to hire');

--
update [qmr].dbo.displayorder2020
set displayorder = 890
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Employees engaged in career development');
--
update [qmr].dbo.displayorder2020
set displayorder = 900
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Meeting mileage requirements');
---

-- SPOTS Log Error Rate not in 4quarter --  only in 1,2 ,3
-- in dev SPOTS is in 4th quarter and not in 1, 2, 3
-- not in dev
update [qmr].dbo.displayorder2020
set displayorder = 910
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'SPOTS Log Error Rate');


-- finishing 1,2 and 3 quarter to the end
-- starts
update [qmr].dbo.displayorder2020
set displayorder = 920
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Deposit Timeliness');

update [qmr].dbo.displayorder2020
set displayorder = 930
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Accounts Payable Timeliness');

update [qmr].dbo.displayorder2020
set displayorder = 940
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Technology Project Tracking');


update [qmr].dbo.displayorder2020
set displayorder = 950
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'Percent of EDMS project milestones achieved');


update [qmr].dbo.displayorder2020
set displayorder = 960
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'IT Service Desk Request Resolution Time');

update [qmr].dbo.displayorder2020
set displayorder = 970
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'IT System Uptime');

update [qmr].dbo.displayorder2020
set displayorder = 980
where year = 2020 and quarter = '4th quarter' and reporting = 1 
and (title like 'IT Service Satisfaction');

update [qmr].dbo.displayorder2020
set displayorder = 990
where  (year=2020) and (quarter in ('4th quarter'))
and (title like 'Employee Engagement Survey');

---

