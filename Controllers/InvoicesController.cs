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
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult sorting(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var invoices = from s in _context.Invoice
                           select s;
            switch (sortOrder)
            {
                case "name_desc":
                    invoices = _context.Invoice.OrderByDescending(s => s.SupplierName);
                    break;
                case "Date":
                    invoices = _context.Invoice.OrderBy(s => s.DateOrdered);
                    break;
                case "date_desc":
                    invoices = _context.Invoice.OrderByDescending(s => s.DateOrdered);
                    break;
                default:
                    invoices = _context.Invoice.OrderBy(s => s.SupplierName);
                    break;
            }
            return View(_context.Invoice.ToList());
        }

        // GET: Invoices/ ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View("ShowSearchForm");
        }

        // POST: Invoices/ ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            return View("Index", await _context.Invoice.Where(r => r.InvoiceNumber.Contains(SearchPhrase)).ToListAsync());
        }

        public async Task<IActionResult> Index()
        {
            List<Invoice> lstinv = await _context.Invoice.ToListAsync();

            foreach (Invoice inv in lstinv)
            {
                List<LineItem> lstLineItem = await _context.LineItem.Where(x => x.InvoiceId == inv.Id).ToListAsync();
                double grandTotal = 0;
                foreach (LineItem item in lstLineItem)
                {
                    double subTotal = item.Quantity * item.UnitPrice;
                    grandTotal += subTotal;
                }
                inv.Total = grandTotal;
            }
            return View(lstinv);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SupplierName,DateOrdered,DateReceived,Total,InvoiceNumber")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SupplierName,DateOrdered,DateReceived,Total,InvoiceNumber")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.Id == id);
        }
    }
}
