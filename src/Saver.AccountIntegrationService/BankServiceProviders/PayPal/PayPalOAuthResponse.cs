﻿namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal;

public record PayPalOAuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}