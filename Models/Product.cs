﻿namespace NpgSqlBackup.Models;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public string? Image { get; set; }
}