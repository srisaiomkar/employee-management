using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public IWebHostEnvironment _webHostEnvironment { get; }

        public HomeController(IEmployeeRepository employeeRepository,IWebHostEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            _webHostEnvironment = hostingEnvironment;
        }
        public ViewResult Index()
        {
            IEnumerable<Employee> employees = _employeeRepository.GetEmployees();
            return View(employees);
        }
        public ViewResult Details(int id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id),
                PageTitle = "Employee Details"
            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(HomeAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Image != null)
                {
                    string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(folderPath, uniqueFileName);
                    model.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Employee employee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    ImageName = uniqueFileName
                };
                int newEmployeeId = _employeeRepository.AddEmployee(employee);
                return RedirectToAction("details", new { id = newEmployeeId });
            }
            return View();
        }
    }
}
