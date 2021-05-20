using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeEditViewModel : EmployeeAddViewModel
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
    }
}
