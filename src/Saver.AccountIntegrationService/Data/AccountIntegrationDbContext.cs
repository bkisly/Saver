using Microsoft.EntityFrameworkCore;

namespace Saver.AccountIntegrationService.Data;

public class AccountIntegrationDbContext(DbContextOptions<AccountIntegrationDbContext> options) : DbContext(options)
{

}