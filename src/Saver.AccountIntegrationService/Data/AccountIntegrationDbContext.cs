using Microsoft.EntityFrameworkCore;
using Saver.AccountIntegrationService.Models;

namespace Saver.AccountIntegrationService.Data;

public class AccountIntegrationDbContext(DbContextOptions<AccountIntegrationDbContext> options) : DbContext(options)
{
    public virtual DbSet<AccountIntegration> AccountIntegrations { get; set; }
}