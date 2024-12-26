using OfficeOpenXml;
using Refined.EasyHospital.Base;
using Refined.EasyHospital.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace Refined.EasyHospital.Districts
{
    /// <summary>
    /// App service implementation for District
    /// </summary>
    public class DistrictAppService(
        IRepository<District, Guid> districtRepository,
        IDistrictDapperRepository districtDapperRepository)
        :
        CrudAppService<District, DistrictDto, Guid, LocalityPagedAndSortedResultRequestDto, DistrictCreateDto, DistrictUpdateDto>(districtRepository),
        IDistrictAppService
    {
        public override async Task<PagedResultDto<DistrictDto>> GetListAsync(LocalityPagedAndSortedResultRequestDto input)
        {
            // Extract pagination and filter parameters
            var search = input.Search;
            var provinceCode = input.ProvinceCode;
            var pageSize = input.MaxResultCount;
            var currentPage = input.SkipCount / input.MaxResultCount + 1;

            var districts = new List<District>();
            var totalCount = 0;

            if (provinceCode == null)
            {
                (districts, totalCount) = await districtDapperRepository.GetManyAsync(search, pageSize, currentPage);
            }
            else
            {
                (districts, totalCount) = await districtDapperRepository.GetManyByProvinceCodeAsync(search, provinceCode, pageSize, currentPage);
            }

            var districtDtos = await MapToGetListOutputDtosAsync(districts);

            return new PagedResultDto<DistrictDto>(
                totalCount,
                districtDtos
            );
        }

        public override async Task<DistrictDto> GetAsync(Guid id)
        {
            var district = await districtDapperRepository.GetAsync(id);

            var districtDto = await MapToGetOutputDtoAsync(district);

            return districtDto;
        }

        public async Task<List<string>> Import(IRemoteStreamContent input)
        {
            var validationErrors = new List<string>();

            if (input == null || input.ContentLength == 0)
            {
                validationErrors.Add("Upload file is empty");
                return validationErrors;
            }

            // Validate the content type
            if (!input.ContentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase))
            {
                validationErrors.Add("Invalid file type. Please upload a valid Excel file (.xlsx).");
                return validationErrors;
            }

            try
            {
                // Process the upload file
                using (var stream = new MemoryStream())
                {
                    await input.GetStream().CopyToAsync(stream);
                    stream.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            validationErrors.Add("The uploaded excel file is empty or invalid");
                            return validationErrors;
                        }

                        // Validate headers
                        var headers = new[] { "Mã", "Tên", "Tên Tiếng Anh", "Cấp", "Mã TP", "Tỉnh / Thành Phố" };
                        for (var col = 1; col <= headers.Length; col++)
                        {
                            if (worksheet.Cells[1, col].Text.Trim() != headers[col - 1])
                            {
                                validationErrors.Add($"Column {col} must be '{headers[col - 1]}'");
                            }
                        }

                        if (validationErrors.Any())
                        {
                            return validationErrors;
                        }

                        // Read rows
                        var districts = new List<District>();
                        var codes = new HashSet<string>();
                        var names = new HashSet<string>();
                        var englishNames = new HashSet<string>();
                        var enumMappingHelper = new EnumMappingHelper();

                        for (var row = 2; row <= worksheet.Dimension.End.Row - 1; row++)
                        {
                            var code = worksheet.Cells[row, 1].Text.Trim();
                            var name = worksheet.Cells[row, 2].Text.Trim();
                            var englishName = worksheet.Cells[row, 3].Text.Trim();
                            var levelText = worksheet.Cells[row, 4].Text.Trim();
                            var provinceCode = worksheet.Cells[row, 5].Text.Trim();

                            // Validate empty rows
                            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(levelText))
                            {
                                validationErrors.Add($"Row {row} contains empty value");
                                continue;
                            }

                            // Validate level values
                            if (!enumMappingHelper.TryMapDistrictLevel(levelText, out var level))
                            {
                                validationErrors.Add($"Row {row}: Invalid level value '{levelText}'. Expected value are 'Quận' or 'Huyện' or 'Thành phố' or 'Thị xã'");
                                continue;
                            }

                            // Collect for batch checking
                            codes.Add(code);
                            names.Add(name);
                            englishNames.Add(englishName);

                            // Create new province
                            districts.Add(new District
                            {
                                Code = code,
                                Name = name,
                                EnglishName = englishName,
                                Level = level,
                                ProvinceCode = provinceCode
                            });
                        }

                        if (validationErrors.Any())
                        {
                            return validationErrors;
                        }

                        // Batch-check existing codes and names
                        var existingDistricts = await districtDapperRepository.GetByCodesOrNamesAsync(codes, names);

                        // Filter out existing records
                        districts = districts.Where(p =>
                            !existingDistricts.Any(ep => ep.Code == p.Code || ep.Name == p.Name)).ToList();

                        if (!districts.Any())
                        {
                            validationErrors.Add("All rows are already in the database.");
                            return validationErrors;
                        }

                        // No error found, insert to DB
                        await districtRepository.InsertManyAsync(districts);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error when importing excel", ex);
            }

            // Return empty if no error found
            return validationErrors;
        }
    }
}
