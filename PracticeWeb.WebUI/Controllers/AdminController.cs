using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web;
using System.Threading.Tasks;
using PracticeWeb.WebUI.Models;
using Microsoft.AspNet.Identity;
using Practice.Domain.Abstract;
using Practice.Domain.Entities;
using Practice.Domain.Concrete;
using PracticeWeb.WebUI.Infrastructure;
using System;
using Azure.Storage.Blobs;
using System.IO;
using Azure.Storage.Blobs.Models;

namespace PracticeWeb.WebUI.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        private IProductRepository repository;
        private IUserManager myUserManager;

        public AdminController(IProductRepository repo, IUserManager userManager)
        {
            repository = repo;
            myUserManager = userManager;
        }

        public PartialViewResult Menu(string selectedCategory)
        {
            AdminMenuViewModel[] dataList = new AdminMenuViewModel[] {
                new AdminMenuViewModel { ControllerName = "Admin" , ActionName = "UserIndex", DisplayName  = "會員清單"},
                new AdminMenuViewModel { ControllerName = "Admin" , ActionName = "ProductIndex", DisplayName  = "產品清單"},
                new AdminMenuViewModel { ControllerName = "Admin" , ActionName = "ProductCreate", DisplayName  = "產品新增"},
                new AdminMenuViewModel { ControllerName = "Admin" , ActionName = "OrderIndex", DisplayName  = "訂單總覽"},
                new AdminMenuViewModel { ControllerName = "RoleAdmin" , ActionName = "Index", DisplayName  = "權限設定"},
                new AdminMenuViewModel { ControllerName = "Admin" , ActionName = "ProductCateAndBrandIndex", DisplayName  = "產品廠商與類別"}
            };
            if (selectedCategory == null || !dataList.Select(a => a.ActionName).Contains(selectedCategory))
                selectedCategory = "UserIndex";
            ViewBag.SelectedCategory = selectedCategory;
            return PartialView("~/Views/Admin/_Menu.cshtml", dataList);
        }

        public ActionResult UserIndex()
        {
            return View(UserManager.Users);
        }

        public ActionResult UserCreate()
        {
            int defaultCityId = repository.Cities.OrderBy(c => c.ID).First().ID;
            CityAndCountryPorvider.SetSelectListToViewBag(this, repository, defaultCityId);
            ViewBag.IsAdminAccess = true;
            if (!CityAndCountryPorvider.CheckIfSelectListOfViewBagCorrect(this))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View("~/Views/Home/CreateUser.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserCreate(CreateAccountModel model)
        {
            if (ModelState.IsValid)
            {
                //page.310
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    CityID = model.CityID,
                    CountryID = model.CountryID,
                    ShippingAddress = model.ShippingAddress
                };
                IdentityResult result = await myUserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserIndex");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            CityAndCountryPorvider.SetSelectListToViewBag(this, repository, model.CityID, model.CountryID);
            ViewBag.IsAdminAccess = true;
            if (!CityAndCountryPorvider.CheckIfSelectListOfViewBagCorrect(this))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View("~/Views/Home/CreateUser.cshtml", model);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UserDelete(string id)
        {
            AppUser user = await myUserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserIndex");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "User Not Found" });
            }
        }

        public async Task<ActionResult> UserView(string id)
        {
            AppUser user = await myUserManager.FindByIdAsync(id);
            if (user == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            AdminUserInfoViewModel userData = new AdminUserInfoViewModel
            {
                ID = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                CityName = user.City.Name,
                CountryName = user.Country.Name,
                AddressLine = user.ShippingAddress
            };
            IEnumerable<ProductOrder> oriData = repository.ProductOrders
                .Where(p => p.AppUserID == user.Id)
                .OrderByDescending(p => p.Date);
            userData.OrderList = new List<UserOrderViewModel>(
                oriData.Select(p => new UserOrderViewModel
                {
                    Date = p.Date,
                    ProductList = p.GetDetailFromProductList(),
                    ShippingAddress = p.ShippingAddress,
                    ReceiverName = p.ReceiverName

                }));
            return View(userData);
        }

        public ActionResult OrderIndex()
        {
            return View(repository.ProductOrders);
        }

        public ActionResult OrderDetail(int id)
        {
            ProductOrder oriData = repository.ProductOrders
                .Where(p => p.ID == id).FirstOrDefault();
            if (oriData == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserOrderViewModel data = new UserOrderViewModel
            {
                Date = oriData.Date,
                ProductList = oriData.GetDetailFromProductList(),
                ShippingAddress = oriData.ShippingAddress,
                ReceiverName = oriData.ReceiverName
            };
            return View(data);
        }

        public ActionResult ProductCateAndBrandIndex()
        {
            AdminCateAndBrandViewModel data = new AdminCateAndBrandViewModel
            {
                CategoryList = repository.Categories.Select(c => new AdminCateAndBrandModel
                {
                    ID = c.ID.ToString(),
                    Name = c.Name,
                    Quantity = c.Products.Count.ToString()
                }).ToList(),
                BrandList = repository.Brands.Select(b => new AdminCateAndBrandModel
                {
                    ID = b.ID.ToString(),
                    Name = b.Name,
                    Quantity = b.Products.Count().ToString()
                }).ToList()
            };
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductAddCategory(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("name", "請輸入類別名稱");
                TempData["message"] = "請輸入類別名稱！";
            }
            if (ModelState.IsValid)
            {
                if (repository.AddCategory(name))
                {
                    TempData["message"] = string.Format("類別 {0} 已新增", name);
                }
                else
                {
                    TempData["message"] = "已有相同名稱之產品類別！";
                }
            }
            return RedirectToAction("ProductCateAndBrandIndex");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductAddBrand(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("name", "請輸入廠商名稱");
                TempData["message"] = "請輸入廠商名稱！";
            }
            if (ModelState.IsValid)
            {
                if (repository.AddBrand(name))
                {
                    TempData["message"] = string.Format("廠商 {0} 已新增", name);
                }
                else
                {
                    TempData["message"] = "已有相同名稱之產品廠商！";
                }
            }
            return RedirectToAction("ProductCateAndBrandIndex");
        }

        public ViewResult ProductIndex()
        {
            return View(repository.Products);
        }

        public ActionResult ProductEdit(int ID)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ID == ID);
            if (product == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.IsEditProcess = true;
            BrandAndCategoryProvider.SetSelectListToViewBag(this, repository, product.CategoryID, product.BrandID);
            if (!BrandAndCategoryProvider.CheckIfSelectListOfViewBagCorrect(this))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View(product);
        }

        private string GetUrlOfDefaultImageInBlob()
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                string containerName = "techstoreimage";
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                return containerClient.Uri.AbsoluteUri + "/default.jpg";
            }
            catch
            { }
            return "default.jpg";
        }

        private async Task<string> UploadImageAsync(HttpPostedFileBase fileToUpload)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                string containerName = "techstoreimage";
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                BlobUploadOptions blobUploadOptions = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = "image/jpeg"
                    }
                };
                await blobClient.UploadAsync(fileToUpload.InputStream, blobUploadOptions);
                return blobClient.Uri.AbsoluteUri;
            }
            catch
            { }
            return "default.jpg";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductEdit(Product product, HttpPostedFileBase fileToUpload = null)
        {
            try
            {
                if (product == null ||
                    (product.ID != 0 && !repository.Products.Select(p => p.ID).Contains(product.ID)))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                if (product.CoverImagePath == "default")
                    product.CoverImagePath = GetUrlOfDefaultImageInBlob();
                if (ModelState.IsValid)
                {
                    if (fileToUpload != null)
                    {
                        string fileUrl = await UploadImageAsync(fileToUpload);
                        product.CoverImagePath = fileUrl;
                    }

                    if (product.ID == 0)
                        TempData["message"] = string.Format("{0} 已新增", product.Name);
                    else
                        TempData["message"] = string.Format("{0} 的修改已儲存", product.Name);

                    repository.SaveProduct(product);
                    return RedirectToAction("ProductIndex");
                }
            }
            catch (DataException /* dex */)
            {
                ModelState.AddModelError("", "無法儲存修改值！");
            }
            ViewBag.IsEditProcess = true;
            BrandAndCategoryProvider.SetSelectListToViewBag(this, repository, product.CategoryID, product.BrandID);
            if (!BrandAndCategoryProvider.CheckIfSelectListOfViewBagCorrect(this))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View(product);
        }

        public ActionResult ProductCreate()
        {
            Product product = new Product();
            ViewBag.IsEditProcess = false;
            BrandAndCategoryProvider.SetSelectListToViewBag(this, repository);
            if (!BrandAndCategoryProvider.CheckIfSelectListOfViewBagCorrect(this))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View("ProductEdit", product);
        }

        [HttpPost]
        public ActionResult ProductDelete(int productID)
        {
            Product deletedProduct = repository.DeleteProduct(productID);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedProduct.Name);
            }
            return RedirectToAction("ProductIndex");
        }

        private AppUserManager UserManager
        {
            get { return myUserManager.UserManager; }
        }
    }
}