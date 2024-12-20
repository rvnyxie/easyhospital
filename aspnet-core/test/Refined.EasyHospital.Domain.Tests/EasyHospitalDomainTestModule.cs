using Volo.Abp.Modularity;

namespace Refined.EasyHospital;

[DependsOn(
    typeof(EasyHospitalDomainModule),
    typeof(EasyHospitalTestBaseModule)
)]
public class EasyHospitalDomainTestModule : AbpModule
{

}
