using Application;
using Dai_2022.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;



public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    public HomeController(ILogger<HomeController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
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

        return View();
    }

   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
