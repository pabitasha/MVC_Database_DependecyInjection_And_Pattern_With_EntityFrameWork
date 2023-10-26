using ASPNETCore_DB.Interfaces;
using ASPNETCore_DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCore_DB.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudent _studentRepo;
        public StudentController(IStudent studentRepo)
        {
            _studentRepo = studentRepo;
        }
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            pageNumber = pageNumber ?? 1;
            int pageSize = 3;

            ViewData["CurrentSort"] = sortOrder;
            ViewData["StudentNumberSortParm"] = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else 
            { 
                searchString = currentFilter; 
            }

            ViewData["CurrentFilter"] = searchString;

            return View(PaginatedList<Student>.Create(_studentRepo.GetStudents(searchString, sortOrder).AsNoTracking(),pageNumber ?? 1, pageSize));
        }
        public IActionResult Details(string id)
        {
            return View(_studentRepo.Details(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("StudentNumber, FirstName, Surname, EnrollmentDate")] Student student)
 {
            try
            {
                if (ModelState.IsValid)
                {
                    _studentRepo.Create(student);
                }
            }
            catch { }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            return View(_studentRepo.Details(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("StudentNumber, FirstName, Surname, EnrollmentDate")] Student student)
 {
            if (ModelState.IsValid)
            {
                _studentRepo.Edit(student);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            return View(_studentRepo.Details(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([Bind("StudentNumber, FirstName, Surname, EnrollmentDate")] Student student)
        {
            _studentRepo.Delete(student);
            return RedirectToAction(nameof(Index));
        }


    }
}
