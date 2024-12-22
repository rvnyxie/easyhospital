using Refined.EasyHospital.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Dapper repository implementation for Province
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public class ProvinceDapperRepository(IDbContextProvider<EasyHospitalDbContext> dbContextProvider) :
        DapperRepository<EasyHospitalDbContext>(dbContextProvider),
        ITransientDependency,
        IProvinceDapperRepository
    {
        public async Task<List<Province>> GetManyAsync()
        {
            await Task.Delay(100);

            var provinces = new List<Province> { new Province(), new Province() };

            return provinces;
        }

        public async Task<Province> GetAsync(Guid id)
        {
            await Task.Delay(100);

            var province = new Province();

            return province;
        }
    }
}
