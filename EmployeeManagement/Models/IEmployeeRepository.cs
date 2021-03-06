using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id);
        IEnumerable<Employee> GetEmployees();
        int AddEmployee(Employee employee);
        int UpdateEmployee(Employee employee);
        int RemoveEmployee(int id);
    }
}
