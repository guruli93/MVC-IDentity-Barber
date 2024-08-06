
using Dai_2022.Models;
using Application.Models_DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Dai.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;

        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductReqvestModel2 productRequest)
        {
           

            if (!ModelState.IsValid)
            {
                return View(productRequest); 
            }

            try
            {

                var result = await _productService.Add_Product(productRequest);

                return RedirectToAction("ProductX"); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the product");

                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(productRequest); 
            }
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<IActionResult> Update(ProductReqvestModel2 productReqvestModel, int id)
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;
            try
            {
                var ex = await _productService.Update_Product(productReqvestModel, id);

                return RedirectToAction(("Product"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product");

                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(productReqvestModel); // Return the view with the current model to display the error message
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //----------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;

            var products = await _productService.GetByCategory(searchString);

           
            if (products.Count() > 0)
            {
                return View(products);
            }

            var product = await _productService.Get_Product_ByName(searchString);
            var productResponse = new ProductResponseModel2
            {
                Id_Pr = product.Id_Pr,
                ProductCategory = product.ProductCategory,
                ProductName = product.ProductName,
                ProdutCount = product.ProdutCount,
                ImageData = product.ImageData,
                ContentType = product.ContentType,
            };

            return View(new[] { productResponse });

            /*
            var product = await _productService.Get_Product_ByName(searchString);
            var productResponse = new ProductResponseModel2
            {
                Id_Pr = product.Id_Pr,
                ProductCategory = product.ProductCategory,
                ProductName = product.ProductName,
                ProdutCount = product.ProdutCount,
                ImageData = product.ImageData,
                ContentType = product.ContentType,
            };

            IEnumerable<ProductResponseModel2> productResponses = new List<ProductResponseModel2> { productResponse };
            return View(productResponses);
            */

            /*
            var product = await _productService.Get_Product_ByName(searchString);
            var productResponse = new ProductResponseModel2
            {   Id_Pr=product.Id_Pr,
                ProductCategory = product.ProductCategory,
                ProductName = product.ProductName,
                ProdutCount = product.ProdutCount,
                ImageData = product.ImageData,
                ContentType = product.ContentType,
            };
            return View(new List<ProductResponseModel2> { productResponse });
            */
        }
        public async Task<IActionResult> Delete(int Id)
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;
            await _productService.Delete_Product(Id);


            return RedirectToAction("ProductX");
        }
        [HttpGet]
        public async Task<IActionResult> Product()
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;
            var productList = await _productService.GetAll_Productasync();

            var productResponseList = productList.Select(p => new ProductResponseModel2
            {
                Id_Pr = p.Id_Pr,
                ProductName = p.ProductName,
                ProdutCount = p.ProdutCount.ToString(),
                ProductCategory = p.ProductCategory,
                ContentType = p.ContentType,
                ImageData = p.ImageData
            }).ToList();

         
            return View(productResponseList);
        }

        [HttpGet]
        public async Task<IActionResult> ProductX(int page = 1)
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;
            int pageSize = 4;
            var products = await _productService.GettingDatabyPages(page, pageSize);

            var totalProducts = await _productService.AmountOfallData();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            return View(products);
        }
        public async Task<IActionResult> Edit(ProductReqvestModel2 model)
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;
            return View();
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;
            return View();
        }

        public async Task<IActionResult> DisplayImage(int id)
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;
            
            try
            {
                var product = await _productService.ShowImage(id);

                if (product == null || product.Image == null || product.Image.ImageData == null)
                {
                    return NotFound();
                }

                return File(product.Image.ImageData, product.ContentType);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                _logger.LogError(ex, "An error occurred while displaying the product image.");
                return RedirectToAction("Error", "Product");
            }
           
        }
    }
}
