using Moq;
using Refined.EasyHospital.Base;
using Refined.EasyHospital.Provinces;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Validation;
using Xunit;

namespace Refined.EasyHospital.Samples
{
    /// <summary>
    /// Tests for Province app service
    /// </summary>
    public class ProvinceAppServiceTests
    {
        private readonly Mock<IRepository<Province, Guid>> _provinceRepositoryMock;
        private readonly Mock<IProvinceDapperRepository> _provinceDapperRepositoryMock;
        private readonly Mock<IObjectMapper> _objectMapperMock;
        private readonly TestableProvinceAppService _provinceAppService;

        public ProvinceAppServiceTests()
        {
            _provinceRepositoryMock = new Mock<IRepository<Province, Guid>>();
            _provinceDapperRepositoryMock = new Mock<IProvinceDapperRepository>();
            _objectMapperMock = new Mock<IObjectMapper>();

            _provinceAppService = new TestableProvinceAppService(
                _provinceRepositoryMock.Object,
                _provinceDapperRepositoryMock.Object,
                _objectMapperMock.Object
            );
        }

        /// <summary>
        /// Test for getting list async and is successful
        /// </summary>
        [Fact]
        public async void GetListAsync_ValidInput_Success()
        {
            // Arrange
            var input = new LocalityPagedAndSortedResultRequestDto
            {
                Search = "",
                MaxResultCount = 10,
                SkipCount = 0
            };

            var mockProvinces = new List<Province>
            {
                new Province { Name = "Province 1" },
                new Province { Name = "Province 2" }
            };
            var mockTotalCount = 2;

            _provinceDapperRepositoryMock
                .Setup(repo => repo.GetManyAsync("", 10, 1))
                .ReturnsAsync((mockProvinces, mockTotalCount));

            var mockProvinceDtos = new List<ProvinceDto>
            {
                new ProvinceDto { Id = Guid.NewGuid(), Name = mockProvinces[0].Name },
                new ProvinceDto { Id = Guid.NewGuid(), Name = mockProvinces[1].Name }
            };

            _objectMapperMock
                .Setup(mapper => mapper.Map<List<Province>, List<ProvinceDto>>(mockProvinces))
                .Returns(mockProvinceDtos);

            // Act
            var result = await _provinceAppService.GetListAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.TotalCount.ShouldBe(mockTotalCount);
            result.Items.Count.ShouldBe(mockProvinces.Count);
            result.Items.ShouldContain(dto => dto.Name == "Province 1");
            result.Items.ShouldContain(dto => dto.Name == "Province 2");

            // Verify interactions
            _provinceDapperRepositoryMock.Verify(
                repo => repo.GetManyAsync("", 10, 1),
                Times.Once()
            );
            _objectMapperMock.Verify(
                mapper => mapper.Map<List<Province>, List<ProvinceDto>>(mockProvinces),
                Times.Once()
            );
        }

        /// <summary>
        /// Test for getting async and has returned data
        /// </summary>
        [Fact]
        public async void GetAsync_ValidIdWhichHasData_Return()
        {
            // Arrange
            var mockId = Guid.NewGuid();

            var mockProvince = new Province
            {
                Name = "Province 1"
            };

            var mockProvinceDto = new ProvinceDto
            {
                Id = mockId,
                Name = mockProvince.Name
            };

            _provinceDapperRepositoryMock
                .Setup(repo => repo.GetAsync(mockId))
                .ReturnsAsync(mockProvince);

            _objectMapperMock
                .Setup(mapper => mapper.Map<Province, ProvinceDto>(mockProvince))
                .Returns(mockProvinceDto);

            // Act
            var result = await _provinceAppService.GetAsync(mockId);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(mockId);
            result.Name.ShouldBe(mockProvince.Name);

            _provinceDapperRepositoryMock.Verify(
                repo => repo.GetAsync(mockId),
                Times.Once()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<Province, ProvinceDto>(mockProvince),
                Times.Once()
            );
        }

        /// <summary>
        /// Test for getting async when having no data
        /// </summary>
        [Fact]
        public async void GetAsync_ValidIdWhichNotHasData_ReturnNull()
        {
            // Arrange
            var mockId = Guid.NewGuid();

            _provinceDapperRepositoryMock
                .Setup(repo => repo.GetAsync(mockId))
                .ReturnsAsync((Province?)null);

            // Act
            var result = await _provinceAppService.GetAsync(mockId);

            // Assert
            result.ShouldBeNull();

            _provinceDapperRepositoryMock.Verify(
                repo => repo.GetAsync(mockId),
                Times.Once()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<Province, ProvinceDto>(It.IsAny<Province>()),
                Times.Once()
            );
        }

        /// <summary>
        /// Test for creating async with valid input
        /// </summary>
        [Fact]
        public async void CreateAsync_ValidInput_Success()
        {
            // Arrange
            var mockProvinceCreateDto = new ProvinceCreateDto
            {
                Name = "Province 1",
            };

            var mockProvince = new Province
            {
                Name = mockProvinceCreateDto.Name
            };

            var mockProvinceDto = new ProvinceDto
            {
                Id = Guid.NewGuid(),
                Name = mockProvinceCreateDto.Name,
            };

            _objectMapperMock
                .Setup(mapper => mapper.Map<ProvinceCreateDto, Province>(mockProvinceCreateDto))
                .Returns(mockProvince);

            _objectMapperMock
                .Setup(mapper => mapper.Map<Province, ProvinceDto>(mockProvince))
                .Returns(mockProvinceDto);

            _provinceRepositoryMock
                .Setup(repo => repo.InsertAsync(mockProvince, true, default))
                .ReturnsAsync(mockProvince);

            // Act
            var result = await _provinceAppService.CreateAsync(mockProvinceCreateDto);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe(mockProvinceCreateDto.Name);

            _provinceRepositoryMock.Verify(
                repo => repo.InsertAsync(mockProvince, true, default),
                Times.Once()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<ProvinceCreateDto, Province>(mockProvinceCreateDto),
                Times.Once()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<Province, ProvinceDto>(mockProvince),
                Times.Once()
            );
        }

        /// <summary>
        /// Test for Create async method with invalid create input
        /// </summary>
        [Fact]
        public async void CreateAsync_InvalidInput_ThrowException()
        {
            // Arrange
            var mockProvinceCreateDto = new ProvinceCreateDto
            {
                Name = "Province 1",
            };

            var mockProvince = new Province
            {
                Name = mockProvinceCreateDto.Name
            };

            var mockProvinceDto = new ProvinceDto
            {
                Id = Guid.NewGuid(),
                Name = mockProvinceCreateDto.Name,
            };

            _objectMapperMock
                .Setup(mapper => mapper.Map<ProvinceCreateDto, Province>(mockProvinceCreateDto))
                .Returns(mockProvince);

            _provinceRepositoryMock
                .Setup(repo => repo.InsertAsync(mockProvince, true, default))
                .ThrowsAsync(new AbpValidationException());

            // Act & Assert
            var actualException = await Should.ThrowAsync<AbpValidationException>(() => _provinceAppService.CreateAsync(mockProvinceCreateDto));

            _provinceRepositoryMock.Verify(
                repo => repo.InsertAsync(mockProvince, true, default),
                Times.Once()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<ProvinceCreateDto, Province>(mockProvinceCreateDto),
                Times.Once()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<Province, ProvinceDto>(mockProvince),
                Times.Never()
            );
        }

        /// <summary>
        /// Test for Update async method with valid input
        /// </summary>
        [Fact]
        public async void UpdateAsync_ValidInput_Success()
        {
            // Arrange
            var mockProvinceUpdateDto = new ProvinceUpdateDto
            {
                Id = Guid.NewGuid(),
                Name = "Updated province name"
            };

            var mockProvince = new Province
            {
                Name = mockProvinceUpdateDto.Name
            };

            var mockProvinceDto = new ProvinceDto
            {
                Id = mockProvinceUpdateDto.Id,
                Name = mockProvinceUpdateDto.Name
            };

            _provinceRepositoryMock
                .Setup(repo => repo.GetAsync(mockProvinceUpdateDto.Id, true, default))
                .ReturnsAsync(mockProvince);

            _provinceRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Province>(), true, default))
                .ReturnsAsync(mockProvince);

            _objectMapperMock
                .Setup(mapper => mapper.Map<ProvinceUpdateDto, Province>(mockProvinceUpdateDto, mockProvince))
                .Returns(mockProvince);

            _objectMapperMock
                .Setup(mapper => mapper.Map<Province, ProvinceDto>(mockProvince))
                .Returns(mockProvinceDto);

            // Act
            var result = await _provinceAppService.UpdateAsync(mockProvinceUpdateDto.Id, mockProvinceUpdateDto);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(mockProvinceUpdateDto.Id);
            result.Name.ShouldBe(mockProvinceUpdateDto.Name);

            _provinceRepositoryMock.Verify(
                repo => repo.UpdateAsync(It.IsAny<Province>(), true, default),
                Times.Once()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<ProvinceUpdateDto, Province>(mockProvinceUpdateDto, mockProvince),
                Times.Once()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<Province, ProvinceDto>(mockProvince),
                Times.Once()
            );
        }

        /// <summary>
        /// Test for Update async method with invalid ID
        /// </summary>
        [Fact]
        public async void UpdateAsync_InvalidId_ThrowException()
        {
            // Arrange
            var mockProvinceUpdateDto = new ProvinceUpdateDto
            {
                Id = Guid.NewGuid(),
                Name = "Updated province name"
            };

            var mockProvince = new Province
            {
                Name = mockProvinceUpdateDto.Name
            };

            var mockProvinceDto = new ProvinceDto
            {
                Id = mockProvinceUpdateDto.Id,
                Name = mockProvinceUpdateDto.Name
            };

            _provinceRepositoryMock
                .Setup(repo => repo.GetAsync(mockProvinceUpdateDto.Id, true, default))
                .ThrowsAsync(new EntityNotFoundException());

            // Act & Assert
            var actualException = await Should.ThrowAsync<EntityNotFoundException>(() => _provinceAppService.UpdateAsync(mockProvinceUpdateDto.Id, mockProvinceUpdateDto));

            _provinceRepositoryMock.Verify(
                repo => repo.UpdateAsync(It.IsAny<Province>(), true, default),
                Times.Never()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<ProvinceUpdateDto, Province>(mockProvinceUpdateDto, mockProvince),
                Times.Never()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<Province, ProvinceDto>(mockProvince),
                Times.Never()
            );
        }

        /// <summary>
        /// Test for Update async with invalid body properties
        /// </summary>
        [Fact]
        public async void UpdateAsync_InvalidBodyProperties_ThrowException()
        {
            // Arrange
            var mockProvinceUpdateDto = new ProvinceUpdateDto
            {
                Id = Guid.NewGuid(),
                Code = "Invalid code",
                Name = "Updated province name"
            };

            var mockProvince = new Province
            {
                Name = mockProvinceUpdateDto.Name
            };

            var mockProvinceDto = new ProvinceDto
            {
                Id = mockProvinceUpdateDto.Id,
                Name = mockProvinceUpdateDto.Name
            };

            _objectMapperMock
                .Setup(mapper => mapper.Map<ProvinceUpdateDto, Province>(mockProvinceUpdateDto))
                .Returns(mockProvince);

            _provinceRepositoryMock
                .Setup(repo => repo.GetAsync(mockProvinceUpdateDto.Id, true, default))
                .ReturnsAsync(mockProvince);

            _provinceRepositoryMock
                .Setup(repo => repo.UpdateAsync(mockProvince, It.IsAny<bool>(), default))
                .ThrowsAsync(new AbpValidationException());

            // Act & Assert
            var actualException = await Should.ThrowAsync<AbpValidationException>(() => _provinceAppService.UpdateAsync(mockProvinceUpdateDto.Id, mockProvinceUpdateDto));

            _provinceRepositoryMock.Verify(
                repo => repo.UpdateAsync(It.IsAny<Province>(), true, default),
                Times.Once()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<ProvinceUpdateDto, Province>(mockProvinceUpdateDto, mockProvince),
                Times.Once()
            );

            _objectMapperMock.Verify(
                mapper => mapper.Map<Province, ProvinceDto>(mockProvince),
                Times.Never()
            );
        }

        /// <summary>
        /// Test for Delete async method with valid ID
        /// </summary>
        [Fact]
        public async void DeleteAsync_ValidId_Success()
        {
            // Arrange
            var mockProvinceId = Guid.NewGuid();

            var mockProvince = new Province()
            {
                Name = "Province 1"
            };

            _provinceRepositoryMock
                .Setup(repo => repo.DeleteAsync(mockProvinceId, false, default))
                .Returns(Task.CompletedTask);

            // Act
            await _provinceAppService.DeleteAsync(mockProvinceId);

            _provinceRepositoryMock.Verify(
                repo => repo.DeleteAsync(mockProvinceId, false, default),
                Times.Once()
            );
        }
    }
}
