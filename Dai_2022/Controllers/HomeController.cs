using Application.Models_DB;
using Dai_2022.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Application;
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

    public async Task<IActionResult> Privacy(int page = 1)
    {
        ViewBag.background_image = true;
        ViewBag.IsLoginPage = true;
        int pageSize = 7;
        var products = await _productService.GettingDatabyPages(page, pageSize);

        var totalProducts = await _productService.AmountOfallData();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

        return View(products);
    }


    /*
    public async Task<IActionResult> Privacy(int page = 1)
    {
        ViewBag.background_image = true;
        ViewBag.IsLoginPage = true;
        int pageSize = 5;
        var products = await _productService.GettingDatabyPages(page, pageSize);

        var totalProducts = await _productService.AmountOfallData();
        var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View(products);
    }
    */

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
