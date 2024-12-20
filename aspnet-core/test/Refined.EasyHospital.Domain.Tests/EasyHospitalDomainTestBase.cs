using Volo.Abp.Modularity;

namespace Refined.EasyHospital;

/* Inherit from this class for your domain layer tests. */
public abstract class EasyHospitalDomainTestBase<TStartupModule> : EasyHospitalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
