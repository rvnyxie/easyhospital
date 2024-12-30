using Refined.EasyHospital.Provinces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Refined.EasyHospital.Samples
{
    public class TestableProvinceAppService : ProvinceAppService
    {
        private readonly IObjectMapper _mockObjectMapper;

        public TestableProvinceAppService(
            IRepository<Province, Guid> provinceRepository,
            IProvinceDapperRepository provinceDapperRepository,
            IObjectMapper mockObjectMapper)
            : base(provinceRepository, provinceDapperRepository)
        {
            _mockObjectMapper = mockObjectMapper;
        }

        // Override the method that uses ObjectMapper to use the mocked behavior
        protected override Task<List<ProvinceDto>> MapToGetListOutputDtosAsync(List<Province> entities)
        {
            return Task.FromResult(_mockObjectMapper.Map<List<Province>, List<ProvinceDto>>(entities));
        }

        protected override Task<ProvinceDto> MapToGetOutputDtoAsync(Province entity)
        {
            return Task.FromResult(_mockObjectMapper.Map<Province, ProvinceDto>(entity));
        }

        protected override Task<Province> MapToEntityAsync(ProvinceCreateDto createInput)
        {
            return Task.FromResult(_mockObjectMapper.Map<ProvinceCreateDto, Province>(createInput));
        }

        protected override Task MapToEntityAsync(ProvinceUpdateDto updateInput, Province entity)
        {
            return Task.FromResult(_mockObjectMapper.Map<ProvinceUpdateDto, Province>(updateInput, entity));
        }
    }
}
