namespace Saver.AccountIntegrationService.Contracts;

public record BankServiceProviderDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}