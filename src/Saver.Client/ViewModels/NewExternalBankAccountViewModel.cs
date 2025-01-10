using System.ComponentModel.DataAnnotations;

namespace Saver.Client.ViewModels;

public class NewExternalBankAccountViewModel
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public int ProviderId { get; set; } = 1;
}