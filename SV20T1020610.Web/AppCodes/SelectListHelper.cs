using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1020610.BusinessLayers;
using System.Security.AccessControl;

namespace SV20T1020610.Web
{
    public static class SelectListHelper
    {
            public static List<SelectListItem> Provinces()
            {
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem()
                {
                    Value = "",
                    Text = "---Chọn tỉnh thành---"
                });
                foreach (var item in CommonDataService.ListOfProVinces())
                {

                    list.Add(new SelectListItem()
                    {
                        Value = item.ProvinceName,
                        Text = item.ProvinceName
                    });
                }
                return list;
            }

            public static List<SelectListItem> Categories()
            {
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem()
                {

                    Value = "",
                    Text = "---Tất cả---"
                });
                foreach (var item in CommonDataService.ListOfCategories())
                {

                    list.Add(new SelectListItem()
                    {
                        Value = item.CategoryID.ToString(),
                        Text = item.CategoryName
                    });
                }
                return list;
            }
            public static List<SelectListItem> SelectCategories()
            {
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem()
                {

                    Value = "",
                    Text = "---Chọn loại hàng---"
                });
                foreach (var item in CommonDataService.ListOfCategories())
                {

                    list.Add(new SelectListItem()
                    {
                        Value = item.CategoryID.ToString(),
                        Text = item.CategoryName
                    });
                }
                return list;
            }

            public static List<SelectListItem> Suppliers()
            {
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem()
                {

                    Value = "",
                    Text = "---Tất cả---"
                });
                foreach (var item in CommonDataService.ListOfSuppliers())
                {

                    list.Add(new SelectListItem()
                    {
                        Value = item.SupplierID.ToString(),
                        Text = item.SupplierName
                    });
                }
                return list;
            }
            public static List<SelectListItem> SelectSuppliers()
            {
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem()
                {

                    Value = "",
                    Text = "---Chọn nhà cung cấp---"
                });
                foreach (var item in CommonDataService.ListOfSuppliers())
                {

                    list.Add(new SelectListItem()
                    {
                        Value = item.SupplierID.ToString(),
                        Text = item.SupplierName
                    });
                }
                return list;
            }
        }
    }