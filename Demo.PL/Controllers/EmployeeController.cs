using Demo.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Demo.DAL.Models;
using AutoMapper;
using Demo.PL.ViewModels;
using System.Collections;
using System.Collections.Generic;
using Demo.BLL.Repositories;
using Demo.PL.Helper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            _departmentRepository=departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string searchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(searchValue))
            {
                employees =await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees= _unitOfWork.EmployeeRepository.GetEmployeesByName(searchValue); 
            } 
            //ViewData=>KeyValuePair
            //Transfer Data from Action To its View
            //.Net framework 3
            ViewData["Message"] = "Hello from ViewData";
            //2)ViewBag=>Dinamic Properity (based on Dinamic Keyword)
            //Transfer Data from Action To its View
            //.Net framework 4
            ViewBag.Message = "Hello from ViewBag";
           
            var mappedEmploye = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmploye);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments=await _departmentRepository.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) {
                employeeVM.ImageName = DocumentSettings.Upload(employeeVM.Image, "Images");
                var mappedEmp=_mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                 await _unitOfWork.EmployeeRepository.AddAsync(mappedEmp);
                 await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }
        public async Task<IActionResult> Details(int?id, string viewName = "Details")
        {
            if (id == null) return BadRequest();
            var employee =await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if(employee == null) return NotFound();
            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(viewName,mappedEmp);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Departments =await _departmentRepository.GetAllAsync();
            //if (id == null) return BadRequest();
            //var employee = _employeeRepository.GetById(id.Value);
            //if (employee == null) return NotFound();
            //return View(employee);
            return await Details(id,"Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute]int id)
        {
            if (id != employeeVM.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if(employeeVM.Image is not null)
                    {
                        employeeVM.ImageName = DocumentSettings.Upload(employeeVM.Image, "Images");
                    }
                  
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(mappedEmp);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(employeeVM);
        }
        public async Task<IActionResult> Delete(int? id )
        {
            //if (id == null) return BadRequest();
            //var employee = _employeeRepository.GetById(id.Value);
            //if (employee == null) return NotFound();
            //return View(employee);
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                    var Res= await _unitOfWork.CompleteAsync();
                    if (Res > 0&&employeeVM.ImageName is not null)
                    {
                        DocumentSettings.Delete(employeeVM.ImageName, "Images");
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(employeeVM);
        }
    }
}
