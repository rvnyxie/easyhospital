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

namespace Refined.EasyHospital.Provinces
{
    /// <summary>
    /// Service implementation for Province
    /// </summary>
    /// <param name="provinceRepository">Province repository</param>
    public class ProvinceAppService(
        IRepository<Province, Guid> provinceRepository,
        IProvinceDapperRepository provinceDapperRepository)
        :
        CrudAppService<Province, ProvinceDto, Guid, LocalityPagedAndSortedResultRequestDto, ProvinceCreateDto, ProvinceUpdateDto>(provinceRepository),
        IProvinceAppService
    {

        public override async Task<PagedResultDto<ProvinceDto>> GetListAsync(LocalityPagedAndSortedResultRequestDto input)
        {
            // Extract pagination and filter parameters
            var search = input.Search;
            var pageSize = input.MaxResultCount;
            var currentPage = input.SkipCount / input.MaxResultCount + 1;

            var provinces = await provinceDapperRepository.GetManyAsync(search, pageSize, currentPage);

            var totalCount = provinces.Count();

            var provinceDtos = await MapToGetListOutputDtosAsync(provinces);

            return new PagedResultDto<ProvinceDto>(
                totalCount,
                provinceDtos
            );
        }

        public async override Task<ProvinceDto> GetAsync(Guid id)
        {
            var province = await provinceDapperRepository.GetAsync(id);

            var provinceDto = await MapToGetOutputDtoAsync(province);

            return provinceDto;
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
                        var headers = new[] { "Mã", "Tên", "Tên Tiếng Anh", "Cấp" };
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
                        var provinces = new List<Province>();
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

                            // Validate empty rows
                            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(levelText))
                            {
                                validationErrors.Add($"Row {row} contains empty value");
                                continue;
                            }

                            // Validate level values
                            if (!enumMappingHelper.TryMapProvinceLevel(levelText, out var level))
                            {
                                validationErrors.Add($"Row {row}: Invalid level value '{levelText}'. Expected value are 'Tỉnh' or 'Thành phố Trung ương'");
                                continue;
                            }

                            // Collect for batch checking
                            codes.Add(code);
                            names.Add(name);
                            englishNames.Add(englishName);

                            // Create new province
                            provinces.Add(new Province
                            {
                                Code = code,
                                Name = name,
                                EnglishName = englishName,
                                Level = level
                            });
                        }

                        if (validationErrors.Any())
                        {
                            return validationErrors;
                        }

                        // Batch-check existing codes and names
                        var existingProvinces = await provinceDapperRepository.GetByCodesOrNamesAsync(codes, names);

                        // Filter out existing records
                        provinces = provinces.Where(p =>
                            !existingProvinces.Any(ep => ep.Code == p.Code || ep.Name == p.Name)).ToList();

                        if (!provinces.Any())
                        {
                            validationErrors.Add("All rows are already in the database.");
                            return validationErrors;
                        }

                        // No error found, insert to DB
                        await provinceRepository.InsertManyAsync(provinces);
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
