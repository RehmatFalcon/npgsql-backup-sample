namespace NpgSqlBackup.ViewModels;

public class ProductAddVm
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public IFormFile Image { get; set; }
}