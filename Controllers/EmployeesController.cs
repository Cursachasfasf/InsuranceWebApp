using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Models;
using System.Threading.Tasks;

namespace InsuranceWebApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly InsuranceDBContext _context;

        public EmployeesController(InsuranceDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.employee_id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee model)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            employee.Name = model.Name;
            employee.Step_Name = model.Step_Name;
            employee.S_Step_Name = model.S_Step_Name;
            employee.phone = model.phone;
            employee.email = model.email;
            employee.position = model.position;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.employee_id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null) _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}