using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NpgSqlBackup.Data;
using NpgSqlBackup.Models;
using NpgSqlBackup.ViewModels;

namespace NpgSqlBackup.Controllers;

public class ProductController : Controller
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.ToListAsync();

        return View(products);
    }

    public IActionResult Add()
    {
        var vm = new ProductAddVm();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ProductAddVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var product = new Product();
        product.Name = vm.Name;
        product.Description = vm.Description;
        product.CreatedDate = DateTime.UtcNow;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(long id)
    {
        var vm = new ProductEditVm();
        var product = _context.Products.Find(id);
        vm.Id = id;
        vm.Name = product.Name;
        vm.Description = product.Description;
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductEditVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var product = await _context.Products.FindAsync(vm.Id);
        product.Name = vm.Name;
        product.Description = vm.Description;

        _context.Products.Update(product);

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}