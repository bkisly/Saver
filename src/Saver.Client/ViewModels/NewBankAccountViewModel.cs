using System.ComponentModel.DataAnnotations;

namespace Saver.Client.ViewModels;

public class NewBankAccountViewModel
{
    [Required, MaxLength(256)] public string Name { get; set; } = string.Empty;
    [Required] public decimal InitialBalance { get; set; }
    [Required] public string CurrencyCode { get; set; } = string.Empty;
}