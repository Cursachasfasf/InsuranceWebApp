using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebApp.Controllers
{
    public class InsuranceSituationsController : Controller
    {
        private readonly InsuranceDBContext _context;
        public InsuranceSituationsController(InsuranceDBContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var situations = await _context.InsuranceSituations
                .Include(s => s.Contract)
                .Include(s => s.Employee)
                .ToListAsync();
            return View(situations);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var situation = await _context.InsuranceSituations
                .Include(s => s.Contract)
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.situation_id == id);
            if (situation == null) return NotFound();
            return View(situation);
        }

        public IActionResult Create()
        {
            ViewBag.Contracts = _context.Contracts.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("contract_id,employee_id,status,damage_amount,description,incident_date,claim_date")] InsuranceSituation situation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(situation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Contracts = _context.Contracts.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View(situation);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var situation = await _context.InsuranceSituations.FindAsync(id);
            if (situation == null) return NotFound();
            ViewBag.Contracts = _context.Contracts.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View(situation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("situation_id,contract_id,employee_id,status,damage_amount,description,incident_date,claim_date")] InsuranceSituation situation)
        {
            if (id != situation.situation_id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(situation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.InsuranceSituations.Any(e => e.situation_id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Contracts = _context.Contracts.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View(situation);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var situation = await _context.InsuranceSituations.FindAsync(id);
            if (situation == null) return NotFound();
            return View(situation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var situation = await _context.InsuranceSituations.FindAsync(id);
            if (situation != null) _context.InsuranceSituations.Remove(situation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}