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

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment)
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
            Employee employee = _employeeRepository.GetEmployee(id);
            if(employee == null)
            {
                return View("EmployeeNotFound", id);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
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
        public IActionResult Add(EmployeeAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = SaveImage(model);
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

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingImageName = employee.ImageName
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    string uniqueFileName = SaveImage(model);
                    if (model.ExistingImageName != null)
                    {
                        string filepath = Path.Combine(_webHostEnvironment.WebRootPath, "images", model.ExistingImageName);
                        System.IO.File.Delete(filepath);
                    }
                    model.ExistingImageName = uniqueFileName;
                }
                Employee employee = new Employee
                {
                    Id = model.Id,
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    ImageName = model.ExistingImageName
                };
                _employeeRepository.UpdateEmployee(employee);
                return RedirectToAction("details", new { id = model.Id });
            }
            return View();
        }

        private string SaveImage(EmployeeAddViewModel model)
        {
            string uniqueFileName = null;
            if (model.Image != null)
            {
                string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(folderPath, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }
            }

            return uniqueFileName;
        }
    }
}
