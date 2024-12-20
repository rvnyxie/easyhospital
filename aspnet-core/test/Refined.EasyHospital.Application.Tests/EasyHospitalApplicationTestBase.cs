using Volo.Abp.Modularity;

namespace Refined.EasyHospital;

public abstract class EasyHospitalApplicationTestBase<TStartupModule> : EasyHospitalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
