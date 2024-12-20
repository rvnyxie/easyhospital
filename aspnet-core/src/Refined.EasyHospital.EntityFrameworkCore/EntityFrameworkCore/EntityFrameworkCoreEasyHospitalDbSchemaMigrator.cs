using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Refined.EasyHospital.Data;
using Volo.Abp.DependencyInjection;

namespace Refined.EasyHospital.EntityFrameworkCore;

public class EntityFrameworkCoreEasyHospitalDbSchemaMigrator
    : IEasyHospitalDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreEasyHospitalDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the EasyHospitalDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<EasyHospitalDbContext>()
            .Database
            .MigrateAsync();
    }
}
