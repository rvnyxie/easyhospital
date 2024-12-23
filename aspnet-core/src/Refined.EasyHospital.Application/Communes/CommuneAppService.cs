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
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Refined.EasyHospital.Communes
{
    /// <summary>
    /// App service implementation for Commune
    /// </summary>
    /// <param name="communeRepository">EF Core commune repository</param>
    /// <param name="communeDapperRepository">Dapper commune repository</param>
    public class CommuneAppService(
        IRepository<Commune, Guid> communeRepository,
        ICommuneDapperRepository communeDapperRepository)
        :
        CrudAppService<Commune, CommuneDto, Guid, LocalityPagedAndSortedResultRequestDto, CommuneCreateDto, CommuneUpdateDto>(communeRepository),
        ITransientDependency,
        ICommuneAppService
    {
        public override async Task<PagedResultDto<CommuneDto>> GetListAsync(LocalityPagedAndSortedResultRequestDto input)
        {
            // Extract pagination and filter parameters
            var search = input.Search;
            var pageSize = input.MaxResultCount;
            var currentPage = input.SkipCount / input.MaxResultCount + 1;

            var communes = await communeDapperRepository.GetManyAsync(search, pageSize, currentPage);

            var communeDtos = await MapToGetListOutputDtosAsync(communes);

            var totalCount = communeDtos.Count;

            return new PagedResultDto<CommuneDto>(
                totalCount,
                communeDtos
            );
        }

        public override async Task<CommuneDto> GetAsync(Guid id)
        {
            var commune = await communeDapperRepository.GetAsync(id);

            var communeDto = await MapToGetOutputDtoAsync(commune);

            return communeDto;
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
                        var headers = new[] { "Mã", "Tên", "Tên Tiếng Anh", "Cấp", "Mã QH", "Quận Huyện" };
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
                        var communes = new List<Commune>();
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
                            var districtCode = worksheet.Cells[row, 5].Text.Trim();

                            // Validate empty rows
                            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(levelText))
                            {
                                validationErrors.Add($"Row {row} contains empty value");
                                continue;
                            }

                            // Validate level values
                            if (!enumMappingHelper.TryMapCommuneLevel(levelText, out var level))
                            {
                                validationErrors.Add($"Row {row}: Invalid level value '{levelText}'. Expected value are 'Xã' or 'Thị trấn'");
                                continue;
                            }

                            // Collect for batch checking
                            codes.Add(code);
                            names.Add(name);
                            englishNames.Add(englishName);

                            // Create new province
                            communes.Add(new Commune
                            {
                                Code = code,
                                Name = name,
                                EnglishName = englishName,
                                Level = level,
                                DistrictCode = districtCode
                            });
                        }

                        if (validationErrors.Any())
                        {
                            return validationErrors;
                        }

                        // Batch-check existing codes and names
                        var existingCommunes = await communeDapperRepository.GetByCodesOrNamesAsync(codes, names);

                        // Filter out existing records
                        communes = communes.Where(p =>
                            !existingCommunes.Any(ep => ep.Code == p.Code || ep.Name == p.Name)).ToList();

                        if (!communes.Any())
                        {
                            validationErrors.Add("All rows are already in the database.");
                            return validationErrors;
                        }

                        // No error found, insert to DB
                        await communeRepository.InsertManyAsync(communes);
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
