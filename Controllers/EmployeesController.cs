using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceWebApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace InsuranceWebApp.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Create([Bind("Name,Step_Name,S_Step_Name,phone,email,position")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
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
        public async Task<IActionResult> Edit(int id, [Bind("employee_id,Name,Step_Name,S_Step_Name,phone,email,position")] Employee model)
        {
            if (id != model.employee_id) return NotFound();
            if (ModelState.IsValid)
            {
                try
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Employees.Any(e => e.employee_id == id)) return NotFound();
                    else throw;
                }
            }
            return View(model);
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