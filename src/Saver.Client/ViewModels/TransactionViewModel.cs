using System.ComponentModel.DataAnnotations;

namespace Saver.Client.ViewModels;

public class TransactionViewModel
{
    [Required, MaxLength(256)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required] public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    [Required] public decimal Value { get; set; }
    public Guid? CategoryId { get; set; }
}