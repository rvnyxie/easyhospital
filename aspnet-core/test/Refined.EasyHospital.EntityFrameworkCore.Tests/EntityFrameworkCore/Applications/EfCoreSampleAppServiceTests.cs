using Refined.EasyHospital.Samples;
using Xunit;

namespace Refined.EasyHospital.EntityFrameworkCore.Applications;

[Collection(EasyHospitalTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<EasyHospitalEntityFrameworkCoreTestModule>
{

}
