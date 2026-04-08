using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebApp.Controllers
{
    public class InsuranceProductsController : Controller
    {
        private readonly InsuranceDBContext _context;
        public InsuranceProductsController(InsuranceDBContext context) => _context = context;

        public async Task<IActionResult> Index() => View(await _context.InsuranceProducts.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.InsuranceProducts.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,insurance_type,description,tariff_rate,min_term_days,max_term_days")] InsuranceProduct product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.InsuranceProducts.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("product_id,Name,insurance_type,description,tariff_rate,min_term_days,max_term_days")] InsuranceProduct product)
        {
            if (id != product.product_id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.InsuranceProducts.Any(e => e.product_id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.InsuranceProducts.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.InsuranceProducts.FindAsync(id);
            if (product != null) _context.InsuranceProducts.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}