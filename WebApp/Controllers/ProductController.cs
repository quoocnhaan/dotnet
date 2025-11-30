using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Controllers;
using WebApp.Data;
using WebApp.Models;
using WebApp.Service;

public class ProductController : BaseController
{
    private readonly ProductService _productService;

    public ProductController(
        IDbContextFactory<AppDBContext> context,
        ProductService productService,
        CartService cartService,
        UserService userService)
        : base(context, userService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();
        return View(products);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();

        return View(product);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Category"] = await _productService.GetCategoriesAsync();
        return View("Upsert", new Product());
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Category"] = await _productService.GetCategoriesAsync();
        var product = await _productService.GetByIdAsync(id);

        if (product == null) return NotFound();
        return View("Upsert", product);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(Product model)
    {
        if (!ModelState.IsValid) return View("Upsert", model);

        await _productService.UpsertAsync(model);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}
