using Refined.EasyHospital.Samples;
using Xunit;

namespace Refined.EasyHospital.EntityFrameworkCore.Domains;

[Collection(EasyHospitalTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<EasyHospitalEntityFrameworkCoreTestModule>
{

}
