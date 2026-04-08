using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebApp.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly InsuranceDBContext _context;
        public PaymentsController(InsuranceDBContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var payments = await _context.Payments
                .Include(p => p.Contract)
                .ToListAsync();
            return View(payments);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var payment = await _context.Payments
                .Include(p => p.Contract)
                .FirstOrDefaultAsync(m => m.payment_id == id);
            if (payment == null) return NotFound();
            return View(payment);
        }

        public IActionResult Create()
        {
            ViewBag.Contracts = _context.Contracts.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("contract_id,payment_date,amount,payment_type,status")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Contracts = _context.Contracts.ToList();
            return View(payment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();
            ViewBag.Contracts = _context.Contracts.ToList();
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("payment_id,contract_id,payment_date,amount,payment_type,status")] Payment payment)
        {
            if (id != payment.payment_id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Payments.Any(e => e.payment_id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Contracts = _context.Contracts.ToList();
            return View(payment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();
            return View(payment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null) _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}