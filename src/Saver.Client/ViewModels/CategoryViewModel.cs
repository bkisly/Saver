using System.ComponentModel.DataAnnotations;

namespace Saver.Client.ViewModels;

public class CategoryViewModel
{
    [Required, MaxLength(256)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}