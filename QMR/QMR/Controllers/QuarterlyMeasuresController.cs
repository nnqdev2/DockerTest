using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QMR.Data;
using QMR.Models;
using QMR.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography.X509Certificates;

namespace QMR.Controllers
{
    public class QuarterlyMeasuresController : BaseController
    {
        private readonly QMRContext _context;
        public QuarterlyMeasuresController(QMRContext context)
        {
            _context = context;
        }
        // GET: QuarterlyMeasures
        public IActionResult Detail()
        {
            SetSessionInfo();
            var measureCount = _context.Measure.Count(m => m.Status == true);

            int AppCalculatedMeasure = (from m in _context.Measure.Where(m=>(m.RollUpMeasure == true || m.DataSteward == "QMR OBM" ) && m.Status == true).ToList()
                                        select m.MeasureId).Count();

            int currentReportingQuarterId = (from qm in _context.QuarterlyMeasure.Where(x => (x.Locked == false)).ToList()
                                       .OrderBy(x => x.Year).ThenBy(x => x.Quarter)
                                       select qm.QuarterId).FirstOrDefault();

            var quarterlyMeasure = (from m in _context.QuarterlyMeasure.OrderByDescending(x => x.Year).ThenByDescending(x => x.Quarter).ToList()
                                    select new QuarterlyMeasure
                                    {
                                        QuarterId = m.QuarterId,
                                        Quarter = m.Quarter,
                                        Year = m.Year,
                                        Locked = m.Locked,
                                        MeasureCollected = _context.QuarterlyReviewDetails.Count(QRD => (QRD.CurrentValue != 0 || QRD.ActionId != 1 || QRD.TrendId != 1) && QRD.QuarterId == m.QuarterId),
                                        MeasureCounted = m.Locked == true ? _context.QuarterlyReviewDetails.Count(QRD => QRD.QuarterId == m.QuarterId) : _context.QuarterlyReviewDetails.Count(QRD => QRD.QuarterId == m.QuarterId) - AppCalculatedMeasure,
                                        IsCurrentReportingQuarter = m.QuarterId == currentReportingQuarterId ? true : false
                                    }
                                   ); ;
            return View(quarterlyMeasure);
        }
        public IActionResult DetailDataSteward()
        {
            SetSessionInfo();
            var measureCount = _context.Measure.Count(m => m.Status == true);

            var quarterlyMeasure = (from m in _context.QuarterlyMeasure.OrderByDescending(x => x.Year).ThenByDescending(x => x.Quarter).ToList()
                                    select new QuarterlyMeasure
                                    {
                                        QuarterId = m.QuarterId,
                                        Quarter = m.Quarter,
                                        Year = m.Year,
                                        Locked = m.Locked,
                                        MeasureCollected = _context.QuarterlyReviewDetails.Count(QRD => QRD.CurrentValue != 0 && QRD.QuarterId == m.QuarterId),
                                        MeasureCounted = _context.QuarterlyReviewDetails.Count(QRD => QRD.QuarterId == m.QuarterId)
                                    }
                                   );
            return View(quarterlyMeasure);
        }
        public IActionResult QuarterlyRollupSeries()
        {
            SetSessionInfo();
            var measureCount = _context.Measure.Count(m => m.Status == true);

            var quarterlyMeasure = (from m in _context.QuarterlyMeasure.OrderByDescending(x => x.Year).ThenByDescending(x => x.Quarter).ToList()
                                    select new QuarterlyMeasure
                                    {
                                        QuarterId = m.QuarterId,
                                        Quarter = m.Quarter,
                                        Year = m.Year,
                                        Locked = m.Locked,
                                        MeasureCollected = _context.QuarterlyReviewDetails.Count(QRD => (QRD.CurrentValue != 0 || QRD.ActionId != 1 || QRD.TrendId != 1) && QRD.QuarterId == m.QuarterId),
                                        MeasureCounted = _context.QuarterlyReviewDetails.Count(QRD => QRD.QuarterId == m.QuarterId)
                                    }
                                   );
            return View(quarterlyMeasure);
        }
        // GET: QuarterlyMeasures/Create
        public IActionResult Create()
        {
            SetSessionInfo();
            return View();
        }
        // POST: QuarterlyMeasures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuarterId,Quarter,Year")] QuarterlyMeasure quarterlyMeasure)
        {
            SetSessionInfo();
            if (ModelState.IsValid)
            {
                if (!QuarterlySeriesExists(quarterlyMeasure))
                    {
                    _context.Add(quarterlyMeasure);
                    await _context.SaveChangesAsync();
                    int seasonId = QMRTLookUp.GetSeasonId(_context, quarterlyMeasure.Quarter);
                    var sameQuarterLastYearQuarterId = QMRTLookUp.GetQuarterId(_context, (quarterlyMeasure.Year-1), quarterlyMeasure.Quarter);
                    var quarterlyReviewDetails = (from m in _context.Measure
                                   join qrd in _context.QuarterlyReviewDetails
                                   on new { m.MeasureId, mQuarterId = sameQuarterLastYearQuarterId } equals
                                      new { qrd.MeasureId, mQuarterId = qrd.QuarterId }  into temp1
                                   from t1 in temp1.DefaultIfEmpty()
                                    where m.Status == true && (m.SeasonId == seasonId || m.SeasonId == 5)
                                   select new QuarterlyReviewDetails
                                   {
                                       MeasureId = m.MeasureId,
                                       QuarterId = quarterlyMeasure.QuarterId,
                                       CurrentValue = 0,
                                       ActionId = 1,
                                       TrendId = 1,
                                       DisplayOrder = t1.DisplayOrder,
                                       Reporting = m.Reporting
                                   });
                    var maxDisplayOrder = (int)_context.QuarterlyReviewDetails.Where(e => e.QuarterId == sameQuarterLastYearQuarterId).Max(x => x.DisplayOrder);
                    foreach (QuarterlyReviewDetails quarterlyReview in quarterlyReviewDetails)
                    {
                        if (quarterlyReview.Reporting) 
                        {
                            if (quarterlyReview.DisplayOrder is null || quarterlyReview.DisplayOrder == 0)
                                quarterlyReview.DisplayOrder = ++maxDisplayOrder;
                        } else
                        {
                            quarterlyReview.DisplayOrder = 0;
                        }
                        _context.QuarterlyReviewDetails.Add(quarterlyReview);
                    }
                    await _context.SaveChangesAsync();
                    TempData["Success"] = " Quarterly Measures Series saved successfully!";
                    return View(quarterlyMeasure);
                }
            }
            TempData["Success"] = "Series already exists!";
            return RedirectToAction(nameof(Detail));
        }

        // GET: QuarterlyMeasures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            SetSessionInfo();
            if (id == null)
            {
                return NotFound();
            }

            var quarterlyMeasure = await _context.QuarterlyMeasure.FindAsync(id);
            if (quarterlyMeasure == null)
            {
                return NotFound();
            }
            return View(quarterlyMeasure);
        }
        // POST: QuarterlyMeasures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuarterId,Quarter,Year")] QuarterlyMeasure quarterlyMeasure)
        {
            SetSessionInfo();
            if (id != quarterlyMeasure.QuarterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quarterlyMeasure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuarterlyMeasureExists(quarterlyMeasure.QuarterId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(quarterlyMeasure);
        }

        // GET: QuarterlyMeasures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            SetSessionInfo();
            if (id == null)
            {
                return NotFound();
            }

            var quarterlyMeasure = await _context.QuarterlyMeasure
                .FirstOrDefaultAsync(m => m.QuarterId == id);
            if (quarterlyMeasure == null)
            {
                return NotFound();
            }

            return View(quarterlyMeasure);
        }

        // POST: QuarterlyMeasures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SetSessionInfo();
            var quarterlyMeasure = await _context.QuarterlyMeasure.FindAsync(id);
            _context.QuarterlyMeasure.Remove(quarterlyMeasure);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuarterlyMeasureExists(int id)
        {
            return _context.QuarterlyMeasure.Any(e => e.QuarterId == id);
        }
        private bool QuarterlySeriesExists(QuarterlyMeasure qm)
        {
            return _context.QuarterlyMeasure.Any(e => e.Quarter == qm.Quarter && e.Year == qm.Year);
        }
        public ActionResult QuarterlyReview(int id)
        {
            SetSessionInfo();
            TempData["QuarterId"] = id;
            TempData["QuarterStatus"] = QMRTLookUp.GetQuarterStatus(_context, id);
            TempData["Quarter"] = QMRTLookUp.GetQuarter(_context, id);
            TempData["MeasureCount"] = _context.QuarterlyReviewDetails.Count(QRD => QRD.QuarterId == id);
            TempData["Measurecollected"] = _context.QuarterlyReviewDetails.Count(m => (m.CurrentValue !=0 || m.ActionId!=1 || m.TrendId != 1) &&  m.QuarterId==id);
           
            if (HttpContext.Session.GetString("IsAdmin")=="True" || TempData["QuarterStatus"].ToString()=="True")
            {
                var  quarterlyReviewDetails = (from m in _context.Measure.ToList()
                                              join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                              from mg in table1.ToList()
                                              join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                              from mt in table2.ToList()
                                              join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                              from s in table3.ToList()
                                              join MRD in _context.QuarterlyReviewDetails.ToList() on m.MeasureId equals MRD.MeasureId
                                              where m.Status == true && MRD.QuarterId == id
                                              select new QuarterlyReviewDetails
                                              {
                                                  MeasureId = m.MeasureId,
                                                  MeasureTypeDescription = mt.MeasureTypeDescription,
                                                  measureGroupName = mg.MeasureGroupName,
                                                  Title = m.Title,
                                                  Description = m.Description,
                                                  RollUpMeasure = m.RollUpMeasure ? "Yes" : "",
                                                  Target = m.Target,
                                                  GreenRange = m.GreenRange,
                                                  RedRange = m.RedRange,
                                                  MeasureUnit=m.MeasureUnit,
                                                  SeasonName = s.SeasonName,
                                                  DataSteward = m.DataSteward,
                                                  MeasureOwner = m.MeasureOwner,
                                                  CurrentValue = Math.Round(MRD.CurrentValue,3),
                                                  Action = QMRTLookUp.GetActionName(_context, MRD.ActionId),
                                                  Trend = QMRTLookUp.GetTrend(_context, MRD.TrendId),
                                                  Status = m.Status,
                                                  listAction = QMRTLookUp.GetAllActiveActions(_context),
                                                  listTrend = QMRTLookUp.GetAllTrends(_context),
                                                  CurrentValueRange = this.GetCurrentValueRange(m.CurrentValueOptionId
                                                  , m.GreenRange, m.RedRange, MRD.CurrentValue)
                                              }
                                    );
                return View(quarterlyReviewDetails);
            }
            else
            {
                var quarterlyReviewDetails = (from m in _context.Measure.ToList()
                                              join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                              from mg in table1.ToList()
                                              join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                              from mt in table2.ToList()
                                              join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                              from s in table3.ToList()
                                              join MRD in _context.QuarterlyReviewDetails.ToList() on m.MeasureId equals MRD.MeasureId
                                              where m.Status == true && MRD.QuarterId == id && (m.DataSteward == HttpContext.Session.GetString("currentUserName") || m.MeasureOwner == HttpContext.Session.GetString("currentUserName"))
                                              select new QuarterlyReviewDetails
                                              {
                                                  MeasureId = m.MeasureId,
                                                  MeasureTypeDescription = mt.MeasureTypeDescription,
                                                  measureGroupName = mg.MeasureGroupName,
                                                  Title = m.Title,
                                                  Description = m.Description,
                                                  RollUpMeasure = m.RollUpMeasure ? "Yes" : "",
                                                  Target = m.Target,
                                                  GreenRange = m.GreenRange,
                                                  RedRange = m.RedRange,
                                                  MeasureUnit = m.MeasureUnit,
                                                  SeasonName = s.SeasonName,
                                                  DataSteward = m.DataSteward,
                                                  MeasureOwner = m.MeasureOwner,
                                                  CurrentValue = Math.Round(MRD.CurrentValue, 3),
                                                  Action = QMRTLookUp.GetActionName(_context, MRD.ActionId),
                                                  Trend = QMRTLookUp.GetTrend(_context, MRD.TrendId),
                                                  Status = m.Status,
                                                  listAction = QMRTLookUp.GetAllActions(_context),
                                                  listTrend = QMRTLookUp.GetAllTrends(_context),
                                                  CurrentValueRange = this.GetCurrentValueRange(m.CurrentValueOptionId
                                                  , m.GreenRange, m.RedRange, MRD.CurrentValue)
                                              }
                                                    );
                return View(quarterlyReviewDetails);
            }
        }

        public ActionResult QRRollUp(int id)
        {
            SetSessionInfo();
            TempData["QuarterId"] = id;
            TempData["QuarterStatus"] = QMRTLookUp.GetQuarterStatus(_context, id);
            TempData["Quarter"] = QMRTLookUp.GetQuarter(_context, id);
            TempData["MeasureCount"] = _context.QuarterlyReviewDetails.Count(QRD => QRD.QuarterId == id);
            TempData["Measurecollected"] = _context.QuarterlyReviewDetails.Count(m => (m.CurrentValue != 0 || m.ActionId != 1 || m.TrendId != 1) && m.QuarterId == id);
            var SubMeasure = (from m in _context.Measure.ToList()
                                 join rollupM in _context.RollUpMeasures.ToList() on m.MeasureId equals rollupM.MeasureId
                                 where m.Status == true && m.RollUpMeasure == true
                                 select rollupM.RollUpMeasureId);
            var RollupMeasure = (from m in _context.Measure.ToList()
                                 join rollupM in _context.RollUpMeasures.ToList() on m.MeasureId equals rollupM.MeasureId
                                 where m.Status == true && (m.RollUpMeasure == true|| m.DataSteward == "QMR OBM")
                                 select m.MeasureId);
            var AppCalculatedMeasure = (from m in _context.Measure.ToList()
                                        where m.DataSteward == "QMR OBM" && m.Status == true
                                        select m.MeasureId);
          
            var quarterlyReviewDetails = (from m in _context.Measure.Where(x => SubMeasure.Contains(x.MeasureId) || RollupMeasure.Contains(x.MeasureId)|| AppCalculatedMeasure.Contains(x.MeasureId)).ToList()
                                              join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                              from mg in table1.ToList()
                                              join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                              from mt in table2.ToList()
                                              join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                              from s in table3.ToList()
                                              join MRD in _context.QuarterlyReviewDetails.ToList() on m.MeasureId equals MRD.MeasureId
                                              where m.Status == true && MRD.QuarterId == id
                                              select new QuarterlyReviewDetails
                                              {
                                                  MeasureId = m.MeasureId,
                                                  MeasureTypeDescription = mt.MeasureTypeDescription,
                                                  measureGroupName = mg.MeasureGroupName,
                                                  Title = m.Title,
                                                  Description = m.Description,
                                                  RollUpMeasure = m.RollUpMeasure ? "Yes" : "",
                                                  Target = m.Target,
                                                  GreenRange = m.GreenRange,
                                                  RedRange = m.RedRange,
                                                  MeasureUnit = m.MeasureUnit,
                                                  SeasonName = s.SeasonName,
                                                  DataSteward = m.DataSteward,
                                                  CurrentValue = Math.Round(MRD.CurrentValue, 3),
                                                  Action = QMRTLookUp.GetActionName(_context, MRD.ActionId),
                                                  Trend = QMRTLookUp.GetTrend(_context, MRD.TrendId),
                                                  Status = m.Status,
                                                  listAction = QMRTLookUp.GetAllActiveActions(_context),
                                                  listTrend = QMRTLookUp.GetAllTrends(_context),
                                                  CurrentValueRange = this.GetCurrentValueRange(m.CurrentValueOptionId
                                                  , m.GreenRange, m.RedRange, MRD.CurrentValue)
                                              }
                                    ).OrderByDescending(x=>x.RollUpMeasure);
            @TempData["Success"] = "Roll Up set succesfully! Please update Action and Trend for RollUp measures";
            return View(quarterlyReviewDetails);
        }

        public ActionResult QuarterlyReviewRollup(int id)
        {
            SetSessionInfo();
            TempData["QuarterId"] = id;
            TempData["QuarterStatus"] = QMRTLookUp.GetQuarterStatus(_context, id);
            TempData["Quarter"] = QMRTLookUp.GetQuarter(_context, id);
           
            var quarterlyReviewDetails = (from m in _context.Measure.Where(x=> x.Reporting==true).ToList()
                                          join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                          from mg in table1.ToList()
                                          join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                          from mt in table2.ToList()
                                          join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                          from s in table3.ToList()
                                          join QRD in _context.QuarterlyReviewDetails.ToList() on m.MeasureId equals QRD.MeasureId into table4
                                          from QRD in table4.ToList()
                                          where m.Status == true && QRD.QuarterId == id 
                                          select new QuarterlyReviewDetails
                                          {
                                              MeasureId = m.MeasureId,
                                              MeasureTypeDescription = mt.MeasureTypeDescription,
                                              measureGroupName = mg.MeasureGroupName,
                                              Title = m.Title,
                                              Description = m.Description,
                                              RollUpMeasure = m.RollUpMeasure ? "Yes" : "",
                                              Target = m.Target,
                                              GreenRange = m.GreenRange,
                                              RedRange = m.RedRange,
                                              MeasureUnit = m.MeasureUnit,
                                              SeasonName = s.SeasonName,
                                              MeasureOwner = m.MeasureOwner,
                                              Status = m.Status,
                                              CurrentValue = Math.Round(QRD.CurrentValue, 3),
                                              Action = QMRTLookUp.GetActionName(_context, QRD.ActionId),
                                              Trend =QMRTLookUp.GetTrend(_context, QRD.TrendId),
                                              CurrentValueRange = this.GetCurrentValueRange(m.CurrentValueOptionId
                                                  , m.GreenRange, m.RedRange, QRD.CurrentValue)
                                          }
                                );
            return View("QuarterlyReviewRollup",quarterlyReviewDetails);
        }
        [HttpPost]
        public async Task<IActionResult> Save(int id,float CurrentValue,string selectedAction, string SelectedTrend, int quarterid)
        {
            SetSessionInfo();
            var quarterlyReviewDetails = await _context.QuarterlyReviewDetails.FirstOrDefaultAsync(e => e.QuarterId == quarterid && e.MeasureId == id);
            quarterlyReviewDetails.MeasureId = id;
            quarterlyReviewDetails.CurrentValue = CurrentValue;
            quarterlyReviewDetails.ActionId= _context.measureAction.SingleOrDefault(m => m.ActionName == selectedAction).ActionId;
            quarterlyReviewDetails.TrendId = _context.Trend.SingleOrDefault(m => m.TrendName == SelectedTrend).TrendId;
            quarterlyReviewDetails.QuarterId = quarterid;
            if (QMRTLookUp.IsReporting(_context, quarterlyReviewDetails.MeasureId))
            {
                if (quarterlyReviewDetails.DisplayOrder is null || quarterlyReviewDetails.DisplayOrder == 0)
                {
                    var maxDisplayOrder = (int)_context.QuarterlyReviewDetails.Where(e => e.QuarterId == quarterid).Max(x => x.DisplayOrder);
                    quarterlyReviewDetails.DisplayOrder = ++maxDisplayOrder;
                }
            } else
            {
                quarterlyReviewDetails.DisplayOrder = 0;
            }
            if (ModelState.IsValid)
            {
                _context.Update(quarterlyReviewDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(QuarterlyReview));
            }
            return View(quarterlyReviewDetails);
        }
        private bool QuarterExists(int id)
        {
            return _context.QuarterlyMeasure.Any(e => e.QuarterId == id);
        }
        [HttpPost]
        public async Task<IActionResult> quarterSave(int id, bool quarterStatus)
        {
            SetSessionInfo();
            int QuarterId = id;
            var selectedQuarterlyMeasure = await _context.QuarterlyMeasure.FindAsync(QuarterId);
            selectedQuarterlyMeasure.Locked = quarterStatus;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(selectedQuarterlyMeasure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuarterExists(selectedQuarterlyMeasure.QuarterId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Status"] = "QuarterStatus set succesfully!";
                return RedirectToAction(nameof(Detail));
            }
            return View(selectedQuarterlyMeasure);
        }
        public async Task<IActionResult> Rollup(int id)
        {
            SetSessionInfo();
            int QuarterId = id;
            var selectedQuarterlyMeasure = await _context.QuarterlyMeasure.FindAsync(QuarterId);

            var rollUpMeasure = await _context.Measure.Where(s => s.RollUpMeasure == true && s.Status == true).ToListAsync();
            foreach (var rollUp in rollUpMeasure)
            {
                if (rollUp.MeasureUnit == "%")
                {
                    var QuarterlyrollUpMeasure = (from m in _context.Measure.ToList()
                                                  join QRD in _context.QuarterlyReviewDetails.ToList() on m.MeasureId equals QRD.MeasureId into table1
                                                  from QRD in table1.ToList()
                                                  join rollupM in _context.RollUpMeasures.ToList() on m.MeasureId equals rollupM.RollUpMeasureId
                                                  where rollupM.MeasureId == rollUp.MeasureId && QRD.QuarterId == id
                                                  select new QuarterlyReviewDetails
                                                  {
                                                      MeasureId = rollupM.RollUpMeasureId,
                                                      Weighingfactor = rollupM.WeightingFactor,
                                                      Target = m.Target,
                                                      GreenRange = m.GreenRange,
                                                      RedRange = m.RedRange,
                                                      MeasureUnit = m.MeasureUnit,
                                                      CurrentValue = Math.Round(QRD.CurrentValue, 3),
                                                  });
                    double RollupCurrentValue = CalculateCurrentvalue(QuarterlyrollUpMeasure);
                    var UpdaterollUpMeasure = await _context.QuarterlyReviewDetails.Where(s => s.MeasureId == rollUp.MeasureId && s.QuarterId == id).FirstAsync();
                    UpdaterollUpMeasure.CurrentValue = RollupCurrentValue;
                    _context.Update(UpdaterollUpMeasure);
                    await _context.SaveChangesAsync();
                }
            }

            string quarter = await _context.QuarterlyMeasure.Where(s => s.QuarterId == id).Select(s => s.Quarter).FirstAsync();
            int seasonId = QMRTLookUp.GetSeasonId(_context, quarter);
            var UpdateOutcomeMeasure = await _context.QuarterlyReviewDetails.Where(s => s.MeasureId == 93 && s.QuarterId == id).FirstAsync();
            int GreenOutcomeMeasure = 0;
            var OutcomeMeasure = (from m in _context.Measure.ToList()
                                       join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                       from mg in table1.ToList()
                                       join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                       from mt in table2.ToList()
                                       join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                       from s in table3.ToList()
                                       where mt.MeasureTypeDescription == "Outcome" && m.Status == true && (m.SeasonId == seasonId || m.SeasonId == 5)
                                       select m.MeasureId).ToList();

            var calMeasure = new List<int>() { 92,93,94 };
            int TotalOutcomeMeasure = 0;
            TotalOutcomeMeasure = OutcomeMeasure.Except(calMeasure).Count();
            
            foreach (var processM in OutcomeMeasure.Except(calMeasure))
            {
                GreenOutcomeMeasure = GreenOutcomeMeasure + _context.QuarterlyReviewDetails.Where(s => s.ActionId == 54 && s.MeasureId == processM && s.QuarterId == id).Count();
            }

            UpdateOutcomeMeasure.CurrentValue = GreenOutcomeMeasure * 100 / TotalOutcomeMeasure;
            _context.Update(UpdateOutcomeMeasure);
            await _context.SaveChangesAsync();

            var UpdateProcessMeasure = await _context.QuarterlyReviewDetails.Where(s => s.MeasureId == 92 && s.QuarterId == id).FirstAsync();
           
            int TotalProcessMeasure = (from m in _context.Measure.Where(m=>m.Status == true).ToList()
                                  join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                  from mg in table1.ToList()
                                  join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                  from mt in table2.ToList()
                                  join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                  from s in table3.ToList()
                                  join QRD in _context.QuarterlyReviewDetails.ToList() on m.MeasureId equals QRD.MeasureId into table4
                                  from QRD in table4.ToList()
                                  where (mt.MeasureTypeDescription == "Operating Process"|| mt.MeasureTypeDescription == "Support Process") && m.Status == true && (m.SeasonId == seasonId || m.SeasonId == 5) && QRD.CurrentValue != 0 && QRD.QuarterId == id
                                  select m.MeasureId).Count();

            int GreenProcessMeasure = (from m in _context.Measure.ToList()
                                  join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                  from mg in table1.ToList()
                                  join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                  from mt in table2.ToList()
                                  join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                  from s in table3.ToList()
                                  join QRD in _context.QuarterlyReviewDetails.ToList() on m.MeasureId equals QRD.MeasureId into table4
                                  from QRD in table4.ToList()
                                  where (mt.MeasureTypeDescription == "Operating Process" || mt.MeasureTypeDescription == "Support Process") && m.Status == true && (m.SeasonId == seasonId || m.SeasonId == 5) && QRD.ActionId==54 && QRD.QuarterId==id
                                  select m.MeasureId).Count();

            UpdateProcessMeasure.CurrentValue = GreenProcessMeasure * 100 / TotalProcessMeasure;
            _context.Update(UpdateProcessMeasure);
            await _context.SaveChangesAsync();

            var UpdateImproveMeasure = await _context.QuarterlyReviewDetails.Where(s => s.MeasureId == 94 && s.QuarterId == id).FirstAsync();
            int RedMeasureCount = 0,ProcessImproveMeasure =0,NoDataMeasure=0;
            var NodataActionID= new List<int>() { 14, 15, 16,19,7 };
            var TotalRedOrYellowMeasure = (from m in _context.Measure.Where(m => m.Status == true).ToList()
                                       join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                       from mg in table1.ToList()
                                       join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                       from s in table3.ToList()
                                       join QRD in _context.QuarterlyReviewDetails.ToList() on m.MeasureId equals QRD.MeasureId into table4
                                       from QRD in table4.ToList()
                                       where m.Status == true && (m.SeasonId == seasonId || m.SeasonId == 5) && QRD.ActionId != 54 && QRD.QuarterId == id
                                       select new Measure { MeasureId=m.MeasureId, RedRange=m.RedRange, GreenRange=m.GreenRange });
           
            foreach ( var RedMeasure in TotalRedOrYellowMeasure)
            {
                if ((RedMeasure.GreenRange - RedMeasure.RedRange) > 0)
                {
                    var RedMeasureId= _context.QuarterlyReviewDetails.Where(s => s.MeasureId == RedMeasure.MeasureId && s.CurrentValue < RedMeasure.RedRange && s.QuarterId == id).ToList();
                    RedMeasureCount= RedMeasureCount+RedMeasureId.Count();
                    ProcessImproveMeasure = ProcessImproveMeasure + _context.QuarterlyReviewDetails.Where(s => s.MeasureId == RedMeasure.MeasureId && s.CurrentValue < RedMeasure.RedRange && s.QuarterId == id && s.ActionId== 17).Count();
                    NoDataMeasure= NoDataMeasure + _context.QuarterlyReviewDetails.Where(s => s.MeasureId == RedMeasure.MeasureId && s.CurrentValue < RedMeasure.RedRange && s.QuarterId == id && NodataActionID.Contains(s.ActionId)).Count();
                }
                else
                { 
                    RedMeasureCount = RedMeasureCount + _context.QuarterlyReviewDetails.Where(s => s.MeasureId == RedMeasure.MeasureId && s.CurrentValue > RedMeasure.RedRange && s.QuarterId == id).Count();
                    ProcessImproveMeasure = ProcessImproveMeasure + _context.QuarterlyReviewDetails.Where(s => s.MeasureId == RedMeasure.MeasureId && s.CurrentValue > RedMeasure.RedRange && s.QuarterId == id && s.ActionId == 17).Count();
                    NoDataMeasure = NoDataMeasure + _context.QuarterlyReviewDetails.Where(s => s.MeasureId == RedMeasure.MeasureId && s.CurrentValue > RedMeasure.RedRange && s.QuarterId == id && NodataActionID.Contains(s.ActionId)).Count();
                }

            }
            UpdateImproveMeasure.CurrentValue = ProcessImproveMeasure*100 / (RedMeasureCount- NoDataMeasure);
            _context.Update(UpdateImproveMeasure);
            await _context.SaveChangesAsync();
            return Json(new { result = "Redirect", url = Url.Action("QRRollUp", "QuarterlyMeasures", new { id = QuarterId }) });

        }
        private double CalculateCurrentvalue(IEnumerable<QuarterlyReviewDetails> quarterlyReviewDetails)
        {
            double RollupCurrentValue=0, Rollup_TScore, SumRollup_TScore=0, SumRollup_Possible_TScore=0,Rollup_Possible_TScore;
            foreach (var quarterrollUp in quarterlyReviewDetails)
            {
                if (!String.IsNullOrEmpty(quarterrollUp.Weighingfactor.ToString()) && (quarterrollUp.Target >= quarterrollUp.GreenRange))
                    {
                        Rollup_Possible_TScore = quarterrollUp.Weighingfactor * 5;
                        if (quarterrollUp.CurrentValue >= quarterrollUp.GreenRange)
                            Rollup_TScore = quarterrollUp.Weighingfactor * 5;
                        else if (quarterrollUp.CurrentValue < quarterrollUp.GreenRange && quarterrollUp.CurrentValue > quarterrollUp.RedRange)
                            Rollup_TScore = quarterrollUp.Weighingfactor * 2.5;
                        else
                            Rollup_TScore = 0;
                    }
                    else
                    {
                        Rollup_Possible_TScore = quarterrollUp.Weighingfactor * 5;
                        if (quarterrollUp.CurrentValue <= quarterrollUp.GreenRange)
                            Rollup_TScore = quarterrollUp.Weighingfactor * 5;
                        else if (quarterrollUp.CurrentValue > quarterrollUp.GreenRange && quarterrollUp.CurrentValue < quarterrollUp.RedRange)
                            Rollup_TScore = quarterrollUp.Weighingfactor * 2.5;
                        else
                            Rollup_TScore = 0;
                    }
                    SumRollup_TScore = SumRollup_TScore + Rollup_TScore;
                    SumRollup_Possible_TScore = SumRollup_Possible_TScore + Rollup_Possible_TScore;
            }
            return RollupCurrentValue= SumRollup_TScore*100/ SumRollup_Possible_TScore;
        }
        private bool QuarterlyReviewDetailsExists(int id,int quarterid)
        {
            return _context.QuarterlyReviewDetails.Any(e => e.QuarterId == quarterid && e.MeasureId == id);
        }
        public ActionResult Chart(int id, int measureId)
        {
            string QuarterDetail = QMRTLookUp.GetQuarter(_context, id);
            string quarter = QuarterDetail.Substring(0, 11);
            int year = Int32.Parse(QuarterDetail.Substring(12, 4));
            int intquarter = Int32.Parse(quarter.Substring(0, 1));
            int Quaterid;
            List<int> lstQuaterid = new List<int>();
            for (int i = 12; i > 0; i--)
            {
                switch (intquarter)
                {
                    case 1:
                        quarter = "1st quarter";
                        break;
                    case 2:
                        quarter = "2nd quarter";
                        break;
                    case 3:
                        quarter = "3rd quarter";
                        break;
                    case 4:
                        quarter = "4th quarter";
                        break;
                }
                Quaterid = (from qm in _context.QuarterlyMeasure
                               where qm.Quarter == quarter && qm.Year == year
                               select qm.QuarterId).FirstOrDefault();
                lstQuaterid.Add(Quaterid);
                if (--intquarter == 0)
                {
                    intquarter = 4;
                    year--;
                }
            }  
             var  ChartData = (from qm in _context.QuarterlyMeasure.Where(x=> lstQuaterid.Contains(x.QuarterId)).OrderBy(x => x.Year).ThenBy(x => x.Quarter).ToList()
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
          
            var maxCurrentValue = ChartData.Max(x => x.CurrentValue);
            for (int i = 0; i < ChartData.Count; i++)
            {
                if ((ChartData[i].GreenRange - ChartData[i].RedRange) > 0)
                    {
                        if ((maxCurrentValue - ChartData[i].GreenRange) > 0)
                            ChartData[i].Greenheight = string.Concat(((maxCurrentValue - ChartData[i].GreenRange)* 70 / maxCurrentValue).ToString(), "%");
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
                            ChartData[i].Yellowheight = string.Concat((((maxCurrentValue - ChartData[i].GreenRange) *70 / maxCurrentValue)).ToString(), "%");
                        else
                            ChartData[i].Yellowheight = "0%";
                        if ((maxCurrentValue - ChartData[i].RedRange) > 0)
                            ChartData[i].Redheight = string.Concat((((maxCurrentValue - ChartData[i].RedRange) * 70 / maxCurrentValue)).ToString(), "%");
                        else
                            ChartData[i].Redheight = "0%";
                    }
            }
            return Json(ChartData);
        }
        public ActionResult AllMeasureChart(int id)
        { 
            string QuarterDetail = QMRTLookUp.GetQuarter(_context, id);
            string quarter = QuarterDetail.Substring(0, 11);
            int year = Int32.Parse(QuarterDetail.Substring(12, 4));
            int intquarter = Int32.Parse(quarter.Substring(0, 1));
            int Quaterid;
            List<int> lstQuaterid = new List<int>();
            for (int i = 12; i > 0; i--)
            {
                switch (intquarter)
                {
                    case 1:
                        quarter = "1st quarter";
                        break;
                    case 2:
                        quarter = "2nd quarter";
                        break;
                    case 3:
                        quarter = "3rd quarter";
                        break;
                    case 4:
                        quarter = "4th quarter";
                        break;
                }
                Quaterid = (from qm in _context.QuarterlyMeasure
                            where qm.Quarter == quarter && qm.Year == year
                            select qm.QuarterId).FirstOrDefault();
                lstQuaterid.Add(Quaterid);
                if (--intquarter == 0)
                {
                    intquarter = 4;
                    year--;
                }
            }
            var quarterlyChart = (from m in _context.Measure.Where(x=>x.Reporting==true).ToList()
                                  join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                  from mg in table1.ToList()
                                  join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                  from mt in table2.ToList()
                                  join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                  from s in table3.ToList()
                                  join QRD in _context.QuarterlyReviewDetails.ToList() on m.MeasureId equals QRD.MeasureId into table4
                                  from QRD in table4.ToList()
                                  where QRD.QuarterId == id
                                  orderby QRD.DisplayOrder
                                  select new QuarterlyChart
                                  {
                                      MeasureId = m.MeasureId,
                                      MeasureTypeDescription = mt.MeasureTypeDescription,
                                      measureGroupName = mg.MeasureGroupName,
                                      Title = m.Title,
                                      Description = m.Description,
                                      MeasureUnit = m.MeasureUnit,
                                      GreenRange = m.GreenRange,
                                      RedRange = m.RedRange,
                                      ChartData=new List<ChartQData>(),
                                      QuarterlyReviewDetailsId = QRD.Id
                                  }
                   ).ToList();
           
            for(int i=0; i<quarterlyChart.Count();i++)
            {
                var ChartData = (from qm in _context.QuarterlyMeasure.ToList().Where(x => lstQuaterid.Contains(x.QuarterId)).OrderBy(x => x.Year).ThenBy(x => x.Quarter)
                                 join QRD in _context.QuarterlyReviewDetails.ToList() on qm.QuarterId equals QRD.QuarterId into table2
                                 from QRD in table2.ToList()
                                 join m in _context.Measure.ToList() on QRD.MeasureId equals m.MeasureId into table3
                                 from m in table3.ToList()
                                 where QRD.MeasureId == quarterlyChart[i].MeasureId
                                 select new ChartQData
                           {
                               QuarterDetail = QMRTLookUp.GetQuarter(_context, qm.QuarterId),
                               CurrentValue = Math.Round(QRD.CurrentValue, 3)
                           }
                                 ).Reverse().Take(12).Reverse().ToList();

                quarterlyChart[i].ChartData = ChartData;

                var maxCurrentValue = quarterlyChart[i].ChartData.Max(x => x.CurrentValue);
                var avgCurrentValue = quarterlyChart[i].ChartData.Where(x => x.CurrentValue!=0).Average(x => x.CurrentValue);
                quarterlyChart[i].Yheight = Math.Round(maxCurrentValue*1.1);
                if ((quarterlyChart[i].GreenRange - quarterlyChart[i].RedRange) > 0)
                {
                    if (quarterlyChart[i].MeasureUnit == "Number" || quarterlyChart[i].MeasureUnit == "Injuries" || quarterlyChart[i].MeasureUnit == "minutes")
                    {
                        if (quarterlyChart[i].GreenRange - quarterlyChart[i].RedRange <= 1)
                            quarterlyChart[i].Yheight = quarterlyChart[i].GreenRange * 1.1;
                        else 
                        if(maxCurrentValue> quarterlyChart[i].GreenRange)
                        { quarterlyChart[i].Yheight = Math.Round(maxCurrentValue * 1.1); }
                        else
                            quarterlyChart[i].Yheight = Math.Round(quarterlyChart[i].GreenRange * 1.1);
                        quarterlyChart[i].Greenheight = string.Concat(((quarterlyChart[i].Yheight - quarterlyChart[i].GreenRange) * 55 / quarterlyChart[i].Yheight).ToString(), "%");
                        quarterlyChart[i].Yellowheight = string.Concat((((quarterlyChart[i].Yheight) - quarterlyChart[i].RedRange) * 55 / quarterlyChart[i].Yheight).ToString(), "%");
                        quarterlyChart[i].Redheight = "55.4%";
                    }
                    else
                    {
                        if ((maxCurrentValue - quarterlyChart[i].GreenRange) > 0)
                            quarterlyChart[i].Greenheight = string.Concat((((quarterlyChart[i].Yheight) - quarterlyChart[i].GreenRange) * 55 / quarterlyChart[i].Yheight).ToString(), "%");
                        else
                            quarterlyChart[i].Yheight = Math.Round(quarterlyChart[i].GreenRange * 1.1);
                            quarterlyChart[i].Greenheight = string.Concat((((quarterlyChart[i].Yheight) - quarterlyChart[i].GreenRange) * 55 / quarterlyChart[i].Yheight).ToString(), "%"); ;
                            quarterlyChart[i].Yellowheight = string.Concat((((quarterlyChart[i].Yheight) - quarterlyChart[i].RedRange) * 55 / quarterlyChart[i].Yheight).ToString(), "%");
                            quarterlyChart[i].Redheight = "55.4%";
                    }
                }
                else
                {
                    if (quarterlyChart[i].MeasureUnit == "Number" || quarterlyChart[i].MeasureUnit == "Injuries" || quarterlyChart[i].MeasureUnit == "minutes" || quarterlyChart[i].MeasureUnit == "per 325,000 miles")
                    {
                        if (quarterlyChart[i].RedRange - quarterlyChart[i].GreenRange <= 1)
                            quarterlyChart[i].Yheight = quarterlyChart[i].RedRange * 1.1;
                        else
                        quarterlyChart[i].Yheight = Math.Round(quarterlyChart[i].RedRange * 1.1);
                        quarterlyChart[i].Redheight = string.Concat(((quarterlyChart[i].Yheight - quarterlyChart[i].RedRange) * 55 / quarterlyChart[i].RedRange).ToString(), "%");
                        quarterlyChart[i].Yellowheight = string.Concat((((quarterlyChart[i].Yheight) - quarterlyChart[i].GreenRange) * 55 / quarterlyChart[i].Yheight).ToString(), "%");
                        quarterlyChart[i].Greenheight = "55.4%";
                    }
                    else
                    {
                        quarterlyChart[i].Greenheight = "55.4%";
                        if ((maxCurrentValue - quarterlyChart[i].RedRange) > 0)
                            quarterlyChart[i].Yellowheight = string.Concat(((((quarterlyChart[i].Yheight) - quarterlyChart[i].GreenRange) * 55 / quarterlyChart[i].Yheight)).ToString(), "%");
                        else
                            quarterlyChart[i].Yheight = Math.Round(quarterlyChart[i].RedRange * 1.1);
                            quarterlyChart[i].Yellowheight = string.Concat(((((quarterlyChart[i].Yheight) - quarterlyChart[i].GreenRange) * 55 / quarterlyChart[i].Yheight)).ToString(), "%");               
                            quarterlyChart[i].Redheight = string.Concat(((((quarterlyChart[i].Yheight) - quarterlyChart[i].RedRange) * 55 / quarterlyChart[i].Yheight)).ToString(), "%");
                    }
                }
                if (Math.Abs(quarterlyChart[i].GreenRange - quarterlyChart[i].RedRange) <= 0.3 && quarterlyChart[i].MeasureUnit == "%")
                {
                    quarterlyChart[i].Yheight = 100;
                    quarterlyChart[i].Greenheight = string.Concat((((quarterlyChart[i].Yheight) - quarterlyChart[i].GreenRange) * 55).ToString(), "%"); 
                    quarterlyChart[i].Yellowheight = string.Concat((((quarterlyChart[i].Yheight) - quarterlyChart[i].RedRange) * 55).ToString(), "%");
                    quarterlyChart[i].Redheight = "55.4%";
                }
            }
            return Json(quarterlyChart);
        }
        public ActionResult QuarterlyChart(int id)
        {
            SetSessionInfo();
            TempData["QuarterId"] = id;
            var QuarterlyChart = (from m in _context.Measure.ToList()
                                  join mg in _context.MeasureGroups.ToList() on m.MeasureGroupId equals mg.MeasureGroupId into table1
                                  from mg in table1.ToList()
                                  join mt in _context.MeasureType.ToList() on mg.MeasureTypeId equals mt.MeasureTypeId into table2
                                  from mt in table2.ToList()
                                  join s in _context.Season.ToList() on m.SeasonId equals s.SeasonId into table3
                                  from s in table3.ToList()
                                  join QRD in _context.QuarterlyReviewDetails.ToList() on m.MeasureId equals QRD.MeasureId into table4
                                  from QRD in table4.ToList()
                                  where QRD.QuarterId == id
                                  select new QuarterlyChart
                                  {
                                      MeasureId = m.MeasureId,
                                      MeasureTypeDescription = mt.MeasureTypeDescription,
                                      measureGroupName = mg.MeasureGroupName,
                                      Title = m.Title,
                                      Description = m.Description,
                                      MeasureUnit = m.MeasureUnit,
                                      GreenRange = m.GreenRange,
                                      RedRange = m.RedRange
                                  }
                              );
            return View("QuarterlyChart", QuarterlyChart);
        }
        [HttpPost]
        public async Task<IActionResult> SaveDisplayOrder(String[] displayOrders)
        {
            SetSessionInfo();
            var order = 0;
            foreach (String id in displayOrders)
            {
                var qrd = new QuarterlyReviewDetails
                {
                    Id = int.Parse(id),
                    DisplayOrder = ++order
                };
                _context.QuarterlyReviewDetails.Attach(qrd);
                _context.Entry(qrd).Property(x => x.DisplayOrder).IsModified = true;
            }
            await _context.SaveChangesAsync();
            return View("QuarterlyChart");
        }

        private string GetCurrentValueRange(int currentValueOptionId, double greenRange, double redRange, double currentValue)
        {
            string currentValueColor = null;
            switch (currentValueOptionId)
            {
                case 1:
                    if (greenRange - redRange > 0)
                    {
                        if (currentValue >= greenRange)
                            currentValueColor = "GreenRange";
                        else if (currentValue < greenRange && currentValue >= redRange)
                            currentValueColor = "YellowRange";
                        else
                            currentValueColor = "RedRange";
                    } else
                    {
                        if (currentValue <= greenRange)
                            currentValueColor = "GreenRange";
                        else if (currentValue > greenRange && currentValue <= redRange)
                            currentValueColor = "YellowRange";
                        else
                            currentValueColor = "RedRange";
                       }
                    break;
                case 2:
                    currentValueColor = "RedRange";
                    break;
                case 3:
                    currentValueColor = "GreenRange";
                    break;
                case 4:
                    currentValueColor = "YellowRange";
                    break;
                default:
                      break;
            }
            return currentValueColor;
            ;

        }
    }
}
