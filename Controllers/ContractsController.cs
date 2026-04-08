using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebApp.Controllers
{
    public class ContractsController : Controller
    {
        private readonly InsuranceDBContext _context;
        public ContractsController(InsuranceDBContext context) => _context = context;

        // GET: Contracts
        public async Task<IActionResult> Index()
        {
            var contracts = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.Product)
                .Include(c => c.Employee)
                .ToListAsync();
            return View(contracts);
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.Product)
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(m => m.contract_id == id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            ViewBag.Clients = _context.Clients.ToList();
            ViewBag.Products = _context.InsuranceProducts.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View();
        }

        // POST: Contracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("client_id,product_id,employee_id,insurance_premium,insurance_amount,start_date,end_date,status")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = _context.Clients.ToList();
            ViewBag.Products = _context.InsuranceProducts.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();
            ViewBag.Clients = _context.Clients.ToList();
            ViewBag.Products = _context.InsuranceProducts.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View(contract);
        }

        // POST: Contracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("contract_id,client_id,product_id,employee_id,insurance_premium,insurance_amount,start_date,end_date,status")] Contract contract)
        {
            if (id != contract.contract_id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Contracts.Any(e => e.contract_id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = _context.Clients.ToList();
            ViewBag.Products = _context.InsuranceProducts.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.Product)
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(m => m.contract_id == id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract != null) _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}