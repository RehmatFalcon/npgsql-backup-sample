using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NpgSqlBackup.Data;
using NpgSqlBackup.Models;
using NpgSqlBackup.ViewModels;

namespace NpgSqlBackup.Controllers;

public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly ImageDirectoryProvider _imageDirectoryProvider;

    public ProductController(AppDbContext context, ImageDirectoryProvider imageDirectoryProvider)
    {
        _context = context;
        _imageDirectoryProvider = imageDirectoryProvider;
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

        var fileName = Guid.NewGuid() + "." + Path.GetExtension(vm.Image.FileName);
        var directoryPath = Path.Combine(_imageDirectoryProvider.GetPath(), "uploads");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var filePath = Path.Combine(directoryPath, fileName);
        await using var stream = new FileStream(filePath, FileMode.Create);
        {
            await vm.Image.CopyToAsync(stream);
        }

        var product = new Product();
        product.Name = vm.Name;
        product.Description = vm.Description;
        product.CreatedDate = DateTime.UtcNow;
        product.Image = fileName;
        
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