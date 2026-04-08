using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Models;
using System.Threading.Tasks;
using System.Linq;

namespace InsuranceWebApp.Controllers
{
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
        public async Task<IActionResult> Create(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, Client model)
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
            // Загружаем клиента и все связанные сущности через Include
            var client = await _context.Clients
                .Include(c => c.Contracts)
                    .ThenInclude(ct => ct.Payments)
                .Include(c => c.Contracts)
                    .ThenInclude(ct => ct.Situations)
                        .ThenInclude(s => s.Payouts)
                .FirstOrDefaultAsync(c => c.client_id == id);

            if (client == null) return NotFound();

            // Удаляем все выплаты (Payout) через страховые ситуации
            foreach (var contract in client.Contracts)
            {
                foreach (var situation in contract.Situations)
                {
                    if (situation.Payouts != null && situation.Payouts.Any())
                        _context.Payouts.RemoveRange(situation.Payouts);
                }
                // Удаляем страховые ситуации
                if (contract.Situations != null && contract.Situations.Any())
                    _context.InsuranceSituations.RemoveRange(contract.Situations);
                // Удаляем платежи
                if (contract.Payments != null && contract.Payments.Any())
                    _context.Payments.RemoveRange(contract.Payments);
            }
            // Удаляем договоры
            if (client.Contracts != null && client.Contracts.Any())
                _context.Contracts.RemoveRange(client.Contracts);

            // Наконец, удаляем клиента
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}