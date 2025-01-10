using Saver.EventBus;
using Saver.IdentityService.Data;

namespace Saver.IdentityService.Services;

/// <summary>
/// Aggregate class for often-used services.
/// </summary>
public class IdentityServices(
    IUserContextProvider userContextProvider, 
    IAccountService accountService,
    ILoginService loginService,
    IIntegrationEventService<ApplicationDbContext> eventService)
{
    public IUserContextProvider UserContextProvider { get; } = userContextProvider;
    public IAccountService AccountService { get; } = accountService;
    public ILoginService LoginService { get; } = loginService;
    public IIntegrationEventService<ApplicationDbContext> EventService { get; } = eventService;
}