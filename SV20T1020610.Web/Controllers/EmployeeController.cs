using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020610.BusinessLayers;
using SV20T1020610.DataLayers.SQLSever;
using SV20T1020610.DomainModels;
using SV20T1020610.Web.Models;

namespace SV20T1020610.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator} ,{WebUserRoles.Employee}")]
    public class EmployeeController : Controller
    {
        const int PAGE_SIZE = 20;
        const string Create_title = "Bổ sung nhân viên";
        const string Update_title = "Cập nhật thông tin nhân viên";
        // GET: /<controller>/
        const string EMPLOYEE_SEARCH = "employee_search";//Tên biến session dùng để lưu lại điều kiện tìm kiếm
        public IActionResult Index()
        {
            //Kiểm tra xem trong session có lưu điều kiện tìm kiếm không
            //Nếu có thì sử dụng điều kiện tìm kiếm , ngược lại thì tìm kiếm theo điều kiện mặt định
            Models.PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH);
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(input);
        }
        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new EmployeeSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RountCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm
            ApplicationContext.SetSessionData(EMPLOYEE_SEARCH, input);

            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.IsEdit = false;
            ViewBag.Title = Create_title;
            var model = new Employee()
            {
                EmployeeID = 0,
                Photo = "nophoto.png",
                BirthDate = new DateTime(1990, 1, 1),
                IsWorking = true
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Title = Update_title;
            ViewBag.IsEdit = true;
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrWhiteSpace(model.Photo))
                model.Photo = "nophoto.png";
            return View(model);
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            Employee model = CommonDataService.GetEmployee(id);
            if (model == null)     
                return RedirectToAction("Index");        
            return View(model);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Save(Employee model, IFormFile? uploadPhoto, string BirthDateInput = "")
        {
            if (string.IsNullOrWhiteSpace(model.FullName))
                ModelState.AddModelError(nameof(model.FullName), "Tên không được để trống");

            /*if (string.IsNullOrWhiteSpace(model.BirthDate))
                ModelState.AddModelError(nameof(model.BirthDate), "Ngày sinh không được để trống");*/
            if (string.IsNullOrWhiteSpace(model.Address))
                ModelState.AddModelError(nameof(model.Address), "Địa chỉ không được để trống");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError(nameof(model.Email), "Email không được để trống");
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.EmployeeID == 0 ? "Bổ Sung Khách Hàng" : "Cập nhật thông tin khách hàng";
                return View("Edit", model);
            }



            //Xử lý ngày sinh
            DateTime? d = BirthDateInput.ToDateTime();
            if (d.HasValue)
                model.BirthDate = d.Value;



            //Xử lý ảnh : nếu có ảnh upload thì lưu ảnh lên sever , gán tên file ảnh cho model.photo
            if (uploadPhoto != null && uploadPhoto.Length > 0)
            {
                // Lấy tên của tệp ảnh
                var originalFileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";


                // Lưu tên của ảnh vào model
                model.Photo = originalFileName;

                // Tạo đường dẫn đầy đủ cho tệp ảnh đích
                var imagePath = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath,@"images\employees", originalFileName);

                // Sao chép tệp ảnh từ tạm thời vị trí tải lên sang thư mục images/employees
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }

            }
            if (model.EmployeeID == 0)
            {
                int id = CommonDataService.AddEmployee(model);
                if (id <= 0)
                {
                    ModelState.AddModelError("Email", "Email bị trùng");
                    ViewBag.Title("Bổ sung nhân viên");
                    return View();
                }

            }
            else
            {
                //Xóa ảnh ban đầu
                var existingEmployee = CommonDataService.GetEmployee(model.EmployeeID);
                if (!string.IsNullOrEmpty(existingEmployee.Photo))
                {
                    var imagePathToDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "employees", existingEmployee.Photo);
                    if (System.IO.File.Exists(imagePathToDelete))
                    {
                        System.IO.File.Delete(imagePathToDelete);
                    }
                }
                CommonDataService.UpdateEmployee(model);
            }
            return RedirectToAction("Index");
        }

    }
}