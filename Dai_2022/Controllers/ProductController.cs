using Application;
using Dai_2022.Models;
using Dai_2022.Views.Product;
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
                return View(productRequest); // Return the view with the current model to display validation errors
            }

            try
            {

                var result = await _productService.Add_Product(productRequest);

                // If you want to display a success message and redirect to another action, use TempData
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Product"); // Redirect to the list of products
            }
            catch (Exception ex)
            {
                // Log the exception (assuming you have a logging mechanism in place)
                _logger.LogError(ex, "An error occurred while creating the product");

                // Handle any exceptions appropriately
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(productRequest); // Return the view with the current model to display the error message
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

                // If update is successful, redirect to list of products
                return RedirectToAction(("Product"));
            }
            catch (Exception ex)
            {
                // Log the exception (assuming you have a logging mechanism in place)
                _logger.LogError(ex, "An error occurred while updating the product");

                // Handle any exceptions appropriately
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
            var products = await _productService.GetAll_Productasync();
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString));
            }
            return View(products.ToList());
        }
        public async Task<IActionResult> Delete(int Id)
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;
            await _productService.Delete_Product(Id);

            return RedirectToAction("Product");
            // return View();
        }
        [HttpGet]
        public async Task<IActionResult> Product()
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;

            var product007List = await _productService.GetAll_Productasync();
            return View(product007List);
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
