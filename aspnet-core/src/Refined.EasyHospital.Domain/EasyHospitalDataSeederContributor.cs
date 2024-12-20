using Refined.EasyHospital.Provinces;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Refined.EasyHospital
{
    /// <summary>
    /// Main data seeder
    /// </summary>
    public class EasyHospitalDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Province, Guid> _provinceRepository;

        public EasyHospitalDataSeederContributor(IRepository<Province, Guid> provinceRepository)
        {
            _provinceRepository = provinceRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // Provinces
            if (await _provinceRepository.GetCountAsync() <= 0)
            {
                await _provinceRepository.InsertAsync(
                    new Province
                    {
                        Code = "01",
                        Name = "Province 1",
                        Level = ProvinceLevel.Province
                    },
                    autoSave: true
                );
                await _provinceRepository.InsertAsync(
                    new Province
                    {
                        Code = "02",
                        Name = "Province 2",
                        Level = ProvinceLevel.Province
                    },
                    autoSave: true
                );
            }
        }
    }
}
