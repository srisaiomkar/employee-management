using EmployeeManagement.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {  
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Sri",
                    Email = "sri@dummy.com",
                    Department = Department.HR
                },
                new Employee
                {
                    Id = 2,
                    Name = "Sai",
                    Email = "sai@dummy.com",
                    Department = Department.IT
                }
            ); ;
        }
    }
}
