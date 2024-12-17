using System.ComponentModel.DataAnnotations;

namespace Saver.Client.ViewModels;

public class EditBankAccountViewModel
{
    [Required, MaxLength(256)] public string Name { get; set; } = string.Empty;
    [Required] public string CurrencyCode { get; set; } = string.Empty;
}