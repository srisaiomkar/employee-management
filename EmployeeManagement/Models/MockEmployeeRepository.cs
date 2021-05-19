using EmployeeManagement.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>
            {
                new Employee(){Id = 1, Name = "Huskar", Email = "huskar@dota.com",Department = Department.HR},
                new Employee(){Id = 2, Name = "Anti Mage", Email = "am@dota.com",Department = Department.IT},
                new Employee(){Id = 3, Name = "Dazzle", Email = "dazzle@dota.com",Department = Department.Others},
            };
        }

        public int AddEmployee(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee.Id;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _employeeList;
        }

        public int RemoveEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
