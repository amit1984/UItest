using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using contoso.Models;
using contoso.DAL;

namespace contoso.Controllers
{
    public class studentController : Controller
    {
        //
        // GET: /student/
     //   private SchoolContext db = new SchoolContext();

      private IStudentRepository studentRepository;

      public studentController()
      {
         this.studentRepository = new StudentRepository(new SchoolContext());
      }

      //public studentController(IStudentRepository studentRepository)
      //{
      //   this.studentRepository = studentRepository;
      //}


        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            //if (searchString != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            ViewBag.CurrentFilter = searchString;

    //        var students = from s in db.Students
     //                      select s;
            var students = from s in studentRepository.GetStudents()
                        select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            return View(students.ToList());
        }
        //public ViewResult Details(int id)
        //{
        //    Student student = StudentRepository.GetStudentByID(id);
        //    return View(student);
        //}
        public ActionResult Create()
        {
            return View();
        }


   //     [HttpPost]
   //     [ValidateAntiForgeryToken]
   //     public ActionResult Create(
   //        [Bind(Include = "LastName, FirstMidName, EnrollmentDate")]
   //Student student)
   //     {
   //         try
   //         {
   //             if (ModelState.IsValid)
   //             {
   //                 studentRepository.UpdateStudent(student);
   //                 studentRepository.Save();
   //                 return RedirectToAction("Index");
   //             }
   //         }
   //         catch (System.Data.DataException /* dex */)
   //         {
   //             //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
   //             ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
   //         }
   //         return View(student);
   //     }
    }
}
