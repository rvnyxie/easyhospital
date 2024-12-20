using Xunit;

namespace Refined.EasyHospital.EntityFrameworkCore;

[CollectionDefinition(EasyHospitalTestConsts.CollectionDefinitionName)]
public class EasyHospitalEntityFrameworkCoreCollection : ICollectionFixture<EasyHospitalEntityFrameworkCoreFixture>
{

}
