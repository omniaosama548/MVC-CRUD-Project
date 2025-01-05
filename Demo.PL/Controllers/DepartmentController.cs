using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IDepartmentRepository departmentRepo,
            IUnitOfWork unitOfWork)
        {
           // _departmentRepo = departmentRepo;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments =await _unitOfWork.DepartmentRepository.GetAllAsync();
            return  View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) { //server side validation
               await _unitOfWork.DepartmentRepository.AddAsync(department);
               await _unitOfWork.CompleteAsync();
                //TempData=>Dictionary object
                //Transfer Data From Action To Action
               
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        public async Task<IActionResult> Details(int? id,string viewName="Details")
        {
            if(id == null) return BadRequest();
            var department=await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if(department == null) return NotFound();
            return View(viewName,department);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null) return BadRequest();
            //var department = _departmentRepo.GetById(id.Value);
            //if (department == null) return NotFound();
            // return View(department);
            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Department department, [FromRoute]int id)
        {
            if(id !=department.Id) return BadRequest();
            if (ModelState.IsValid) {
                try
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message); 
                }
               
            }
            return View(department);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var department =await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department == null) return NotFound();
            return View(department);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Department department) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Delete(department);
                   await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(department);
        }
    }
}
