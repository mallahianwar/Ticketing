using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PWF.DataLayer.Models.Auth;
using PWF.DataLayer.Models.Product;

namespace PWF.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AuthModel _authModel;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        public ProductsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
            _httpContextAccessor = httpContextAccessor;
            _session = httpContextAccessor.HttpContext.Session;
            var m = _session.GetString("AuthModel");
            _authModel = (m == null ? default(AuthModel) : JsonConvert.DeserializeObject<AuthModel>(_session.GetString("AuthModel")));
        }


        // GET: Admin/ProductCategories
        public async Task<IActionResult> Index()
        {
            List<Product> productCategory = new List<Product>();
            string endpoint = apiBaseUrl + "/Products/GetAll/";
            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        productCategory = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Try again");
                        return View();
                    }

                }
            }

            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productCategory = new Product();
            string endpoint = apiBaseUrl + "/Products/GetByID/" + id;
            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        productCategory = JsonConvert.DeserializeObject<Product>(apiResponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Try again");
                        return View();
                    }

                }
            }

            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Description1,Description2,Price,RegPrice,SpecialMessage,Image,CategoryId")]  Product productCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string endpoint = apiBaseUrl + "/Products/AddCategory";
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(productCategory), Encoding.UTF8, "application/json");

                        using (var response = await httpClient.PostAsync(endpoint, content))
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Failed !");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                //_context.Add(productCategory);
                //await _context.SaveChangesAsync();
            }
            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = new Product();
            string endpoint = apiBaseUrl + "/Products/GetByID/" + id;
            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        productCategory = JsonConvert.DeserializeObject<Product>(apiResponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Try again");
                        return View();
                    }

                }
            }


            if (productCategory == null)
            {
                return NotFound();
            }
            return View(productCategory);
        }

        // POST: Admin/ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Description1,Description2,Price,RegPrice,SpecialMessage,Image,CategoryId")] Product productCategory)
        {
            if (id != productCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        string endpoint = apiBaseUrl + "/Products/EditCategory";
                        using (var httpClient = new HttpClient())
                        {
                            StringContent content = new StringContent(JsonConvert.SerializeObject(productCategory), Encoding.UTF8, "application/json");

                            using (var response = await httpClient.PutAsync(endpoint, content))
                            {
                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    string apiResponse = await response.Content.ReadAsStringAsync();
                                    return RedirectToAction(nameof(Index));
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Failed !");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (ProductCategoryExists(productCategory.Id).Result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = new Product();
            string endpoint = apiBaseUrl + "/Products/GetByID/" + id;
            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        productCategory = JsonConvert.DeserializeObject<Product>(apiResponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Try again");
                    }

                }
            }
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // POST: Admin/ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string endpoint = apiBaseUrl + "/Products/DeleteCategory/" + id;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                        return View(id);
                    }
                }
            }
        }

        private async Task<bool> ProductCategoryExists(int id)
        {

            var productCategory = new Product();
            string endpoint = apiBaseUrl + "/Products/GetByID/" + id;
            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        productCategory = JsonConvert.DeserializeObject<Product>(apiResponse);
                        return productCategory != null;
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Try again");
                    }

                }
            }
            return false;

        }
    }
}
