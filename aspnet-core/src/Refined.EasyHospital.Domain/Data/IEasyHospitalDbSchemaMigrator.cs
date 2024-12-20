using System.Threading.Tasks;

namespace Refined.EasyHospital.Data;

public interface IEasyHospitalDbSchemaMigrator
{
    Task MigrateAsync();
}
