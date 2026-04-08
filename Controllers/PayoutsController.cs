using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebApp.Controllers
{
    public class PayoutsController : Controller
    {
        private readonly InsuranceDBContext _context;
        public PayoutsController(InsuranceDBContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var payouts = await _context.Payouts
                .Include(p => p.Situation)
                .ToListAsync();
            return View(payouts);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var payout = await _context.Payouts
                .Include(p => p.Situation)
                .FirstOrDefaultAsync(m => m.payout_id == id);
            if (payout == null) return NotFound();
            return View(payout);
        }

        public IActionResult Create()
        {
            ViewBag.Situations = _context.InsuranceSituations.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("situation_id,payout_date,payout_amount,payment_method")] Payout payout)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Situations = _context.InsuranceSituations.ToList();
            return View(payout);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var payout = await _context.Payouts.FindAsync(id);
            if (payout == null) return NotFound();
            ViewBag.Situations = _context.InsuranceSituations.ToList();
            return View(payout);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("payout_id,situation_id,payout_date,payout_amount,payment_method")] Payout payout)
        {
            if (id != payout.payout_id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Payouts.Any(e => e.payout_id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Situations = _context.InsuranceSituations.ToList();
            return View(payout);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var payout = await _context.Payouts.FindAsync(id);
            if (payout == null) return NotFound();
            return View(payout);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payout = await _context.Payouts.FindAsync(id);
            if (payout != null) _context.Payouts.Remove(payout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}