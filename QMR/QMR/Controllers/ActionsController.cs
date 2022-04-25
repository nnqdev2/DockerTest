using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QMR.Models;
using QMR.Data;

namespace QMR.Controllers
{
    public class ActionsController : BaseController
    {
        private readonly QMRContext _context;

        public ActionsController(QMRContext context)
        {
            _context = context;
        }

        // GET: Actions
        public async Task<IActionResult> Index()
        {
            SetSessionInfo();
            return View(await _context.measureAction.OrderBy(s => s.ActionName).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            SetSessionInfo();
            Models.MeasureAction sendAction = new Models.MeasureAction();
            sendAction.ActionColor = "#19AB16";
            return View(sendAction);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActionName,ActionColor,ActionStatus")] Models.MeasureAction measureAction)
        {
            SetSessionInfo();
            if (ModelState.IsValid)
            {
                if (!ActionNameExists(measureAction.ActionName))
                {
                    _context.Add(measureAction);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Action saved successfully!";
                    return View(measureAction);
                }
            }
            TempData["Success"] = "Action exist!";
            return View(measureAction);
        }

        // GET: Actions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            SetSessionInfo();
            if (id == null)
            {
                return NotFound();
            }

            var measureAction = await _context.measureAction.FindAsync(id);
            if (measureAction == null)
            {
                return NotFound();
            }
            return View(measureAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActionId,ActionName,ActionColor,ActionStatus")] Models.MeasureAction measureAction)
        {
            SetSessionInfo();
            if (id != measureAction.ActionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    try
                    {
                        _context.Update(measureAction);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ActionExists(measureAction.ActionId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                TempData["Success"] = "Action saved successfully!";
            }
            return View(measureAction);
        }
        private bool ActionExists(int id)
        {
            return _context.measureAction.Any(e => e.ActionId == id);
        }
        private bool ActionNameExists(string actionName)
        {
            return _context.measureAction.Any(e => e.ActionName == actionName);
        }
        [HttpPost]
        public async Task<IActionResult> Save(int id, bool ActionStatus)
        {
            SetSessionInfo();
            var measureAction = await _context.measureAction.FindAsync(id);
            measureAction.ActionStatus = ActionStatus;

            if (ModelState.IsValid)
            {
                    try
                    {
                        _context.Update(measureAction);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ActionExists(measureAction.ActionId))
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
            return View(measureAction);
        }
    }
}
