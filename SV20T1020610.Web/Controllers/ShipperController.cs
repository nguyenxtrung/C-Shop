﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020610.BusinessLayers;
using SV20T1020610.DataLayers.SQLSever;
using SV20T1020610.DomainModels;
using SV20T1020610.Web.Models;

namespace SV20T1020610.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator} ,{WebUserRoles.Employee}")]
    public class ShipperController : Controller
    {
        // GET: /<controller>/
        const int PAGE_SIZE = 20; 
        const string Create_title = "Bổ sung người giao hàng";
        const string Update_title = "Cập nhật thông tin người giao hàng";
        const string SHIPPER_SEARCH = "shipper_search";//Tên biến session dùng để lưu lại điều kiện tìm kiếm
        public IActionResult Index()
        {

            //Kiểm tra xem trong session có lưu điều kiện tìm kiếm không
            //Nếu có thì sử dụng điều kiện tìm kiếm , ngược lại thì tìm kiếm theo điều kiện mặt định
            Models.PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SHIPPER_SEARCH);
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
            var data = CommonDataService.ListOfShippers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new ShipperSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RountCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm
            ApplicationContext.SetSessionData(SHIPPER_SEARCH, input);

            return View(model);
        }
        public IActionResult Create()
        {
            var model = new Shipper()
            {
                ShipperID = 0
            };
            ViewBag.Title = Create_title;
            return View("Edit", model);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật thông tin người giao hàng";
            var model = CommonDataService.GetShipper(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetShipper(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Xóa người giao hàng";
            return View(model);
        }
        [HttpPost]
        public IActionResult Save(Shipper model)
        {
            if (string.IsNullOrWhiteSpace(model.ShipperName))
                ModelState.AddModelError(nameof(model.ShipperName), "Tên người giao hàng không được để trống!!!");
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError(nameof(model.Phone), "Số điện thoại không được để trống!!!");


            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ShipperID == 0 ? Create_title : Update_title;
                return View("Edit", model);
            }
            if (model.ShipperID == 0)
            {
                int id = CommonDataService.AddShiper(model);
                if (id <= 0)
                {
                    ModelState.AddModelError("Error", "Số điện thoại đã tồn tại!!!");
                    ViewBag.Title = Create_title;
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateShipper(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được người giao hàng (Số điện thoại có thể đã tồn tại)");
                    ViewBag.Title = Update_title;
                    return View("Edit", model);
                }
            }
            return RedirectToAction("Index");
        }
    }
}