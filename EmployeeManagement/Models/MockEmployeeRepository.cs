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
                new Employee(){Id = 1, Name = "Huskar", Email = "huskar@dota.com",Department = "Carry"},
                new Employee(){Id = 2, Name = "Anti Mage", Email = "am@dota.com",Department = "Carry"},
                new Employee(){Id = 3, Name = "Dazzle", Email = "dazzle@dota.com",Department = "Support"},
            };
        }
        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }
    }
}
