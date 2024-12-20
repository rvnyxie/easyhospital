using Volo.Abp.Modularity;

namespace Refined.EasyHospital;

[DependsOn(
    typeof(EasyHospitalApplicationModule),
    typeof(EasyHospitalDomainTestModule)
)]
public class EasyHospitalApplicationTestModule : AbpModule
{

}
