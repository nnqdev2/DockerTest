using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QMR.Data;
using QMR.Models;
using Microsoft.EntityFrameworkCore;

namespace QMR.Components
{
    public class DetailsViewComponent : ViewComponent
    {
        private readonly QMRContext _context;
        public DetailsViewComponent(QMRContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var measureCount = _context.Measure.Count(m => m.Status == true);
            var quarterlyMeasure = (from m in _context.QuarterlyMeasure.OrderByDescending(x => x.Year).ThenByDescending(x => x.Quarter).ToList()

                                    select new QuarterlyMeasure
                                    {
                                        QuarterId = m.QuarterId,
                                        Quarter = m.Quarter,
                                        Year = m.Year,
                                        Locked=m.Locked,
                                        MeasureCollected = _context.QuarterlyReviewDetails.Count(QRD => (QRD.CurrentValue != 0 || QRD.ActionId != 1 ||  QRD.TrendId != 1) && QRD.QuarterId == m.QuarterId),
                                        MeasureCounted = _context.QuarterlyReviewDetails.Count(QRD => QRD.QuarterId == m.QuarterId),
                                    }
                                   );
            return await Task.FromResult((IViewComponentResult)View("Details", quarterlyMeasure)); 
        }
        private bool QuarterExists(int id)
        {
            return _context.QuarterlyMeasure.Any(e => e.QuarterId == id);
        }
        [HttpPost]
        public async Task<IActionResult> quarterSave(int id, bool quarterStatus)
        {
            int QuarterId = id;
            var QuarterlyMeasure = await _context.QuarterlyMeasure.FindAsync(QuarterId);
            QuarterlyMeasure.Locked = quarterStatus;

            if (ModelState.IsValid)
            {
                _context.Update(QuarterlyMeasure);
                await _context.SaveChangesAsync();
            }
            return await Task.FromResult((IActionResult)View("Details", QuarterlyMeasure));

        }
    }
}
