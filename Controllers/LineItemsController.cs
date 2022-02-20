using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Data;
using InvoiceApp.Models;

namespace InvoiceApp.Controllers
{
    public class LineItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LineItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LineItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.LineItem.ToListAsync());
        }

        // GET: LineItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineItem = await _context.LineItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lineItem == null)
            {
                return NotFound();
            }

            return View(lineItem);
        }

        // GET: LineItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LineItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvoiceId,Description,Quantity,UnitPrice,LineTotal")] LineItem lineItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lineItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lineItem);
        }

        // GET: LineItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineItem = await _context.LineItem.FindAsync(id);
            if (lineItem == null)
            {
                return NotFound();
            }
            return View(lineItem);
        }

        // POST: LineItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceId,Description,Quantity,UnitPrice,LineTotal")] LineItem lineItem)
        {
            if (id != lineItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lineItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineItemExists(lineItem.Id))
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
            return View(lineItem);
        }

        // GET: LineItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineItem = await _context.LineItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lineItem == null)
            {
                return NotFound();
            }

            return View(lineItem);
        }

        // POST: LineItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lineItem = await _context.LineItem.FindAsync(id);
            _context.LineItem.Remove(lineItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LineItemExists(int id)
        {
            return _context.LineItem.Any(e => e.Id == id);
        }

        public async Task<JsonResult> SaveLineItems(LineItem lineItem)
        {
            bool ok = false;
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                if (lineItem.Id == 0)
                {
                    try
                    {
                        _context.Add(lineItem);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    try
                    {
                        _context.Update(lineItem);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                ok = true;
                msg = "Line items has been saved.";
            }
            return Json(new { ok = ok, msg = msg, newurl = "" });
        }

        public async Task<JsonResult> DeleteLineItems(int Id)
        {
            bool ok = false;
            string msg = string.Empty;
            try
            {
                var lineItem = await _context.LineItem.FindAsync(Id);
                _context.LineItem.Remove(lineItem);
                await _context.SaveChangesAsync();
                ok = true;
                msg = "Line items has been deleted";
            }
            catch (Exception ex)
            {

            }
            return Json(new { ok = ok, msg = msg, newurl = "" });
        }

        public async Task<JsonResult> GetLineItems(LineItem lineItem)
        {
            List<LineItem> LineItems = await _context.LineItem.Where(x => (x.InvoiceId == lineItem.InvoiceId || lineItem.InvoiceId == 0) && (x.Id == lineItem.Id || lineItem.Id == 0)).ToListAsync();
            return Json(new { ok = true, msg = "", LineItems = LineItems });
        }
    }
}
