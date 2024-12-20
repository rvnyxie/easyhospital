using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Refined.EasyHospital.Data;

/* This is used if database provider does't define
 * IEasyHospitalDbSchemaMigrator implementation.
 */
public class NullEasyHospitalDbSchemaMigrator : IEasyHospitalDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
