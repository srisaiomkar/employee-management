using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name ="Role Name")]
        public string Name { get; set; }
    }
}
