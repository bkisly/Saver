namespace Saver.AccountIntegrationService.Contracts;

public record BankServiceDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}