using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public int AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee.Id;
        }

        public Employee GetEmployee(int Id)
        {
            return _context.Employees.Find(Id);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _context.Employees;
        }

        public int RemoveEmployee(int id)
        {
            Employee employeeToRemove = _context.Employees.Find(id);
            while (employeeToRemove != null)
            {
                _context.Employees.Remove(employeeToRemove);
                _context.SaveChanges();
            }
            return employeeToRemove.Id;
        }

        public int UpdateEmployee(Employee employee)
        {
            var employeeToUpdate = _context.Employees.Attach(employee);
            employeeToUpdate.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return employee.Id;

        }
    }
}
