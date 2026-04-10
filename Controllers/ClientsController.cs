using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace InsuranceWebApp.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly InsuranceDBContext _context;

        public ClientsController(InsuranceDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var client = await _context.Clients.FirstOrDefaultAsync(m => m.client_id == id);
            if (client == null) return NotFound();
            return View(client);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Step_Name,S_Step_Name,phone,address,email")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("client_id,Name,Step_Name,S_Step_Name,phone,address,email")] Client model)
        {
            if (id != model.client_id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var client = await _context.Clients.FindAsync(id);
                    if (client == null) return NotFound();

                    client.Name = model.Name;
                    client.Step_Name = model.Step_Name;
                    client.S_Step_Name = model.S_Step_Name;
                    client.phone = model.phone;
                    client.address = model.address;
                    client.email = model.email;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Clients.Any(e => e.client_id == id)) return NotFound();
                    else throw;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var client = await _context.Clients.FirstOrDefaultAsync(m => m.client_id == id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Contracts)
                    .ThenInclude(ct => ct.Payments)
                .Include(c => c.Contracts)
                    .ThenInclude(ct => ct.Situations)
                        .ThenInclude(s => s.Payouts)
                .FirstOrDefaultAsync(c => c.client_id == id);

            if (client == null) return NotFound();

            foreach (var contract in client.Contracts)
            {
                foreach (var situation in contract.Situations)
                {
                    if (situation.Payouts != null && situation.Payouts.Any())
                        _context.Payouts.RemoveRange(situation.Payouts);
                }
                if (contract.Situations != null && contract.Situations.Any())
                    _context.InsuranceSituations.RemoveRange(contract.Situations);
                if (contract.Payments != null && contract.Payments.Any())
                    _context.Payments.RemoveRange(contract.Payments);
            }
            if (client.Contracts != null && client.Contracts.Any())
                _context.Contracts.RemoveRange(client.Contracts);

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}