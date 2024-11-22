namespace Saver.IdentityService.Services;

/// <summary>
/// Aggregate class for often-used services.
/// </summary>
public class IdentityServices(
    IUserContextProvider userContextProvider, 
    IAccountService accountService,
    ILoginService loginService)
{
    public IUserContextProvider UserContextProvider { get; } = userContextProvider;
    public IAccountService AccountService { get; } = accountService;
    public ILoginService LoginService { get; } = loginService;
}