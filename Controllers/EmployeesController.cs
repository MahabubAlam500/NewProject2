using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Paggination.Data;
using Paggination.Models;

namespace Paggination.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly dbContext _context;

        public EmployeesController(dbContext context)
        {
            _context = context;
        }

        //public async<IActionResult> Index2(int pagenumber, int pagesize)
        //{

        //    int excludeRecords = (pagesize * pagenumber) - pagesize;
        //    var employee = _context.Employee.Include(m => m.EmployeeName)
        //        .Skip(excludeRecords)
        //        .Take(pagesize);
        //    return View();

        //}

        private async<IActionResult> View(Func<int, int, async<IActionResult>> index2)
        {
            throw new NotImplementedException();
        }




        // GET: Employees
        public async Task<IActionResult> Index(string utext, string sortname, int Page)

        {
            IQueryable<Employee> emp = _context.Employee;
            ViewBag.sortbyname = string.IsNullOrEmpty(sortname) ? "Desc" : " ";



            ViewBag.stext = utext;

           

           
            
           
            if (!string.IsNullOrEmpty(utext))
            {
                utext = utext.Trim().ToLower();
                emp = emp.Where(e=>e.EmployeeName.ToLower().Contains(utext)||
                e.EmployeeStatus.ToLower().Contains(utext) ||
                e.PositionTitle.ToLower().Contains(utext)

                    ); 
                
            }
            switch (sortname)
            {
                case "Desc":
                    emp = emp.OrderByDescending(e => e.EmployeeName);
                    break;
                default:
                    emp = emp.OrderBy(e => e.EmployeeName);
                    break;

            }





            ViewBag.Count = emp.Count();

            if (Page <= 0) Page = 1;
            int PageSize = 10;
            
            //return View(emp);
            return View(await PaginatedList<Employee>.CreateAsync(emp, Page, PageSize));
            
        }
        
        
        
        



        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeName,EmployeeStatus,Salary,PayBasis,PositionTitle")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeName,EmployeeStatus,Salary,PayBasis,PositionTitle")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
