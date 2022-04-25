using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using QMR.Common;
using QMR.Data;
using QMR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace QMR.Controllers
{
    public class MeasuresController : BaseController
    { 
        private readonly QMRContext _context;
        Measure measure;
        MeasureType measureType;
        MeasureGroupType measureGroupType;
        MeasureGroups measureGroup;
        private string sucessMessagae = "";
        private static ICollection<string> MeasureOwner = new List<string>() ;
      
        public MeasuresController(QMRContext context)
        {
            _context = context;
            measureType = new MeasureType();
            measureGroup = new MeasureGroups();
        }

        // GET: Measures/Create
        public IActionResult Create()
        {
            SetSessionInfo();
            measure = new Measure();
            measureGroupType = new MeasureGroupType();
            measure.MeasureGroupType = measureGroupType;
            measure.MeasureGroupType.MeasureType = QMRTLookUp.GetMeasureType(_context);
            measure.MeasureGroupType.MeasureGroup = QMRTLookUp.GetMeasureGroup(_context);
            measure.MeasureGroupType.Season = QMRTLookUp.GetSeason(_context);
            measure.MeasureUnitList= QMRTLookUp.GetAllMeasureUnits(_context);
            measure.MeasuresForSelectedGroup = QMRTLookUp.GetAllMeasure(_context);
            measure.CurrentValueOptions = QMRTLookUp.GetCurrentValueOptions(_context);
            MeasureOwner = QMRTLookUp.GetMeasureOwner();
            return View(measure);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MeasureGroupType,MeasureId,Title,Description,MeasureGroupId,RollUpMeasure,Target,GreenRange,RedRange,MeasureOwner,DataSteward" +
            ",Status,MeasuresForSelectedGroup,MeasureUnit,Reporting,SelectedCurrentValueOption,Comment")] Measure measure)
        {
            SetSessionInfo();
            sucessMessagae = "";
            if (ModelState.IsValid)
            {
               if (measure.MeasureGroupType.measureGroupType.ToString() == "Type")
                    {
                    if (!TypeExists(measure.Title))
                    {
                        measureType.MeasureTypeDescription = measure.Title;
                        _context.Add(measureType);
                        await _context.SaveChangesAsync();
                        sucessMessagae = "Type";
                    }
                    TempData["Success"] = "Type already Exist!";
                }
               else if (measure.MeasureGroupType.measureGroupType.ToString() == "Group")
                    {
                    if (!GroupExists(measure.Title))
                    {
                        measureGroup.MeasureGroupName = measure.Title;
                        measureGroup.MeasureTypeId = Convert.ToInt32(QMRTLookUp.GetMeasureTypeId(_context,measure.MeasureGroupType.SelectedType));
                        measureGroup.MeasureGroupStatus = measure.Status;
                        _context.Add(measureGroup);
                        await _context.SaveChangesAsync();
                        sucessMessagae = "Group";
                    }
                    TempData["Success"] = "Group already Exist!";
                }
               else if (measure.MeasureGroupType.measureGroupType.ToString() == "Measure")
                    {
                    if (!MeasureNameExists(measure.Title))
                    {
                        measure.SeasonId = Convert.ToInt32(QMRTLookUp.GetSeasonId(_context, measure.MeasureGroupType.SelectedSeason));
                        measure.CurrentValueOptionId = Convert.ToInt32(QMRTLookUp.GetCurrentValueOptionId(_context, measure.SelectedCurrentValueOption));
                        measure.MeasureGroupId = QMRTLookUp.GetMeasureGroupId(_context, measure.MeasureGroupType.SelectedGroup);
                        measure.LastUpdatedBy = HttpContext.User.Identity.Name;
                        _context.Add(measure);
                        await _context.SaveChangesAsync();
                        sucessMessagae = "Measure";
                    }
                    TempData["Success"] = "Measure already Exist!";
                }
               else if (measure.MeasureGroupType.measureGroupType.ToString() == "RollUp")
                    {
                    measure.MeasureId=QMRTLookUp.GetMeasureId(_context, measure.Title.ToString());
                    foreach (var item in measure.MeasuresForSelectedGroup)
                    {
                        RollUpMeasures rollUpMeasures = new RollUpMeasures();
                        rollUpMeasures.RollUpMeasureId = measure.MeasureId;
                        rollUpMeasures.MeasureId = QMRTLookUp.GetMeasureId(_context, item);
                        _context.Add(rollUpMeasures);
                        await _context.SaveChangesAsync();
                    }
                        measure= (from d in _context.Measure
                                  where d.Title == measure.Title
                                  select d).SingleOrDefault(); 
                        measure.RollUpMeasure = true;
                        _context.Update(measure);
                        await _context.SaveChangesAsync();
                    measureGroupType = new MeasureGroupType();
                    measure.MeasureGroupType = measureGroupType;
                    sucessMessagae = "RollUp Measure";
                    }
               if(sucessMessagae!="")
               TempData["Success"] = sucessMessagae + " saved successfully!"; 
            }
             if (measure.MeasureGroupType.MeasureType == null)
            {
                measure.MeasureGroupType.MeasureType = QMRTLookUp.GetMeasureType(_context);
                measure.MeasureGroupType.MeasureGroup = QMRTLookUp.GetMeasureGroup(_context);
                measure.MeasureGroupType.Season= QMRTLookUp.GetSeason(_context);
                measure.MeasuresForSelectedGroup = QMRTLookUp.GetAllMeasure(_context);
                measure.MeasureUnitList = QMRTLookUp.GetAllMeasureUnits(_context);
                measure.CurrentValueOptions = QMRTLookUp.GetCurrentValueOptions(_context);
                ModelState.Clear();
            }
            return View(measure);
        }
        // GET: Measures/Edit/5
        public async Task<IActionResult> Edit(int? id,string? MeasureTypeGroup)
        {
            SetSessionInfo();
            if (id == null)
            {
                return NotFound();
            }

            var measure = await _context.Measure.FindAsync(id);

            measureGroupType = new MeasureGroupType();
            measure.MeasureGroupType = measureGroupType;
            measure.MeasureGroupType.MeasureType = QMRTLookUp.GetMeasureType(_context);
            measure.MeasureGroupType.MeasureGroup = QMRTLookUp.GetMeasureGroup(_context);
            measure.MeasureGroupType.Season = QMRTLookUp.GetSeason(_context);
            measure.CurrentValueOptions = QMRTLookUp.GetCurrentValueOptions(_context);
            var measureDetails = (from m in _context.Measure.ToList()
                                  join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                  from mg in table1.ToList()
                                  join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                  from mt in table2.ToList()
                                  join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                  from s in table3.ToList() 
                                  where m.MeasureId==id
                                  select new MeasureGroupType
                                  {
                                      SelectedType = mt.MeasureTypeDescription,
                                      SelectedGroup = mg.MeasureGroupName,
                                      SelectedSeason = s.SeasonName,
                                  });
            foreach (var item in measureDetails)
            {
                measure.MeasureGroupType.measureGroupType = MeasureTypeGroup;
                measure.MeasureGroupType.SelectedType = item.SelectedType;
                measure.MeasureGroupType.SelectedGroup = item.SelectedGroup;
                measure.MeasureGroupType.SelectedSeason = item.SelectedSeason;
            }
            MeasureOwner = QMRTLookUp.GetMeasureOwner();
            measure.SelectedCurrentValueOption = QMRTLookUp.GetCurrentValueOptionDescription(_context, measure.CurrentValueOptionId);
            if (measure == null)
            {
                return NotFound();
            }
            return View(measure);
        }
        [HttpPost]
        public async Task<IActionResult> Save(int id, bool MeasureStatus)
        {
            SetSessionInfo();
            var measure = await _context.Measure.FindAsync(id);
            measure.Status = MeasureStatus;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(measure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeasureExists(measure.MeasureId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DisplayMeasures));
            }
            return View(measure);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MeasureGroupType,MeasureId,Title,Description,MeasureGroupId,RollUpMeasure,Target,GreenRange,RedRange,MeasureOwner,DataSteward,Status,MeasureUnit,Reporting,SelectedCurrentValueOption,Comment")] Measure measure)
        {
            SetSessionInfo();
            MeasureGroupType measureGroupType = new MeasureGroupType();
            MeasureType measureType = new MeasureType();
            MeasureGroups measureGroup = new MeasureGroups();
            if (id != measure.MeasureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                { 
                    if(measure.MeasureGroupType.measureGroupType.ToString()=="Type")
                    {
                        measureType.MeasureTypeDescription = measure.Title;
                        _context.Update(measureType);
                        await _context.SaveChangesAsync();
                        sucessMessagae = "Type";
                    }
                    else if (measure.MeasureGroupType.measureGroupType.ToString() == "Group")
                    {
                        measureGroup.MeasureGroupName = measure.Title;
                        measureGroup.MeasureTypeId =1;
                        measureGroup.MeasureGroupStatus = measure.Status;
                        _context.Update(measureGroup);
                        await _context.SaveChangesAsync();
                        sucessMessagae = "Group";
                    }
                    else if (measure.MeasureGroupType.measureGroupType.ToString() == "Measure")
                    {
                        measure.SeasonId = Convert.ToInt32(QMRTLookUp.GetSeasonId(_context, measure.MeasureGroupType.SelectedSeason));
                        measure.MeasureGroupId = QMRTLookUp.GetMeasureGroupId(_context, measure.MeasureGroupType.SelectedGroup);
                        measure.CurrentValueOptionId = QMRTLookUp.GetCurrentValueOptionId(_context, measure.SelectedCurrentValueOption);
                        measure.LastUpdatedBy = HttpContext.User.Identity.Name;
                        _context.Update(measure);
                        await _context.SaveChangesAsync();
                        sucessMessagae = "Measure";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeasureExists(measure.MeasureId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Success"] = sucessMessagae + " saved successfully at " + DateTime.Now.ToLocalTime();
                ModelState.Clear();
                return RedirectToAction(nameof(Edit), "Measures", new { id = id, MeasureTypeGroup = sucessMessagae });
            }
            if (measure.MeasureGroupType.MeasureType == null)
            {
                measure.MeasureGroupType.MeasureType = QMRTLookUp.GetMeasureType(_context);
                measure.MeasureGroupType.MeasureGroup = QMRTLookUp.GetMeasureGroup(_context);
                measure.MeasureGroupType.Season = QMRTLookUp.GetSeason(_context);
                measure.MeasuresForSelectedGroup = QMRTLookUp.GetAllMeasure(_context);
                ModelState.Clear();
            }
            return View(measure);
        }
        private bool MeasureExists(int id)
        {
            return _context.Measure.Any(e => e.MeasureId == id);
        }
        private bool MeasureNameExists(string measureName)
        {
            return _context.Measure.Any(e => e.Title == measureName);
        }
        private bool TypeExists(string typeName)
        {
            return _context.MeasureType.Any(e => e.MeasureTypeDescription == typeName);
        }
        private bool GroupExists(string GroupName)
        {
            return _context.MeasureGroups.Any(e => e.MeasureGroupName == GroupName);
        }
        [HttpGet]
        public JsonResult MeasureByGroup(string Group=null)
        {
            List<string> Measures;
            if (Group != null)
            {
                Measures = QMRTLookUp.GetMeasurebyselectedGroup(_context, Group);
            }
            else
            {
                Measures = new List<string>();
            }
            Measures.Insert(0, ""); //add the initial selected value of nothing
            return Json(Measures);
        }
        [HttpGet]
        public IActionResult AutoCompleteMeasureOwner(string term)
        {
            SetSessionInfo();
            var result = (from member in MeasureOwner
                          where member.ToLower().StartsWith(term)
                          select new {value=member }).ToList();
            return Json(result);
        }
        public IActionResult DisplayMeasures()
        {
            SetSessionInfo();
            var measureDetails = (from m in _context.Measure.ToList()
                                      join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                      from mg in table1.ToList()
                                      join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                      from mt in table2.ToList()
                                      join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                      from s in table3.ToList()
                                      select new MeasureDetails
                                      {
                                          MeasureId = m.MeasureId,
                                          MeasureTypeDescription = mt.MeasureTypeDescription,
                                          measureGroupName = mg.MeasureGroupName,
                                          Title = m.Title,
                                          Description = m.Description,
                                          RollUpMeasure = m.RollUpMeasure ? "Yes" : "",
                                          MeasureUnit =m.MeasureUnit,
                                          Target = m.Target,
                                          GreenRange = m.GreenRange,
                                          RedRange = m.RedRange,
                                          SeasonName = s.SeasonName,
                                          MeasureOwner = m.MeasureOwner,
                                          Status = m.Status,
                                          DataSteward = m.DataSteward
                                      }
                                );
                return View(measureDetails);
        }
        public IActionResult Chart(int measureId)
        {
            var ChartData = (from qm in _context.QuarterlyMeasure.OrderBy(x => x.Year).ThenBy(x => x.Quarter).ToList()
                             join QRD in _context.QuarterlyReviewDetails.ToList() on qm.QuarterId equals QRD.QuarterId into table1
                             from QRD in table1.ToList()
                             join m in _context.Measure.ToList() on QRD.MeasureId equals m.MeasureId into table2
                             from m in table2.ToList()
                             where QRD.MeasureId == measureId
                             select new ChartData
                             {
                                 MeasureId = QRD.MeasureId,
                                 QuarterDetail = QMRTLookUp.GetQuarter(_context, qm.QuarterId),
                                 CurrentValue = Math.Round(QRD.CurrentValue, 3),
                                 GreenRange = m.GreenRange,
                                 RedRange = m.RedRange,
                                 MeasureUnit = m.MeasureUnit
                             }
                                ).Reverse().Take(12).Reverse().ToList();
            if (ChartData.Count > 0)
            {
                var maxCurrentValue = ChartData.Max(x => x.CurrentValue);
                for (int i = 0; i < ChartData.Count; i++)
                {
                    if ((ChartData[i].GreenRange - ChartData[i].RedRange) > 0)
                    {
                        if ((maxCurrentValue - ChartData[i].GreenRange) > 0)
                            ChartData[i].Greenheight = string.Concat(((maxCurrentValue - ChartData[i].GreenRange) * 70 / maxCurrentValue).ToString(), "%");
                        else
                            ChartData[i].Greenheight = "0%";
                        if ((maxCurrentValue - ChartData[i].RedRange) > 0)
                            ChartData[i].Yellowheight = string.Concat(((maxCurrentValue - ChartData[i].RedRange) * 70 / maxCurrentValue).ToString(), "%");
                        else
                            ChartData[i].Yellowheight = "0%";
                        ChartData[i].Redheight = "70%";
                    }
                    else
                    {
                        ChartData[i].Greenheight = "70%";
                        if ((maxCurrentValue - ChartData[i].GreenRange) > 0)
                            ChartData[i].Yellowheight = string.Concat((((maxCurrentValue - ChartData[i].GreenRange) * 70 / maxCurrentValue)).ToString(), "%");
                        else
                            ChartData[i].Yellowheight = "0%";
                        if ((maxCurrentValue - ChartData[i].RedRange) > 0)
                            ChartData[i].Redheight = string.Concat((((maxCurrentValue - ChartData[i].RedRange) * 70 / maxCurrentValue)).ToString(), "%");
                        else
                            ChartData[i].Redheight = "0%";
                    }
                }
            }
            return Json(ChartData);
        }






        [HttpGet]
        public IActionResult MeasureHistory(int? id)
        {
            SetSessionInfo();
            var measureHistory = (from m in _context.MeasureHistory.ToList()
                                  join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                  from mg in table1.ToList()
                                  join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                  from mt in table2.ToList()
                                  join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                  from s in table3.ToList()
                                  join cvo in _context.CurrentValueOption.ToList() on m.CurrentValueOptionId equals cvo.CurrentValueOptionId into table4
                                  from cvo in table4.ToList()
                                  where m.MeasureId == id
                                  orderby m.SysStartTime descending
                                  select new MeasureHistory
                                  {
                                      MeasureId = m.MeasureId,
                                      MeasureTypeDescription = mt.MeasureTypeDescription,
                                      measureGroupName = mg.MeasureGroupName,
                                      Title = m.Title,
                                      Description = m.Description,
                                      RollUpMeasureString = m.RollUpMeasure ? "Yes" : "",
                                      MeasureUnit = m.MeasureUnit,
                                      Target = m.Target,
                                      GreenRange = m.GreenRange,
                                      RedRange = m.RedRange,
                                      SeasonName = s.SeasonName,
                                      MeasureOwner = m.MeasureOwner,
                                      Status = m.Status,
                                      DataSteward = m.DataSteward,
                                      ReportingString = m.Reporting ? "Yes" : "",
                                      LastUpdatedBy = m.LastUpdatedBy,
                                      CurrentValueOptionDescription = cvo.CurrentValueOptionDescription,
                                      Comment = m.Comment,
                                      SysStartTime = m.SysStartTime.ToLocalTime(),
                                      SysEndTime = m.SysEndTime.ToLocalTime()
                                  }
                                );
            return View(measureHistory);
        }
    }
}
