using Refined.EasyHospital.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Refined.EasyHospital.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EasyHospitalEntityFrameworkCoreModule),
    typeof(EasyHospitalApplicationContractsModule)
    )]
public class EasyHospitalDbMigratorModule : AbpModule
{
}
