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

        // Список сотрудников
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        // Детали
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.employee_id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // Создать (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Создать (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Редактировать (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // Редактировать (POST)
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
            employee.hire_date = model.hire_date;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Удалить (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.employee_id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // Удалить (POST)
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